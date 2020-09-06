Mega Man 2 Randomizer
by duckfist
version 0.5.0 beta

vvv READ THIS BEFORE PLAYING vvvv



<TODO INSTALL INSTRUCTIONS, extract folder from zip file, ensure dll is in same dir>

-----------------------------------------------------------------------------------------------

PREREQUISITES

The Mega Man 2 Randomizer desktop application (MM2RandoHost.exe) requires the use of Windows 7 
or newer, with the .NET Framework 4.7.2 runtime installed. You can find the link to install
the .NET Framework 4.7.2 from Microsoft here:
https://support.microsoft.com/en-us/help/4054530/microsoft-net-framework-4-7-2-offline-installer-for-windows


-----------------------------------------------------------------------------------------------

CAUTION: KNOWN MAJOR GLITCHES

--- Passwords do not work ---
The Password function is broken and cannot be relied upon. This happened due to one of the
many reshufflings that this program performs, and will be fixed at a later date. Please use
savestates or be very careful not to cause a softlock or crash.

--- Touching the boss door in a boss room sometimes glithces the game ---
This can occur in Mega Man 2 in any boss room under the right circumstances, more often when
the game is lagging due to many sprites. It has been observed in the randomizer as well,
such as in the Wily 4 boss room. Avoid touching the left sides of any boss room.

--- Bird despawns Mega Man ---
In some very rare circumstances, a Pipi can sometimes despawn Mega Man himself when it leaves
the screen. This causes Mega Man to suddenly appear inside the ceiling or floor, unable to
move. If you see this occur and you have video footage of it, please let me know.

All known bugs are tracked here:
https://github.com/duckfist/MM2Random/issues?q=is%3Aopen+is%3Aissue+label%3Abug
  

  
-----------------------------------------------------------------------------------------------  
  
NEW PLAYERS: BEFORE YOU PLAY MEGA MAN 2 RANDOMZIER

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

- Leaf Shield, Atomic Fire, and Clash Bomber tend to be very effective against enemies - don't
forget to try them out, along with other weapons, if enemies get too tough.

- On Easy Mode, every weapon simply does double damage to everything. Also, the drop rate for
Large Weapon Capsules, Large Energy Capsules, and Extra Lives is increased. These are the only
differences. (Same goes for the original Mega Man 2's Normal Mode).



-----------------------------------------------------------------------------------------------  

OTHER MODIFICATIONS TO THE BASE GAME

- When you Game Over, your E-Tanks are not lost
- You can no longer accidentally use an E-Tank when already at full HP
- All Wily bosses now only have a 25% chance each to take any damage from the Mega Buster,
  except Wily Machine, which has a 75% chance. Don't forget to try the Buster on these bosses 
  to save some weapon energy!
- No Wily boss will be weak to Time Stopper, so don't bother trying it. One of the Robot
  Masters will, though, so try it in the Wily 5 refights.
- Additional large weapon capsules have been placed in Wily 5 and Wily 6.
- The ceilings have been removed from a few Robot Master boss rooms
- Woodman's leaf shield now only does 4 damage to you instead of 8



-----------------------------------------------------------------------------------------------

RANDOMIZER FLAGS OVERVIEW

Gameplay Settings

- Randomizer Core: Default ON. Cannot be disabled.
  This primary component shuffles the weapons you obtain from Robot Masters. It also changes
  the portrait names on the stage select screen, and the teleporter destinations in Wily 5,
  which are all related. Other small modifications are performed. All other randomizer modules
  depend on this setting. Note that the portrait position on the stage select screen will still
  correspond to the actual stage you play (not the Robot Master that appears there, or the
  weapon that is rewarded; enable "Hide Stage Names" to make this a surprise). 

- Weapon Behavior: Default ON.
  Movement properties, sounds, and ammo usage of each weapon are changed.
  
  > Mega Buster is unchanged
  
  > Atomic Fire may only have 2 charge levels, or may instantly charge. It is generally quite
    a bit more effective than it was in the original Mega Man 2. Its charge speeds and X-
    velocity is also randomized. Atomic Fire tends to have lower ammo consumption than vanilla.
    20% chance to have old charge Behavior
    40% chance to skip the level 1 charge
    40% chance to skip all charge levels and instantly shoot at a full charge
  
  > Air Shooter may shoot between 1 and 3 projectiles, each of which having random X and Y-
    accelerations and initial X and Y-velocities.
    60% chance to have 3 projectiles
    20% chance to have 2 projectiles
    20% chance to have 1 projectile
    
  > Leaf Shield is generally very effective against normal enemies. Its deployment time, launch
    directions, and movement speeds are randomized.
    - Random deployment time, from 0 to 34 frames (vanilla is 12)
    - 50% chance to invert the X launch direction
    - Another 50% chance to invert the Y launch direction
    - X and Y-velocity on launch can range from 2 to 8 pixels per frame (vanilla is 4)
    
  > Bubble Lead has random number of maximum projectiles, inital X and Y velocities, random
    surface-crawling speed, and random X and Y falling speeds (after traversing a surface).
    - There may be between 1 and 4 maximum projectiles (vanilla is 3), with an even probability
    - Initial X-velocity is between 1 and 3 pixels per frame (vanilla is 1)
    - Initial Y-velocity is between 0 and 6 pixels per frame (upwards; vanilla is 2)
    - Surface X-velocity is between 1 and 5 pixels per frame (vanilla is 2)
    - Falling X-velocity is between 0 and 5 pixels per frame, with a 50% chance to be 0 and the
      rest with an even probability (vanilla is 0)
    - Falling Y-velocity is between -6 and 6 (vanilla is -2)
    
  > Quick Boomerang has several random parameters that affect its trajectory when fired, and is
    generally very effective for its low ammo consumption.
    - Autofire cooldown is between 5 and 18 frames (vanilla 11)
    - Maximum projectiles on screen is between 2 and 6 shots (vanilla is 5)
  
  > Time Stopper has a 75% chance of being modified to be reusable, like the Rockman 4 version.
    How much ammo it consumes and its duration of activation is also randomized.
    
  > Metal Blade has a random number of maximum projectiles, randomized component velocities,
    and tends to have a higher ammo consumption than vanilla 
    - Maximum number of projectiles is between 1 and 4 (vanilla is 3)
    
  > Clash Bomber has a random velocity, and tends to have lower ammo consumption than vanilla.
    - Ammo consumption is between 1 and 3 ticks (vanilla is 4)
    - 25% chance to have its explosion drift upwards slightly
    - Initial X-velocity is between 2 and 7 (vanilla is 4)
    - Delay before exploding is between 1 and 192 frames (vanilla is 126)
    - 50% chance to change the explosion behavior to a "single explosion"
    
  > Item 1 has a randomized delay before it begins flashing and the time it takes to disappear,
    as well as its Y-velocity.
    - 25% chance to have a Y-velocity of 0
    
- Boss Weaknesses: Default ON.
  The damage done by each weapon against each boss is changed.
  > Four Robot Masters take 2x damage from Buster.
  > Every Robot Master will have a primary weakness and secondary weakness.
  > In addition to primary and secondary, one robot master will have one "great" weakness for
    increased damage, and another will have one "ultimate" weakness for even more damage.
  > Damage values are, on average, lower than in vanilla Mega Man 2.
  > Wily bosses are also affected, including Boobeam Trap and its Barriers.
  > There is a 25% chance for each Wily boss to take damage from buster.
  > Time Stopper will never do damage to Wily bosses.
  > With Atomic Fire, only a full charge will deal weakness damage.
  > Damage is scaled based on Ammo consumption

- Boss Room: Default ON.
  Robot Master rooms may contain a different Robot Master.
       
- Boss AI: Default ON.
  Properties of Robot Master attack patterns are changed.
  TODO: Details!

- Items 1, 2, and 3 Get: Default ON.
  Which Robot Master awards each of the 3 Items is shuffled.

- Enemy Locations: Default ON.
  The enemy IDs for most enemy instances have been changed. 
  > Sprite banks for each room are modified to support different enemy combinations appearing.
  > A few enemies are not yet supported.
  > The layouts of a few stages have been edited to allow for more enemy variety, particularly
    the first room of Airman stage.
  > Yoku blocks in Heatman stage and Drop Platforms in Bubbleman stage still appear, slightly
    reducing enemy variety in those rooms.
  > Frienders (wolves) still spawn like normal, due to dependence on solid blocks in their rooms.
  > Big Fish (wily 3 fish) do not spawn anywhere yet, TODO.
  > Anko (angler in bubble stage) do not spawn anymore.
  > Acid still appears like normal in Wily 6. Although, I could easily replace them with tons of 
    enemies. Do you really want that?
  
- Enemy Weaknesses: Default ON.
  The damage done by each weapon against each normal enemy is changed.
  > This includes the Clash Barrier and Press objects (although only exactly 1 weapon will be
    able to damage Clash Barriers)
  > Note that all enemies in the game have 20 HP, weapons simply do different damage to each
  > Buster will always do 1 damage to Friender (Woodman miniboss) 
  > The original damage tables have been slightly modified before the shuffling occurs, so that
    a more balanced and interesting range of damage is applied
  > Atomic Fire, Leaf Shield, and Clash Bomber are generally more effective than in the original
    Mega Man 2, due to the nature of shuffling the existing damage values
  > Time Stopper will never do damage to normal enemies

- Stage Layouts: Default ON.
  Randomize tile structures and the design of individual stages.
  > Currently only the false floors in Wily 4 are affected. More to come in the future.

- Faster Text: Default ON.
  Several cutscene timers have been reduced to provide less downtime.
  > Weapon Get cutscene text is much faster (7 frame-per-character delay to 4 frames)
  > Weapon Get cutscenes skip several of the extra pages when getting an item
  > On stage start, the "READY" message and time to teleport in has been reduced
  > The Wily Map cutscenes advance faster
  > When defeating a boss, you immediately teleport out of the room. 
    NOTE: This also keeps a pretty nasty glitch from happening. Leave this setting on!
      
- Burst Chaser Mode: Default OFF.
  Greatly increase movement speed.
  > This will be replaced in the future with a more comprehensive set of optional parameters.
  <TODO details>

- Hide Stage Names: Default OFF.
  Disable the portrait names on the Stage Select screen. Since the names normally correspond to
  the stage that you play, enable this setting to have each stage be a surprise. Note that this
  removes the early game strategy of going to easy stages, impacting races.


Cosmetic Options

- Random Colors: Default ON.
  Stage backgrounds, the Start/Password screen, Stage Select screen, Stage Intro screen, Weapon
  Get screen, Wily Map screen, Robot Master colors, Mega Man's weapon colors, and the Wily bosses
  have randomly assigned colors.

- Random New Music: Default ON.
  The background music for all stages are changed, and include several tracks from Mega Man 2
  romhacks and other Capcom games.
  > There are 11 total shuffled songs: 8 for the Robot Master Stages, and 3 for the Wily stages.
  > Wily 1 and 2 will play the same track, Wily 3 and 4 will play the same track, and Wily 5 has
    been modified to play a unique 11th track.
  > Wily 6 and the second half of the credits will play a random stage track as well
  > There are currently 102 different songs that can be played

- Random Text Content: Default ON.
  > The story in the intro cutscene will be different.
  > In the Weapon Get screen, the weapon name and letter will be randomized.

- New Player Sprite: Default Rockman.
  Choose to replace the Rockman sprite with Roll, Bass, or Protoman!

  
Custom Variables

- <TODO>


  
-----------------------------------------------------------------------------------------------
  
DEVELOPMENT TOOLS AND CREDITS

- FCEUX 2.2.3 http://www.fceux.com/web/home.html
- visine 2.8.2 by -=Fx3=- http://www.romhacking.net/utilities/172/
- Rockman 2 Editor 1.0 by Rock5easily http://www.romhacking.net/utilities/836/
- Tile Molestor Mod 0.19 http://www.romhacking.net/utilities/991/
- Visual Studio Community 2017

Protoman sprite ripped from "Mega Man II - Protoman mode" by Riffman81.
https://www.romhacking.net/hacks/861/

Roll sprite ripped from "Roll-chan 2" by Zynk Oxhyde.
https://www.romhacking.net/hacks/2265/

Bass sprite ripped from "The Adventure of Bass II" by Sivak
https://www.romhacking.net/hacks/9/

Thanks to every Mega Man 1 and Mega Man 2 romhack author/composer that I ripped music from.

Special thanks to Binarynova, RaneofSoTN, Coltaho, ramon-rocha, NARFNra, and BrooklynS for
their code contributions. 

Mega Man 2 Randomizer lead developer: duckfist
twitch.tv/duckfist
twitter.com/regularduck
