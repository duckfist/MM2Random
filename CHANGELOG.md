CHANGELOG

v0.5.0-beta
- When at full HP, prevent E-tank consumption
- Added "Great" and "Ultimate" weaknesses to Robot Masters
- Added random colors to the Wily Map screen
- Added Protoman, Roll, and Bass sprite replacement options
- Added the display of flags on the title screen
- The pause menu now displays different weapon letters
- Modified title screen text spacing
- Reduced the time spent on the Wily Map screen
- Wily 6 now plays a random stage song
- Now instantly teleport out of a boss chamber upon victory
- Fixed Bubbleman's boss room issue of zipping out-of-bounds (thanks to the above)
- Fixed Clashman's Clash Bomber X-velocity
- Fixed Game Over theme glitching
- Fixed Metal Blades sometimes being invisible in Airman's boss room
- Removed Tournament Mode for now; a more generic spoiler-free mode will be available later
- Converted MM2RandoLib from .NET Framework 4.5 to .NET Standard 2.0.
- Updated MM2RandoHost from .NET Framework 4.5 to .NET Framework 4.7.2.
- Lots of code refactoring

v0.4.1-beta
- Re-enabled user customizable flags, removing old obsolete flags and adding new ones
- Added flag to hide stage names from the stage select screen
- Created a partition of "Cosmetic Flags" that don't impact randomization logic
- Added a "Tournament Mode", which selects preset flags, disables spoilers, and changes seed
- Now requires the user to provide a ROM again. Sorry!
- Added checksum validation of user's ROM, with 2 different MM2 dumps supported for now
- Redesigned WPF GUI: New layout, custom styles, bindings
- Updated assemblies from .NET Framework 4.0 to 4.5.

v0.4.0-beta
- E-Tanks in your inventory now persist through Game Overs
- (Stage Select): The boss name beneath each portrait now corresponds to the stage it points to
- (Stage - Airman): Removed first room's Lightning Goro requirement
- (Stage - Airman): Redesigned first room with static platforms and adjusted enemy spawns
- (Stage - Flashman): Edited Flashman's boss room to have no ceiling (fixes Airman behavior)
- (Stage - Quickman): Fixed bug that accidentally changed the velocity of Quick Beams
- (Boss - Woodman): Nerf - Reduce leaf fall-speed so he does't jump as quickly 
- (Boss - Woodman): Nerf - Reduce chance of parameters that produce more difficult patterns
- (Boss - Airman): Reduced max Y-velocity for both jumps
- (Weapon) Buster now does 2 damage against 4 Robot Masters instead of only 2
- (Enemy): Add Changkey Maker enemy type back in and disabled its palette change behavior
- (Enemy): Made "auxiliary" enemy types have consistent weaknesses (i.e. Shotman, Shrink, Mole)
- (Enemy): Finally corrected all instances of enemies appearing with glitched graphics 
- (Enemy): Glitched M-445s no longer suddenly spawn where they shouldn't
- (Text): Increased the speed of the Item Get cutscene, skipping Dr. Light's extra text
- (Text): Added 6 new intro cutscene paragraphs, bringing the total to 20.
- (Colors): The "Stage-Selected Boss Intro" screen now has some random colors
- (Music): Disabled legacy music randomizer
- (Music): Removed Credits 2 song, Weapon Get song, and Start/Password song
- (Music): A random stage song is played during "Credits 2"
- (Music): "Start/Password" and "Weapon Get" scenes now just play the Stage Select song
- (Music): 11 stage songs are chosen instead of 10, with Wily 5 taking the unique extra song
- (Music): Fixed bug in parsing songs with no noise channel, fixing 2 tracks
- (Music): Fixed some songs and added many others, bringing the track total to 102! This
includes most songs from Mega Man, Bionic Commando, and Legendary Wings.

v0.3.7-beta
- Bug fix: Fixed glitchy instant-charge Atomic Fire (sometimes crashed the game)
- (Enemy): Added several restrictions on M-445 spawning, should be easier now

v0.3.6-beta
- Exported the randomizer to a .dll
- Bug fix: Contact damage from bosses no longer depends on the boss room
- (Boss) Significantly altered Bubbleman's AI, mostly projectile properties
- (Boss) Nerfed Woodman's Leaf Shield attack damage from 8 to 4
- (Enemy) Added M-445 enemy back in, after fixing its palette change trigger
- (Weapon) Significantly changed Atomic Fire. Lower ammo cost, chance to skip charge levels
- (Colors) Bubble stage has more variety, took out the disco
- (Music) 14 new songs from romhacks, bringing the track total to 66

v0.3.5-beta
- Implemented Music Importer for stage music. 42 new songs from Rockman 2 romhacks.
- Bug fix: First 3 enemies in Heat stage. Again.

v0.3.4-beta
- Bug fix: Removed ceiling from Woodman's room to prevent stuck Robot Masters

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
