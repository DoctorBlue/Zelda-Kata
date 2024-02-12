namespace ZeldaKata;

public static class GlobalDropTable
{
    public static Dictionary<EnemyGroup, CombatResult[]> Get() => new()
    {
        { EnemyGroup.A, new[] { CombatResult.Rupee, CombatResult.Heart, CombatResult.Rupee, CombatResult.Fairy, CombatResult.Rupee, CombatResult.Heart, CombatResult.Heart, CombatResult.Rupee, CombatResult.Rupee, CombatResult.Heart } },
        { EnemyGroup.B, new[] { CombatResult.Bomb, CombatResult.Rupee, CombatResult.Clock, CombatResult.Rupee, CombatResult.Heart, CombatResult.Bomb, CombatResult.Rupee, CombatResult.Bomb, CombatResult.Heart, CombatResult.Heart } },
        { EnemyGroup.C, new[] { CombatResult.Rupee, CombatResult.Heart, CombatResult.Rupee, CombatResult.FiveRupees, CombatResult.Heart, CombatResult.Clock, CombatResult.Rupee, CombatResult.Rupee, CombatResult.Rupee, CombatResult.FiveRupees } },
        { EnemyGroup.D, new[] { CombatResult.Heart, CombatResult.Fairy, CombatResult.Rupee, CombatResult.Heart, CombatResult.Fairy, CombatResult.Heart, CombatResult.Heart, CombatResult.Heart, CombatResult.Rupee, CombatResult.Heart } }
    };
}
