using FluentAssertions;
using FluentAssertions.Execution;
using Xunit;

namespace ZeldaKata;

public class Tests
{
  private readonly GameEngine _engine = new();

  [Fact]
  public void GetFiverOn10thKill()
  {
    CombatResult tenthResult = MakeConsecutiveKills(10);
    tenthResult.Should().Be(CombatResult.FiveRupees);
  }

  [Fact]
  public void GetBombOn10thKillWhen10thKillIsBomb()
  {
    CombatResult tenthResult = MakeConsecutiveKills(10, CombatAction.KillEnemyWithBomb);
    tenthResult.Should().Be(CombatResult.Bomb);
  }

  [Fact]
  public void DoNotGetFiverOn10thKillWhenGettingHitBefore()
  {
    MakeConsecutiveKills(9);
    _engine.GetCombatResult(CombatAction.GetHit, EnemyGroup.A);
    CombatResult tenthKillResult = _engine.GetCombatResult(CombatAction.KillEnemy, EnemyGroup.A);

    tenthKillResult.Should().NotBe(CombatResult.FiveRupees);
  }

  [Fact]
  public void GetNothingInFirst9KillsForGroupX()
  {
    using (new AssertionScope())
    {
      for (int i = 1; i <= 9; i++)
      {
        var combatResult = _engine.GetCombatResult(CombatAction.KillEnemy, EnemyGroup.X);
        combatResult.Should().Be(CombatResult.Nothing, $"kill {i} should result in nothing");
      }
    }
  }

  [Fact]
  public void GetNothingOn11thConsecutiveKillForGroupX()
  {
    MakeConsecutiveKills(10);

    CombatResult eleventhKill = _engine.GetCombatResult(CombatAction.KillEnemy, EnemyGroup.X);

    eleventhKill.Should().Be(CombatResult.Nothing);
  }

  [Fact]
  public void GetFairyOn16thConsecutiveKill()
  {
    CombatResult sixteenth = MakeConsecutiveKills(16);
    sixteenth.Should().Be(CombatResult.Fairy);
  }

  [Fact]
  public void GetNothingOn20thConsecutiveKillForXGroup()
  {
    MakeConsecutiveKills(19);

    CombatResult twentiethKill = _engine.GetCombatResult(CombatAction.KillEnemy, EnemyGroup.X);

    twentiethKill.Should().Be(CombatResult.Nothing);
  }

  [Fact]
  public void GetFiver10KillsAfterFairyDrop()
  {
    CombatResult tenKillsAfterFairy = MakeConsecutiveKills(26);

    tenKillsAfterFairy.Should().Be(CombatResult.FiveRupees);
  }

  [Fact]
  public void GetBomb10KillsAfterFairyDropWhenLastKillIsWithBomb()
  {
    CombatResult tenKillsAfterFairy = MakeConsecutiveKills(26, CombatAction.KillEnemyWithBomb);

    tenKillsAfterFairy.Should().Be(CombatResult.Bomb);
  }

  [Theory]
  [InlineData(EnemyGroup.A)]
  [InlineData(EnemyGroup.B)]
  [InlineData(EnemyGroup.C)]
  [InlineData(EnemyGroup.D)]
  public void GetGlobalFromFirstNineKills(EnemyGroup enemyGroup)
  {
    using (new AssertionScope())
    {
      for (int i = 0; i < 9; i++)
      {
        CombatResult expected = GlobalDropTable.Get()[enemyGroup][i];
        CombatResult combatResult = _engine.GetCombatResult(CombatAction.KillEnemy, enemyGroup);
        combatResult.Should().Be(expected, $"kill {i + 1} for group {enemyGroup} should return global {expected}");
      }
    }
  }

  [Theory]
  [InlineData(EnemyGroup.A)]
  [InlineData(EnemyGroup.B)]
  [InlineData(EnemyGroup.C)]
  [InlineData(EnemyGroup.D)]
  public void GetFirstGlobalOnEleventhKill(EnemyGroup enemyGroup)
  {
    CombatResult expected = GlobalDropTable.Get()[enemyGroup][0];
    MakeConsecutiveKills(10);

    CombatResult eleventhKill = _engine.GetCombatResult(CombatAction.KillEnemy, enemyGroup);

    eleventhKill.Should().Be(expected);
  }

  [Theory]
  [InlineData(EnemyGroup.A)]
  [InlineData(EnemyGroup.B)]
  [InlineData(EnemyGroup.C)]
  [InlineData(EnemyGroup.D)]
  public void GetFirstGlobalOnFirstKillAfterGettingHit(EnemyGroup enemyGroup)
  {
    CombatResult expected = GlobalDropTable.Get()[enemyGroup][0];
    _engine.GetCombatResult(CombatAction.GetHit, enemyGroup);

    CombatResult eleventhKill = _engine.GetCombatResult(CombatAction.KillEnemy, enemyGroup);

    eleventhKill.Should().Be(expected);
  }

  [Theory]
  [InlineData(EnemyGroup.A)]
  [InlineData(EnemyGroup.B)]
  [InlineData(EnemyGroup.C)]
  [InlineData(EnemyGroup.D)]
  public void GetSecondGlobalOnKillAfterGroupXKill(EnemyGroup enemyGroup)
  {
    CombatResult expected = GlobalDropTable.Get()[enemyGroup][1];
    _engine.GetCombatResult(CombatAction.KillEnemy, EnemyGroup.X);

    CombatResult eleventhKill = _engine.GetCombatResult(CombatAction.KillEnemy, enemyGroup);

    eleventhKill.Should().Be(expected);
  }

  [Fact]
  public void GetNothingWhenEnemyGroupIsXForNonForcedDrops()
  {
    using (new AssertionScope())
    {
      for (int i = 1; i <= 25; i++)
      {
        CombatResult result = _engine.GetCombatResult(CombatAction.KillEnemy, EnemyGroup.X);
        if (i is 10 or 16) continue;
        result.Should().Be(CombatResult.Nothing, $"kill {i} with {EnemyGroup.X} should result in no drops");
      }
    }
  }

  private CombatResult MakeConsecutiveKills(int numberOfConsecutiveCombats, CombatAction finalCombatAction = CombatAction.KillEnemy)
  {
    for (int i = 0; i < numberOfConsecutiveCombats - 1; i++)
    {
      _engine.GetCombatResult(CombatAction.KillEnemy, EnemyGroup.A);
    }
    return _engine.GetCombatResult(finalCombatAction, EnemyGroup.A);
  }
}
