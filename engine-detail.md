# The Legend of Zelda Counter Engine Details
Below is a detailed description of how the counters work in Zelda on the NES. It can be used as a reference when attempting the Kata.

## Resources
- A lot more detail is available about the engine (and all of Zelda in general) up on [RedCandle.us Technical Information - Forced Drops](http://redcandle.us/Legend_of_Zelda/Technical_Information#Forced_drops).
- A breakdown of a former world record speedrun that digs into many mechanics of the game (including the drop system and its counters) can be viewed on YouTube: [The Legend of Zelda World Record Speedrun Explained, by Bismuth](https://www.youtube.com/watch?v=ACuk2TXoNhc). I highly recommend giving this a watch for many reasons, but the most relevant one is that the video provides a live, visual representation of the item counting system in action.

## The Global Counter
- Cycles from 0-9
- Advances on kill of any Group A-D enemy
  - The result to return uses current count, before advancing 
- Does not advance on kill of X enemy
- Never resets

## The Consecutive Counter
- Increments once for most Group A-D, X enemies
- 10th Kill drops a reward (Five Rupees, or Bomb if Killed with Bomb)
  - Resets to 0 on reward drop
- If enemy was X enemy, freeze count on 10
  - Drops locked reward and resets to 0 on next Group A-D Kill
- Resets if you Get Hit
- Does not advance for Dodongo Bomb Bomb Kill
- Advances to 10, then resolves as Bomb Streak Bonus for Dodongo Bomb Sword Kill

## The Fairy Counter
- Increments once for most A-D, X enemies
- Does not advance for Dodongo
- After an A-D kill, if this is count is at 16, a Fairy drops
  - Consecutive Counter resets
  - Fairy Counter does not reset
  - This reward overrides any other drop, including consecutive rewards
  - If 16 is skipped over (X enemy Kill, etc.), it is skipped forever
- Resets if you get hit
- Otherwise gets to 255, then NES Limitations returns it to 0
