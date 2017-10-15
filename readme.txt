Mega Man 2 Randomizer
by duckfist
version 0.3.3 beta

-----------------------------------------------------------------------------------------------

Featured Randomizer Modules

- Robot Master Portraits: The boss portraits on the Stage Select point to different stages.

- Robot Master Spawn: Boss rooms may contain a different boss.

- Weapon Behavior: Movement properties and ammo usage of each weapon are changed.

- Enemy Weaknesses: Damage done to each enemy by each weapon is changed.

- Boss Weaknesses: The damage done by each weapon against each boss is changed.
  > Two Robot Masters take 2x damage from Buster.
  > Every Robot Master will have a primary weakness and secondary weakness. 
  > Damage values are, on average, lower than in vanilla Mega Man 2.
  > Note that an uncharged Heat does Buster damage, and a fully charged Heat does Heat damage.
  > Wily bosses are also affected, including Boobeam Trap and its Barriers.
  > There is a 25% chance for each Wily boss to take damage from buster.
  > Time Stopper will never do damage to Wily bosses.
  > Damage is scaled based on Ammo consumption

- Boss AI: Properties of Robot Master attack patterns are changed.

- Weapon Get: The weapons awarded from defeating Robot Masters are shuffled.

- Items 1, 2, and 3 Get: Which Robot Master awards each of the 3 Items is shuffled.

- Wily 5 Teleporters: The Robot Masters inside of each Wily 5 teleporter has been shuffled.

- Enemy Types: The enemy IDs for most enemy instances have been changed. 
  > Sprite banks for each room are modified to support different enemy combinations appearing.
  > A few enemies are not yet supported.

- Colors: Stage backgrounds, Robot Master colors, Mega Man's weapon colors, and a few Wily
bosses have randomly assigned colors.

- BGM: The background music for all stages have been shuffled.

- Random Text:
  > The story in the intro cutscene will be different.
  > In the Weapon Get screen, the weapon name and letter will be randomized.
    (This letter is not used in the pause menu, however)

- And a few surprises

Other Features

- Fast Text: To compensate for the U version being slower, text delay is increased from 7
frames to 4 frames.

- Burst Chaser Mode: Increase Mega Man's movement speed, and reduce a few other delay timers.

- Additional large weapon energy refills provided in Wily 5 and Wily 6

-----------------------------------------------------------------------------------------------

Enemy Randomizer notes

- Yoku blocks in Heatman still appear in their ususal spots, slightly reducing enemy variety in
Heat stage
- Goblins have been removed from Airman stage to give possible enemies more variety.
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

v0.3.3-beta
- Random Robot Masters in boss rooms
- Modified credits sequence with damage table
- Music no longer changes during Wily 5 refights
- Bug fix: Bubbleman softlock, shouldn't zip through the floor anymore
- (Enemy) Adjusted some air enemy spawn positions in Heat and Wily1
- (Enemy) Friender now always takes 1 damage from Buster
- (Boss) Bubbleman fight now has new randomized parameters
- (Stage) Added 1 extra Large Weapon Energy to Wily 5 and Wily 6
- (Text) Added 4 new intro stories
- (Colors) Intro, Credits, Start, Password, Weapon Get, Dragon, and Gutsdozer

v0.3.2-beta
- Enemy Weakness Randomizer
- (Text) Intro splash screen now has some fun stuff

v0.3.1-beta
- Embedded external files as resources. All you need now is the .exe (...and the ROM...)
- Bug fix: Generating multiple ROMs with a set seed should now be consistently random

v0.3.0-beta
- New title screen
- Randomized intro cutscene story
- Renamed difficulties from Difficult/Normal to Normal/Easy
- Cursor now defaults on Normal (formerly Difficult)
- Disabled most options due to compatibility problems. The options suck anyways, everything
  should just be kept on. I will ponder more actually useful options in the future.
- (Boss) Increased damage to Picopico-kun:
- - ammoUse x 10 for the main weakness
- - ammoUse x 6 for secondary weakness
- - ammoUse x 3 for tertiary weakness

v0.2.2-beta
- ROM file is no longer included
- (Stage) Added a Large Weapon Energy to Wily 5
- (Weapon) Fully charged Heat won't cost more than 7 energy now
- (Enemy) Only at most one type of Activator enemy will appear in any given room
- (Enemy) Deactivator objects will now spawn at the end of rooms containing Activators
- (Enemy) Fixed a spawn y-pos in Flashman stage
- (Enemy) Press now take 1 damage from buster
- (Enemy) Clash Barriers are now vulnerable to a random weapon
- (Boss) Fixed a pattern for Metalman
- (Boss) Added another pattern for Woodman (1/3 chance)
- (Boss) Increased damage to Picopico-kun
- (Boss) Boobeam Trap is now vulnerable to a random wepaon
- (Boss) Wily Machine now has 4 weaknesses instead of 3, but Phase 1 will disable 2 of the
  weaknesses, and Phase 1 will disable 1 weakness, similar to the original game
- Bug fix (Enemy) Goblins removed from Airman room 3 and replaced with random spawns
- Bug fix (Enemy) With deactivators being placed, hopefully no more glitchy spawns
- Bug fix (Boss) Heatman is no longer always healed by Atomic Fire
- Bug fix (Boss) Flashman is no longer always healed by Time-Stopper

v0.2.1-beta
- Added missing file

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
