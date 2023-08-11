using FluentAssertions;
using Xunit;

namespace ZeldaKata.GameEngine2Tests;

public class ConsecutiveCounter
{
  private readonly GameEngine2 _engine = new();

  [Fact]
  public void RewardsFiveRupeesOn10KillStreak()
  {
    const CombatResult expectedCombatResult = CombatResult.FiveRupees;
    for (int i = 0; i < 9; i++)
    {
      _engine.GetCombatResult(CombatAction.KillEnemy, EnemyGroup.X);
    }

    CombatResult combatResult = _engine.GetCombatResult(CombatAction.KillEnemy, EnemyGroup.A);

    combatResult.Should().Be(expectedCombatResult);
  }

  [Fact]
  public void RewardsBombOn10KillStreakWhen10thEnemyHasTheBomb()
  {
    const CombatResult expectedCombatResult = CombatResult.Bomb;
    for (int i = 0; i < 9; i++)
    {
      _engine.GetCombatResult(CombatAction.KillEnemy, EnemyGroup.X);
    }

    CombatResult combatResult = _engine.GetCombatResult(CombatAction.KillEnemyWithBomb, EnemyGroup.A);

    combatResult.Should().Be(expectedCombatResult);
  }

  [Theory]
  [InlineData(CombatAction.KillEnemy, CombatResult.FiveRupees)]
  [InlineData(CombatAction.KillEnemyWithBomb, CombatResult.Bomb)]
  public void LocksBonusUntilNonXEnemyKilled(CombatAction combatAction, CombatResult expectedCombatResult)
  {
    for (int i = 0; i < 9; i++)
    {
      _engine.GetCombatResult(CombatAction.KillEnemy, EnemyGroup.X);
    }

    _engine.GetCombatResult(combatAction, EnemyGroup.X);
    _engine.GetCombatResult(CombatAction.KillEnemy, EnemyGroup.X);
    _engine.GetCombatResult(CombatAction.KillEnemyWithBomb, EnemyGroup.X);
    CombatResult unlockedRewardResult = _engine.GetCombatResult(combatAction, EnemyGroup.C);

    unlockedRewardResult.Should().Be(expectedCombatResult);
  }

  [Fact]
  public void ResetsWhenGettingHit()
  {
    for (int i = 0; i < 9; i++)
    {
      _engine.GetCombatResult(CombatAction.KillEnemy, EnemyGroup.X);
    }
    _engine.GetCombatResult(CombatAction.GetHit, EnemyGroup.X);

    CombatResult combatResult = _engine.GetCombatResult(CombatAction.KillEnemyWithBomb, EnemyGroup.C);

    combatResult.Should().NotBe(CombatResult.Bomb);
  }

  [Fact]
  public void ResetsOnReward()
  {
    const CombatResult expectedCombatResult = CombatResult.FiveRupees;
    for (int i = 0; i < 10; i++)
    {
      _engine.GetCombatResult(CombatAction.KillEnemy, EnemyGroup.A);
    }
    for (int i = 0; i < 9; i++)
    {
      _engine.GetCombatResult(CombatAction.KillEnemyWithBomb, EnemyGroup.X);
    }

    CombatResult twentiethCombatResult = _engine.GetCombatResult(CombatAction.KillEnemy, EnemyGroup.A);

    twentiethCombatResult.Should().Be(expectedCombatResult);
  }

  [Fact]
  public void ClearsLockedRewardOnHit()
  {
    const CombatResult expectedCombatResult = CombatResult.FiveRupees;
    for (int i = 0; i < 12; i++)
    {
      _engine.GetCombatResult(CombatAction.KillEnemyWithBomb, EnemyGroup.A);
    }
    _engine.GetCombatResult(CombatAction.GetHit, EnemyGroup.A);
    for (int i = 0; i < 9; i++)
    {
      _engine.GetCombatResult(CombatAction.KillEnemy, EnemyGroup.A);
    }

    CombatResult twentiethCombatResult = _engine.GetCombatResult(CombatAction.KillEnemy, EnemyGroup.A);

    twentiethCombatResult.Should().Be(expectedCombatResult);
  }
}
