namespace ZeldaKata;

public class GameEngine
{
  private int _numberOfKillsWithoutGettingHit;
  private int _globalCounter;
  private int GetAndIncrementGlobalCount()
  {
    var globalCount = _globalCounter++;
    if (_globalCounter > 9)
    {
      _globalCounter = 0;
    }
    return globalCount;
  }

  public CombatResult GetCombatResult(CombatAction combatAction, EnemyGroup enemyGroup)
  {
    if (combatAction == CombatAction.GetHit)
    {
      _numberOfKillsWithoutGettingHit = 0;
      return CombatResult.Nothing;
    }
    _numberOfKillsWithoutGettingHit++;
    int globalCount = GetAndIncrementGlobalCount();
    switch (_numberOfKillsWithoutGettingHit)
    {
      case 10:
        return combatAction == CombatAction.KillEnemyWithBomb ? CombatResult.Bomb : CombatResult.FiveRupees;
      case 16:
        _numberOfKillsWithoutGettingHit = 0;
        return CombatResult.Fairy;
      default:
        return enemyGroup != EnemyGroup.X
          ? GlobalDropTable.Get()[enemyGroup][globalCount]
          : CombatResult.Nothing;
    }
  }
}
