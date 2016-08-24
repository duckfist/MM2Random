Mega Man 2 Randomizer
by duckfist
version 0.01 beta 1

Contributors:
Binarynova
RaneOfSOTN

-----------------------------------------------------------------------------------------------

Features

- Random Robot Master Stages: The 8 robot master stages are shuffled.  The boss portraits on
the Stage Select screen point to different stages.

- Random Weapons: The weapons awarded by defeating a Robot Master are shuffled.

- Random Items 1, 2, and 3: Which Robot Master awards each of the 3 Items is shuffled.

- Random Weaknesses: The damage done by each weapon against each Robot Master is changed. Wily
bosses except Wily 2, Wily 4, and Wily 5 bosses are also changed. Some weapons that previously
healed bosses or caused special AI behavior have not been changed yet.

- Random BGM: The background music for all stages have been shuffled.

- Random Weapon Names: In the Weapon Get screen, the weapon name and letter will be randomized.
This letter is not used in the pause menu, however.

- Random Wily 5 Teleporters: The Robot Masters inside of each Wily 5 teleporter has been
shuffled.

- Random Enemies: The enemy IDs for most enemy instances have been changed. Sprite banks for
each room in each level are modified to support different enemy combinations appearing. Several
enemies are not yet supported, and several instances are not able to be changed yet.
    
- Fast Text: To compensate for the U version being slower, text delay is increased from 7
frames to 4 frames.

- Burst Chaser Mode: Increase Mega Man's movement speed, and reduce a few other delay timers.

-----------------------------------------------------------------------------------------------

Enemy Randomizer notes

- Yoku blocks in Heatman still appear in their ususal spots, slightly reducing enemy variety in
Heat stage
- Goblins still appear in Airman rooms 1 and 3, thus limiting the sprite banks used in those 
rooms and thus enemy variety.
- Petit Goblins have messed up graphics at the moment.
- Lightning Goros still appear in Airman room 1 in order to make crossing the gap possible, 
therefore all of Airman stage is rather plain.
- Frienders (wolves) still spawn like normal, due to dependence on solid blocks in their rooms.
- Big Fish (wily 3 fish) do not spawn anywhere yet, TODO.
- Shrink (shrimp) do not spawn naturally anywhere yet, TODO.
- Anko (anglers in bubble stage) still spawn normally because of some special programming in 
the stage. Note that spawned Shrinks will have messed up graphics for now.
- M-445 enemies (metroids on bubble stage) do not spawn anywhere. Their gfx behave strangely in
most stages, needs more work.
- Changkey Makers (fire guys on quick stage) do not spawn anywhere. Their dependence on palette
changes make it difficult to implement.
- Springer do not spawn anywhere yet. When enabled, they somtimes have a gfx issue and also 
sometimes fail to spawn.
- I have restricted Moles (drills), Pipis (birds), and Presses (crushers) in the number of 
sprite banks (rooms) that they can appear in, hopefully reducing frustration.
- Moles and Presses are rendered behind the background in some stages, so they have been 
limited in the stages that they can appear.
- "Deactivator" objects never spawn. Thus, infinitely spawning enemies like Moles, Pipis, and 
Claws will continue to spawn until you leave the room.
- If you like to do zip glitches, be careful around the Matasaburo enemy (fan guy). You can get
stuck in the wall while one is on screen.  If you can't kill them, use Time-Stopper to resume 
zipping.
- Acid still appears like normal in Wily 6. Although, I could easily replace them with tons of 
enemies. Do you really want that?

-----------------------------------------------------------------------------------------------

Other Known Bugs

- In the Weapon Get screen, the weapon letter shown will not be the same weapon you see in the
pause menu.
- Bosses are still healed by the original weapons that they are healed by, regardless of the
output of the Weaknesses Randomizer.  That is, Heat Man will continue to be healed by Atomic
Fire and Clash Bomber, Bubble Man will be healed by Bubble Lead, and Wood Man will be healed by
Leaf Shield. Alien Wily is the only exception I know of.
  
-----------------------------------------------------------------------------------------------
  
Changelog

v0.1.0 (Beta 1)
- Stage and weapon color randomizer
- Enemy randomizer
- BGM randomizer
- Weapon Get text randomizer
- Title Screen and Stage Select Screen graphics changed
- Display seed and version number on Title Screen
- Bug fix: Broken Wily 5 teleporters
- Bug fix: Wrong portraits being blacked out on stage select
- Bug fix: Weaknesses on (U) Difficult Mode didn't update

v0.0.6
- Bug fix: No longer softlock in Wily 5 whenever "Randomize Weapons" is enabled
- Support for random Robot Master teleporter locations during Wily 5 refights

v0.0.5
- Mega Man 2 (U) version is now supported.
- NOTE: Random Weaknesses will not work in the (U) version when playing on Difficult Mode

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
