# The Legend of Zelda Combat Kata
Blah blah description here.

# TODO

1. Separate existing implementations into subfolders (copy any changed files to it as their own solutions)
2. Create "Starting Point" template (the root project? Own folder?)
3. Separate first four steps of Kata from the rest (Step 5 onwards is 'advanced')
4. Description of the Kata at the top of this file

## Let's Begin!
Use the solution within the `Start` folder to get started. (TODO: Describe the files provided and the files that should be edited.)

### The Kata
1. Always Return Nothing
2. Consecutive Streak Bonus - Kill 10 enemies in a row to get a Five Rupee on the 10th Kill
   - Streak is broken if `CombatAction.GetHit` occurs or when the bonus is reached
3. 10th Enemy has the Bomb - If the 10th enemy is killed with a bomb, a Bomb is returned instead
4. Fairy Streak Bonus - Kill 16 enemies in a row to get a Fairy on the 16th Kill
   - Fairy Streak is broken if `CombatAction.GetHit` occurs or when the bonus is reached
   - This also breaks the Consecutive Streak
   
### Advanced Steps
5. Global Index For Non-Bonus Item Drops
   - Instead of returning Nothing most of the time, use `GlobalDropTable` to find an item to return
     - Any Bonus rewards take priority over this drop
   - Killing an enemy in group X continues to return Nothing and does not require a lookup
   - `GlobalDropTable` requires the `EnemyGroup` from the given `CombatEvent`, and an index value (0-9)
   - The global index value is defined as follows:
     - Initiated to 0 and loops from 0-9, and is never Reset
     - Increments by 1 when a Kill occurs
       - Exception: Kills of EnemyGroup X do not Increment this index
       - Increment after after looking up a Global to return, not before
         - E.g. For a first kill `CombatEvent` with `EnemyGroup` B, return a Bomb (index 0), not a Rupee (index 1)
6. Global Drop Odds - When getting a drop from a Global, there is no guarantee an item is returned
   - Odds of a global drop vary by group:
       - Group A: `80/256`
       - Group B: `104/256`
       - Group C: `152/256`
       - Group D: `104/256`
   - Return Nothing on failure
7. Consecutive Streak Bonus Delay - If the 10th enemy was killed and is in `EnemyGroup` X:
   - Return Nothing like any `EnemyGroup` X Kill
   - Carry the Streak Bonus to the next Kill
   - Repeat process if next kill is also in `EnemyGroup` X
   - If first non-`EnemyGroup` X kill is the 16th Fairy Streak Bonus kill, Fairy gets priority over carried-over Reward
   - Streak breaks as normal, when a Streak Bonus (Consecutive or Fairy) is reached
     - Breaking a Fairy Streak continues to break the Consecutive Streak
8. Fairy Streak Bonus Skip - If the 16th enemy was killed and is in `EnemyGroup` X:
   - Return Nothing
   - The Fairy Streak Bonus is skipped, and no Streaks are broken
   - Note: A Fairy Streak Bonus occurs *only* at the 16th consecutive kill, not every 16th (i.e. 32nd, 48th)
9. New Combat Event: `KillSmokedDodongo`
   - The Global Index is incremented
   - The Consecutive Streak, instead of incrementing is **set to 10**
   - The Fairy Consecutive Streak is **not incremented**
   - A Bomb should be returned, as the Consecutive Streak is at 10, and Dodongo is full of Bombs to drop
10. New Combat Event: `KillDodongoWithBombs`
    - The Global Index is incremented
    - The Consecutive Streak is **not incremented**
    - The Fairy Consecutive Streak is **not incremented**
      - If the Fairy Consecutive Streak previously reached 16, it **remains at 16, rewarding a fairy again**
      - If multiple `KillDodongoWithBombs` `CombatActions` occur, more Fairy items drop
11. Combined Event: `KillAndGetHit`
    - Enemy killed, but before any Drop is given, a hit was taken
    - Global advances, but like with the `GetHit` `CombatAction`, all Streaks are lost
    - Global drop (or nothing for `EnemyGroup` X) is returned since no Bonus occurs
12. NES Limitations
    - All numbers within the `GameEngine` class cannot exceed 255
      - If a number would exceed 255, it rolls back over to 0 instead
    - Consequence: Since Fairy Streak Bonus only resets on hit, it can wrap all the way back around to 16 to force another Fairy

## The Legend of Zelda Counter Engine Details
Below is a detailed description of how the counters work in Zelda on the NES. It can be used as a reference when attempting the Kata.

### The Global Counter
- Cycles from 0-9
- Advances on kill of any Group A-D enemy
  - The result to return uses current count, before advancing 
- Does not advance on kill of X enemy
- Never resets

### The Consecutive Counter
- Increments once for most Group A-D, X enemies
- 10th Kill drops a reward (Five Rupees, or Bomb if Killed with Bomb)
  - Resets to 0 on reward drop
- If enemy was X enemy, freeze count on 10
  - Drops locked reward and resets to 0 on next Group A-D Kill
- Resets if you Get Hit
- Does not advance for Dodongo Bomb Bomb Kill
- Advances to 10, then resolves as Bomb Streak Bonus for Dodongo Bomb Sword Kill

### The Fairy Counter
- Increments once for most A-D, X enemies
- Does not advance for Dodongo
- After an A-D kill, if this is count is at 16, a Fairy drops
  - Consecutive Counter resets
  - Fairy Counter does not reset
  - This reward overrides any other drop, including consecutive rewards
  - If 16 is skipped over (X enemy Kill, etc.), it is skipped forever
- Resets if you get hit
- Otherwise gets to 255, then NES Limitations returns it to 0

## Currently Out of Scope of the Kata
Here are some details of the engine that are presently not part of the Kata, but could be potentially added in the future.

- Differentiating "Spawning X Group Enemies" (which advance the global) from "regular X Group Enemies" (which do not)
- Killing multiple enemies at once, and getting e.g. multiple fairies
- Overkill (i.e. multiple kill counts within a single frame)
- Killing a ringleader which causes other enemies in the room to die and drop globals
