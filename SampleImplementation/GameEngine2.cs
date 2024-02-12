namespace ZeldaKata;

public class GameEngine2
{
  private int _globalCount;
  private int _consecutiveCount;
  private int _fairyCount;
  private CombatResult? _lockedResult;

  private void IncrementGlobalCount()
  {
    _globalCount++;
    if (_globalCount > 9) _globalCount = 0;
  }

  private void IncrementConsecutiveCount()
  {
    _consecutiveCount++;
    _consecutiveCount = Math.Min(_consecutiveCount, 10);
  }

  private void ResetConsecutiveCount()
  {
    _consecutiveCount = 0;
    _lockedResult = null;
  }

  public CombatResult GetCombatResult(CombatAction combatAction, EnemyGroup enemyGroup)
  {
    if (combatAction == CombatAction.GetHit)
    {
      ResetConsecutiveCount();
      _fairyCount = 0;
      return CombatResult.Nothing;
    }
    CombatResult globalCombatResult = enemyGroup != EnemyGroup.X
      ? GlobalDropTable.Get()[enemyGroup][_globalCount]
      : CombatResult.Nothing;
    if (enemyGroup != EnemyGroup.X)
    {
      IncrementGlobalCount();
    }
    IncrementConsecutiveCount();
    _fairyCount++;
    if (_fairyCount == 16 && enemyGroup != EnemyGroup.X)
    {
      ResetConsecutiveCount();
      return CombatResult.Fairy;
    }
    if (_consecutiveCount != 10) return globalCombatResult;
    CombatResult streakResult = combatAction == CombatAction.KillEnemyWithBomb
      ? CombatResult.Bomb
      : CombatResult.FiveRupees;
    if (enemyGroup == EnemyGroup.X)
    {
      _lockedResult ??= streakResult;
      return CombatResult.Nothing;
    }
    CombatResult result = _lockedResult ?? streakResult;
    ResetConsecutiveCount();
    return result;
  }
}
