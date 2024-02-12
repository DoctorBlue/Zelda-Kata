using FluentAssertions;
using FluentAssertions.Execution;
using Xunit;

namespace ZeldaKata.Tests.GameEngine2Tests;

public class Global
{
  private readonly GameEngine2 _engine = new();

  [Theory]
  [InlineData(EnemyGroup.A)]
  [InlineData(EnemyGroup.B)]
  [InlineData(EnemyGroup.C)]
  [InlineData(EnemyGroup.D)]
  public void IsReturnedFor10NonConsecutiveKills(EnemyGroup enemyGroup)
  {
    using (new AssertionScope())
    {
      for (int globalCount = 0; globalCount <= 9; globalCount++)
      {
        CombatResult expectedCombatResult = GlobalDropTable.Get()[enemyGroup][globalCount];
        CombatResult combatResult = _engine.GetCombatResult(CombatAction.KillEnemy, enemyGroup);
        combatResult.Should().Be(expectedCombatResult, $"the global at {globalCount} for {enemyGroup} should match the global table entry");
        _engine.GetCombatResult(CombatAction.GetHit, EnemyGroup.X);
      }
    }
  }

  [Theory]
  [InlineData(EnemyGroup.A)]
  [InlineData(EnemyGroup.B)]
  [InlineData(EnemyGroup.C)]
  [InlineData(EnemyGroup.D)]
  public void IsNotAdvancedWhenNotGettingDrop(EnemyGroup enemyGroup)
  {
    CombatResult expectedCombatResult = GlobalDropTable.Get()[enemyGroup][0];
    _engine.GetCombatResult(CombatAction.GetHit, enemyGroup);
    _engine.GetCombatResult(CombatAction.KillEnemy, EnemyGroup.X);
    _engine.GetCombatResult(CombatAction.KillEnemyWithBomb, EnemyGroup.X);

    CombatResult combatResult = _engine.GetCombatResult(CombatAction.KillEnemy, enemyGroup);

    combatResult.Should().Be(expectedCombatResult);
  }

  [Theory]
  [InlineData(EnemyGroup.A)]
  [InlineData(EnemyGroup.B)]
  [InlineData(EnemyGroup.C)]
  [InlineData(EnemyGroup.D)]
  public void LoopsBackTo0WhenPassing9OnGlobal(EnemyGroup enemyGroup)
  {
    CombatResult expectedCombatResult = GlobalDropTable.Get()[enemyGroup][2];
    for (int i = 0; i <= 11; i++)
    {
      _engine.GetCombatResult(CombatAction.KillEnemy, EnemyGroup.B);
      _engine.GetCombatResult(CombatAction.GetHit, EnemyGroup.X);
    }

    CombatResult combatResult = _engine.GetCombatResult(CombatAction.KillEnemy, enemyGroup);

    combatResult.Should().Be(expectedCombatResult);
  }

  [Theory]
  [InlineData(EnemyGroup.A)]
  [InlineData(EnemyGroup.B)]
  [InlineData(EnemyGroup.C)]
  [InlineData(EnemyGroup.D)]
  public void DoesNotResetOnGetHit(EnemyGroup enemyGroup)
  {
    CombatResult expectedCombatResult = GlobalDropTable.Get()[enemyGroup][4];
    _engine.GetCombatResult(CombatAction.KillEnemy, EnemyGroup.A);
    _engine.GetCombatResult(CombatAction.KillEnemyWithBomb, EnemyGroup.B);
    _engine.GetCombatResult(CombatAction.KillEnemy, EnemyGroup.C);
    _engine.GetCombatResult(CombatAction.KillEnemyWithBomb, EnemyGroup.D);

    _engine.GetCombatResult(CombatAction.GetHit, EnemyGroup.D);
    CombatResult combatResult = _engine.GetCombatResult(CombatAction.KillEnemy, enemyGroup);

    combatResult.Should().Be(expectedCombatResult);
  }
}
