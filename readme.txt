Mega Man 2 Randomizer
by duckfist
version 0.4.1 beta

vvv READ THIS BEFORE PLAYING vvvv
vvv READ THIS BEFORE PLAYING vvvv
vvv READ THIS BEFORE PLAYING vvvv
vvv READ THIS BEFORE PLAYING vvvv
vvv READ THIS BEFORE PLAYING vvvv
vvv READ THIS BEFORE PLAYING vvvv
vvv READ THIS BEFORE PLAYING vvvv
vvv READ THIS BEFORE PLAYING vvvv
vvv READ THIS BEFORE PLAYING vvvv
vvv READ THIS BEFORE PLAYING vvvv
vvv READ THIS BEFORE PLAYING vvvv
vvv READ THIS BEFORE PLAYING vvvv
vvv READ THIS BEFORE PLAYING vvvv
vvv READ THIS BEFORE PLAYING vvvv
vvv READ THIS BEFORE PLAYING vvvv
vvv READ THIS BEFORE PLAYING vvvv

-----------------------------------------------------------------------------------------------

CAUTION: KNOWN MAJOR GLITCHES

--- Softlock/zip out of Bubbleman's Boss room ---
If there significant lag and you are facing left as you defeat the boss in Bubbleman's room,
the game can suddenly consider the water to be solid and zip you out of the room, depriving you
of credit for beating the stage, and respawning you in a glitched version of Wily 4. You can
revert this by Game Overing and then choosing Stage Select. To avoid this, take extra care in
Bubbleman's boss room, especially if Bubble himself spawns there, which, ironically, is when it
happens the most. If you are facing Right as you defeat the boss, there's no risk.
tldr; IF BUBBLE MAN APPEARS IN HIS OWN BOSS ROOM ON BUBBLE MAN STAGE, AS YOU DELIVER THE FINAL
HIT TO DEFEAT HIM, MAKE SURE YOU ARE FACING _RIGHT_.

--- Bird despawns Mega Man ---
In some very rare circumstances, a Pipi can sometimes despawn Mega Man himself when it leaves
the screen. This causes Mega Man to suddenly appear inside the ceiling or floor, unable to
move. If you see this occur and you have video footage of it, please let me know.

All known bugs are tracked here:
https://github.com/duckfist/MM2Random/issues?q=is%3Aopen+is%3Aissue+label%3Abug
  
  
BEFORE YOU PLAY MEGA MAN 2 RANDOMZIER

Basic knowledge of Mega Man 2 game mechanics is highly recommended before playing. A few other
intermediate tricks can significantly help with some painfully synergistic enemy combinations.
Here are some pointers you should keep in mind:

- Atomic Fire: Remember that the uncharged shot and the half-charge shot both use the Mega
Buster damage table for damage calculation against enemies and bosses. Only the full charge
will use the Atomic Fire damage table. When checking a boss for an Atomic Fire weakness, use a
full charge, and don't bother with checking an uncharged or half-charge shot

- "Pausefalling": If you pause the game while falling, and then unpause, Mega Man's fall speed
is reset to 0. By repeatedly pausing during the descent of a jump, you can travel a greater
horizontal distance before landing. In Randomizer, some enemy placements can make for some 
difficult jumps if you don't have Item 1 or 2 - take advantage of pausefalling to make these
situations easier.

- If you like to do zip glitches, be careful around the Matasaburo enemy (fan guy). You can get
stuck in the wall while one is on screen. If you can't kill them, use Time-Stopper to resume 
zipping.

- Leaf Shield and Atomic Fire tend to be very effective against enemies - don't forget to try
them out, along with other weapons, if enemies get too tough.

- On Easy Mode, every weapon simply does double damage to everything. That is the only
difference. (Same goes for the original Mega Man 2's Normal Mode).


SOME ENEMY RANDOMIZER NOTES

- The layouts of a few stages have been edited to allow for more enemy variety, particularly
the first room of Airman stage.
- Yoku blocks in Heatman stage and Drop Platforms in Bubbleman stage still appear, slightly
reducing enemy variety in those rooms.
- Frienders (wolves) still spawn like normal, due to dependence on solid blocks in their rooms.
- Big Fish (wily 3 fish) do not spawn anywhere yet, TODO.
- Anko (angler in bubble stage) do not spawn anymore.
- Acid still appears like normal in Wily 6. Although, I could easily replace them with tons of 
enemies. Do you really want that?


OTHER MODIFICATIONS

- All Wily bosses have a small chance to take damage from the Mega Buster; don't forget to try
the Buster on these bosses to save some weapon energy!
- No Wily boss will be weak to Time Stopper, so don't bother trying it.
- Additional large weapon capsules have been placed in Wily 5 and Wily 6.
- The ceilings have been removed from a few Robot Master boss rooms




-----------------------------------------------------------------------------------------------

Featured Randomizer Modules

- Robot Master Portraits: The boss portraits on the Stage Select point to different stages.

- Robot Master Spawn: Boss rooms may contain a different boss.

- Weapon Behavior: Movement properties and ammo usage of each weapon are changed.

- Enemy Weaknesses: Damage done to each enemy by each weapon is changed.

- Boss Weaknesses: The damage done by each weapon against each boss is changed.
  > Four Robot Masters take 2x damage from Buster.
  > Every Robot Master will have a primary weakness and secondary weakness. 
  > Damage values are, on average, lower than in vanilla Mega Man 2.
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

- BGM: The background music for all stages are changed, and include several tracks from Mega
Man 2 romhacks and other Capcom games.

- Random Text:
  > The story in the intro cutscene will be different.
  > In the Weapon Get screen, the weapon name and letter will be randomized.

- And a few surprises

Other Features

- Fast Text: To compensate for the U version being slower, text delay is increased from 7
frames to 4 frames.

- Burst Chaser Mode: Increase Mega Man's movement speed, and reduce a few other delay timers.

- The layouts of a few stages have been modified to accomodate the randomizer, such as some
tile edits in Airman stage, tiles in a few boss rooms, and some large weapon energy capsules.

  
-----------------------------------------------------------------------------------------------
  
Development Tools

- FCEUX 2.2.3 http://www.fceux.com/web/home.html
- visine 2.8.2 by -=Fx3=- http://www.romhacking.net/utilities/172/
- Rockman 2 Editor 1.0 by Rock5easily http://www.romhacking.net/utilities/836/
- Tile Molestor Mod 0.19 http://www.romhacking.net/utilities/991/
- Visual Studio Community 2017

Special thanks to Binarynova, RaneofSoTN, and Coltaho for their code contributions . 

-----------------------------------------------------------------------------------------------
  
Changelog

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