Mega Man 2 Randomizer
by duckfist
version 0.2.0 beta

-----------------------------------------------------------------------------------------------

Featured Randomizer Modules

- Robot Master Order: The 8 robot master stages are shuffled.  The boss portraits on
the Stage Select screen point to different stages.

- Weapon Behavior: Movement properties and ammo usage of each weapon are changed.

- Boss Weaknesses (CHAOS MODE): The damage done by each weapon against each boss is
changed, except for Wily 4 boss. Two Robot Masters take 2x damage from Buster.  Every Robot
Master will have a primary weakness and secondary weakness. Damage values are, on average,
lower than in vanilla Mega Man 2.

- Boss Weaknesses (EASY MODE): The damage done by each weapon against each Robot Master is
shuffled, so that all weapons remain as effective as in Mega Man 2. Wily 2 and Wily 4 boss
remain unchanged.

- Boss AI: Properties of Robot Master attack patterns are changed.

- Weapon Get: The weapons awarded from defeating Robot Masters are shuffled.

- Items 1, 2, and 3 Get: Which Robot Master awards each of the 3 Items is shuffled.

- Wily 5 Teleporters: The Robot Masters inside of each Wily 5 teleporter has been shuffled.

- Enemies: The enemy IDs for most enemy instances have been changed. Sprite banks for
each room in each level are modified to support different enemy combinations appearing. A few 
enemies are not yet supported.

- Colors: Stage backgrounds, Robot Master colors, Mega Man's weapon colors, and a few Wily
bosses have randomly assigned colors.

- BGM: The background music for all stages have been shuffled.

- Random Weapon Names: In the Weapon Get screen, the weapon name and letter will be randomized.
This letter is not used in the pause menu, however.

- Fast Text: To compensate for the U version being slower, text delay is increased from 7
frames to 4 frames.

- Burst Chaser Mode: Increase Mega Man's movement speed, and reduce a few other delay timers.

-----------------------------------------------------------------------------------------------

Enemy Randomizer notes

- Yoku blocks in Heatman still appear in their ususal spots, slightly reducing enemy variety in
Heat stage
- Goblins have been removed from the first room of Airman stage to give possible enemies more
variety. Their drills still appear glitched in the third room for now.
- Lightning Goros still appear in Airman room 1 in order to make crossing the gap possible, 
therefore all of Airman stage is rather plain.
- Frienders (wolves) still spawn like normal, due to dependence on solid blocks in their rooms.
- Big Fish (wily 3 fish) do not spawn anywhere yet, TODO.
- Anko (angler in bubble stage) do not spawn anymore.
- M-445 enemies (metroids on bubble stage) do not spawn anywhere. Their gfx behave strangely in
most stages, needs more work.
- Changkey Makers (fire guys on quick stage) do not spawn anywhere. Their dependence on palette
changes make it difficult to implement.
- I have restricted Moles (drills), Pipis (birds), and Presses (crushers) in the number of 
areas that they can appear in, hopefully reducing frustration.
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

Other Known Bugs:

https://github.com/duckfist/MM2Random/issues?q=is%3Aopen+is%3Aissue+label%3Abug
  
-----------------------------------------------------------------------------------------------
  
Development Tools

- FCEUX 2.2.3 http://www.fceux.com/web/home.html
- visine 2.8.2 by -=Fx3=- http://www.romhacking.net/utilities/172/
- Rockman 2 Editor 1.0 by Rock5easily http://www.romhacking.net/utilities/836/
- Tile Molestor Mod 0.19 http://www.romhacking.net/utilities/991/
- Visual Studio Community 2015

Special thanks to Binarynova and RaneofSoTN for their code contributions  

-----------------------------------------------------------------------------------------------
  
Changelog

v0.2.0-beta
- Weakness Randomizer "Chaos Mode"
- Weapon Behavior Randomizer
- Robot Master AI Randomizer
- Randomized false-floors in Wily 4
- (Enemy) Added several more potential spawn locations
- (Enemy) Added alternate spawn locations for certain enemy types
- (Enemy) Added "Facing direction" to each spawn location
- (Enemy) Removed Goblins from first room of Airman stage
- (Enemy) Overall improved the algorithm and increased variety.
- (Enemy) Added Shrink, Shrink Spawner, and Springer spawns.
- (Enemy) Reduced Telly and Springer spawn rate
- (Color) Robot Masters, Picopico-kun, Wily Machine, and Alien now have color
- Bug fix (Enemy): Adjusted some bad spawn locations
- Bug fix (Enemy): Moles and Presses no longer spawn in Woodman Pipi room
- Bug fix (Enemy): Graphics in Bubbleman stage don't break anymore
- Bug fix (Enemy): Stopped Springer and Blocky from spawning underwater
- Bug fix (Color): Changed ugly colors in Metalman stage
- Bug fix (Color): Fixed Wily 6 palettes
- Bug fix (Color): Atomic Fire palette now works properly

v0.1.1-beta1
- Added a Large Weapon Capsule to Wily 6 stage
- Balance: Removed Pipi from spawning in Heat Yoku block rooms, due to tendancy of despawning
- Balance: Added flat chance to skip any Pipi and Mole instance
- Bug fix: Wily 5 music now properly resumes when exiting a Robot Master teleporter
- Bug fix: Pipi and Mole will now spawn more frequently past Airman stage

v0.1.0-beta1
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
