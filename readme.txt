Mega Man 2 Randomizer
by duckfist

-----------------------------------------------------------------------------------------------

Features

- Random Robot Master Stages: The 8 robot master stages are shuffled.  The boss portraits on
the Stage Select screen remain in the same positions, but the stages that they go to are 
random.

- Random Weapons: The weapons awarded by defeating a Robot Master are shuffled.

- Random Items 1, 2, and 3: Which Robot Master awards each of the 3 Items is shuffled.

- Random Weaknesses: The damage done by each weapon against each Robot Master is changed.
There are two varieties of Random Weaknesses:
    - Easy: The damage table for each weapon is shuffled independently of each other.  For
    example, in Rockman 2, the Air Shooter originally does 2 damage to Heat Man and Quick Man,
    4 damage to Wood Man, 10 damage to Clash Man, and 0 damage to everyone else.  With this
    randomizer setting and using Air Shooter, two random bosses will take 2 damage, one random
    boss will take 4, one will take 10, and the rest will take 0.
    - Hard: The damage tables for all weapons against all Robot Masters are given random
    values within practical tolerances.  For example, Air Shooter might instantly kill Metal
    Man, deal 1 damage to Bubble Man, and fully heal all other Robot Masters.  However, the
    Mega Buster will do 1 damage against 6 of the Robot Masters, and 2 damage against the
    remaining 2.
    
-----------------------------------------------------------------------------------------------

Known Bugs

- You cannot beat Wily 5.  The teleporters are cleared in the wrong order as a side effect of
randomizing the weapons.
- In the Weapon Get screen, the wrong weapon will be shown. It just shows the original weapon
awarded by that Robot Master.
- On the Stage Select screen, the wrong Robot Master portraits will be marked out after
completing a level.  The portraits are marked out according to which weapons you currently
have. The Stage Select screen still operates normally, however, only allowing you to select
stages you haven't played yet.
- Bosses are still healed by the original weapons that they are healed by, regardless of the
output of the Weaknesses Randomizer.  That is, Heat Man will continue to be healed by Atomic
Fire and Clash Bomber, Bubble Man will be healed by Bubble Lead, and Wood Man will be healed by
Leaf Shield.
  
-----------------------------------------------------------------------------------------------
  
Changelog

v0.0.4
- Support for random Robot Master weaknesses, "Easy" option only.
- Rockman 2 version ONLY now, until the damage table offsets for Mega Man 2 are included.

v0.0.3
- Bug fix: No, seriously, Item 3 is being awarded now

v0.0.2
- Random Weapons added
- Bug fix: Item 3 wasn't being awarded, now it is

v0.0.1
- First iteration
- Support for random Robot Master stages added
- Support for random Items 1, 2, and 3 added
- Supports Rockman 2 (J) and Mega Man 2 (U)
- Created GUI for generating ROMs with a given seed or random seed