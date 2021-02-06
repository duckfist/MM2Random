using System;
using System.Collections.Generic;
using System.Linq;

namespace MM2Randomizer.Randomizers
{
    public class WeaponName
    {
        //
        // Static Constructors
        //

        static WeaponName()
        {
            // Initialize the weapon first name set
            WeaponName.mFirstNameLookup = new Dictionary<Char, HashSet<String>>();

            foreach (String name in WeaponName.WEAPON_FIRST_NAME_LIST)
            {
                if (false == String.IsNullOrEmpty(name) &&
                    name.Length < WeaponName.WEAPON_FIRST_NAME_MAXLENGTH)
                {
                    String upperName = name.ToUpperInvariant();

                    Char key = upperName[0];

                    if (false == WeaponName.mFirstNameLookup.ContainsKey(key))
                    {
                        WeaponName.mFirstNameLookup.Add(key, new HashSet<String>());
                    }

                    WeaponName.mFirstNameLookup[key].Add(upperName);
                }
            }

            // Initialize the weapon second name set
            WeaponName.mSecondNameLookup = new HashSet<String>();

            foreach (String name in WeaponName.WEAPON_SECOND_NAME_LIST)
            {
                if (false == String.IsNullOrEmpty(name))
                {
                    String upperName = name.ToUpperInvariant();
                    WeaponName.mSecondNameLookup.Add(upperName);
                }
            }
        }


        //
        // Constructors
        //

        public WeaponName(String in_FirstName, String in_SecondName, Char in_WeaponLetter)
        {
            this.mFirstName = in_FirstName ?? throw new ArgumentNullException(nameof(in_FirstName));
            this.mSecondName = in_SecondName ?? throw new ArgumentNullException(nameof(in_SecondName));
            this.mLetter = in_WeaponLetter;

            // The '@' character is a newline
            if (this.mFirstName.Length + this.mSecondName.Length + 1 > WeaponName.WEAPON_NAME_MAXLENGTH)
            {
                this.mName = $"{in_FirstName} {in_SecondName[0]}.";
            }
            else
            {
                this.mName = $"{in_FirstName} {in_SecondName}";
            }
        }


        //
        // Properties
        //

        public String FirstName
        {
            get
            {
                return this.mFirstName;
            }
        }

        public String SecondName
        {
            get
            {
                return this.mSecondName;
            }
        }

        public String Name
        {
            get
            {
                return this.mName;
            }
        }

        public Char Letter
        {
            get
            {
                return this.mLetter;
            }
        }

        //
        // Public Static Methods
        //

        public static IEnumerable<WeaponName> GenerateUniqueWeaponNames(Random in_Random, Int32 in_Count)
        {
            List<WeaponName> weaponNames = new List<WeaponName>();

            // Randomize a list of the alphabet to create weapons with
            // unique first letters
            List<Char> randomLetters = WeaponName.ALPHABET.OrderBy(x => in_Random.Next()).ToList();

            // Randomize the list of second names
            IEnumerator<String> secondNameEnumerator = WeaponName.mSecondNameLookup.OrderBy(x => in_Random.Next()).GetEnumerator();

            // Loop over the count of weapon names requested
            for (Int32 weaponIndex = 0; weaponIndex < in_Count; ++weaponIndex)
            {
                // Think about re-shuffling the list if the first run through is exhaused
                Char key = randomLetters[weaponIndex % randomLetters.Count];
                HashSet<String> firstNameSet = WeaponName.mFirstNameLookup[key];

                // Choose a random weapon first name from the list of names
                // based on the first character
                String firstName = firstNameSet.ElementAt(in_Random.Next(firstNameSet.Count));

                // Choose a weapon second name
                if (false == secondNameEnumerator.MoveNext())
                {
                    secondNameEnumerator.Reset();
                    secondNameEnumerator.MoveNext();
                }

                String secondName = secondNameEnumerator.Current;

                // Add the new name to the list
                weaponNames.Add(new WeaponName(firstName, secondName, firstName[0]));
            }

            return weaponNames;
        }


        public static Char? GetUnusedWeaponLetter(Random in_Random, IEnumerable<WeaponName> in_WeaponNames)
        {
            HashSet<Char> weaponLetters = new HashSet<Char>(in_WeaponNames.Select(x => x.Letter));
            HashSet<Char> alphabetSet = new HashSet<Char>(WeaponName.ALPHABET.Select(x => x));

            alphabetSet.ExceptWith(weaponLetters);

            if (alphabetSet.Count > 0)
            {
                return alphabetSet.ElementAt(in_Random.Next(alphabetSet.Count));
            }
            else
            {
                return null;
            }
        }


        //
        // Private Data Members
        //

        private String mFirstName;
        private String mSecondName;
        private String mName;
        private Char mLetter;


        //
        // Private Static Members
        //

        private static readonly Dictionary<Char, HashSet<String>> mFirstNameLookup = new Dictionary<Char, HashSet<String>>();
        private static readonly HashSet<String> mSecondNameLookup = new HashSet<String>();

        private const Int32 WEAPON_FIRST_NAME_MAXLENGTH = 9;
        private const Int32 WEAPON_NAME_MAXLENGTH = 12;
        public const String ALPHABET = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        private static readonly String[] WEAPON_FIRST_NAME_LIST = new String[]
        {
            //
            // Mega Man
            //
            "Proto",        // DLN.000 Proto Man
            "Mega",         // DLN.001 Mega Man
            "Roll",         // DLN.002 Roll

            //
            // Mega Man 1
            //
            "Rolling",      // DLN.003 Cut Man
            "Super",        // DLN.004 Guts Man
            "Ice",          // DLN.005 Ice Man
            "Hyper",        // DLN.006 Bomb Man
            "Fire",         // DLN.007 Fire Man
            "Thunder",      // DLN.008 Elec Man

            //
            // Mega Man 2
            //
            "Metal",        // DWN.009 Metal Man
            "Air",          // DWN.010 Air Man
            "Bubble",       // DWN.011 Bubble Man
            "Quick",        // DWN.012 Quick Man
            "Crash",        // DWN.013 Crash Man
            "Time",         // DWN.014 Flash Man
            "Atomic",       // DWN.015 Heat Man
            "Leaf",         // DWN.016 Wood Man

            //
            // Mega Man 3
            //
            "Needle",       // DWN.017 Needle Man
            "Magnet",       // DWN.018 Magnet Man
            "Gemini",       // DWN.019 Gemini Man
            "Hard",         // DWN.020 Hard Man
            "Top",          // DWN.021 Top Man
            "Search",       // DWN.022 Snake Man
            "Spark",        // DWN.023 Spark Man
            "Shadow",       // DWN.024 Shadow Man

            //
            // Mega Man 4
            //
            "Flash",        // DWN.025 Bright Man
            "Rain",         // DWN.026 Toad Man
            "Drill",        // DWN.027 Drill Man
            "Pharaoh",      // DWN.028 Pharaoh Man
            "Ring",         // DWN.029 Ring Man
            "Dust",         // DWN.030 Dust Man
            "Dive",         // DWN.031 Dive Man
            "Skull",        // DWN.032 Skull Man

            //
            // Mega Man 5
            //
            "Gravity",      // DWN.033 Gravity Man
            "Water",        // DWN.034 Wave Man
            "Power",        // DWN.035 Stone Man
            "Gyro",         // DWN.036 Gyro Man
            "Star",         // DWN.037 Star Man
            "Charge",       // DWN.038 Charge Man
            "Napalm",       // DWN.039 Napalm Man
            "Crystal",      // DWN.040 Crystal Man

            //
            // Mega Man 6
            //
            "Blizzard",     // DWN.041 Blizzard Man
            "Centaur",      // DWN.042 Centaur Man
            "Flame",        // DWN.043 Flame Man
            "Knight",       // DWN.044 Knight Man
            "Plant",        // DWN.045 Plant Man
            "Silver",       // DWN.046 Tomahawk Man
            "Wind",         // DWN.047 Wind Man
            "Yamato",       // DWN.048 Yamato Man

            //
            // Mega Man 7
            //
            "Freeze",       // DWN.049 Freeze Man
            "Junk",         // DWN.050 Junk Man
            "Danger",       // DWN.051 Burst Man
            "Thunder",      // DWN.052 Cloud Man
            "Wild",         // DWN.053 Spring Man
            "Slash",        // DWN.054 Slash Man
            "Noise",        // DWN.055 Shade Man
            "Scorch",       // DWN.056 Turbo Man

            //
            // Mega Man 8
            //
            "Tornado",      // DWN.057 Tengu Man
            "Astro",        // DWN.058 Astro Man
            "Copy",         // DWN.058 Astro Man
            "Flame",        // DWN.059 Sword Man
            "Thunder",      // DWN.060 Clown Man
            "Homing",       // DWN.061 Search Man
            "Ice",          // DWN.062 Frost Man
            "Flash",        // DWN.063 Grenade Man
            "Water",        // DWN.064 Aqua Man

            //
            // Mega Man 9
            //
            "Concrete",     // DLN.065 Concrete Man
            "Tornado",      // DLN.066 Tornado Man
            "Laser",        // DLN.067 Splash Woman
            "Plug",         // DLN.068 Plug Man
            "Jewel",        // DLN.069 Jewel Man
            "Hornet",       // DLN.070 Hornet Man
            "Magma",        // DLN.071 Magma Man
            "Black Hole",   // DLN.072 Galaxy Man

            //
            // Mega Man 10
            //
            "Triple",       // DWN.073 Blade Man
            "Water",        // DWN.074 Pump Man
            "Commando",     // DWN.075 Commando Man
            "Chill",        // DWN.076 Chill Man
            "Thunder",      // DWN.077 Sheep Man
            "Rebound",      // DWN.078 Strike Man
            "Wheel",        // DWN.079 Nitro Man
            "Solar",        // DWN.080 Solar Man

            //
            // Mega Man 11
            //
            "Block",        // DWN.081 Block Man
            "Scramble",     // DWN.082 Fuse Man
            "Chain",        // DWN.083 Blast Man
            "Acid",         // DWN.084 Acid Man
            "Tundra",       // DWN.085 Tundra Man
            "Blazing",      // DWN.086 Torch Man
            "Pile",         // DWN.087 Impact Man
            "Bounce",       // DWN.088 Bounce Man

            //
            // Mega Man Killer
            //
            "Mirror",       // MKN.001 Enker
            "Screw",        // MKN.002 Punk
            "Ballade",      // MKN.003 Ballade

            //
            // Quint
            //
            "Sakugarne",    // ???.??? Quint

            //
            // Mega Man V
            //
            "Spark",        // SRN.001 Terra
            "Grab",         // SRN.002 Mercury
            "Bubble",       // SRN.003 Venus
            "Photon",       // SRN.004 Mars
            "Electric",     // SRN.005 Jupiter
            "Black Hole",   // SRN.006 Saturn
            "Deep",         // SRN.007 Uranus
            "Break",        // SRN.008 Pluto
            "Salt",         // SRN.009 Neptune

            //
            // Mega Man & Bass
            //
            "Lightning",    // KGN.001 Dynamo Man
            "Ice",          // KGN.002 Cold Man
            "Spread",       // KGN.003 Ground Man
            "Remote",       // KGN.004 Pirate Man
            "Wave",         // KGN.005 Burner Man
            "Magic",        // KGN.006 Magic Man
            "Bass",         // SWN.001 Bass

            //
            // Mega Man DOS
            //
            "Sonic",        // ???.??? Sonic Man
            "Force",        // ???.??? Volt Man
            "Nuclear",      // ???.??? Dyna Man

            //
            // Mega Man 3 DOS
            //
            "Bit",          // ???.??? Bit Man
            "Blade",        // ???.??? Blade Man
            "Oil",          // ???.??? Oil Man
            "Shark",        // ???.??? Shark Man
            "Water",        // ???.??? Wave Man
            "Torch",        // ???.??? Torch Man

            //
            // Speacial Weapons
            //
            "Auto",
            "Rush",
            "Beat",
            "Wire",
            "Super",

            //
            // Mega Man X
            //
            "Zero",         // Zero
            "X",            // Mega Man X
            "Shotgun",      // Chill Penguin
            "Electric",     // Spark Mandrill
            "Rolling",      // Armored Armadillo
            "Homing",       // Launch Octopus
            "Boomerang",    // Boomer Kuwanger
            "Chameleon",    // Sting Chameleon
            "Storm",        // Storm Eagle
            "Fire",         // Flame Mammoth

            //
            // Mega Man X2
            //
            "Sonic",        // Overdrive Ostrich
            "Strike",       // Wire Sponge
            "Spin",         // Wheel Gator
            "Bubble",       // Bubble Crab
            "Speed",        // Flame Stag
            "Silk",         // Morph Moth
            "Magnet",       // Magna Centipede
            "Crystal",      // Crystal Snail

            //
            // Mega Man X3
            //
            "Acid",         // Toxic Seahorse
            "Tornado",      // Tunnel Rhino
            "Triad",        // Volt Catfish
            "Spinning",     // Crush Crawfish
            "Ray",          // Neon Tiger
            "Gravity",      // Gravity Beetle
            "Parasitic",    // Blast Hornet
            "Frost",        // Blizzard Buffalo

            //
            // Mega Man Boss Names
            //
            "Guts",
            "Elec",
            "Cut",
            "Toad",
            "Wily",
            "Cossack",
            "Quint",
            "Clown",

            //
            // Mega Man X Boss Names
            //
            "Volt",

            //
            // Other
            //

            "Big",
            "Ion",
            "Key",
            "Kick",
            "Mash",
            "Octopus",
            "Ocean",
            "Ooze",
            "Orb",
            "Quad",
            "Quiet",
            "Rta",
            "Tas",
            "Turbo",
            "Ultimate",
            "Umber",
            "Undo",
            "Urn",
            "Vine",
            "Virus",
            "Void",
            "XRay",
            "Yammar",
            "Yoga",
            "Zap",
            "Zeta",
            "Zoom",
        };

        private static readonly String[] WEAPON_SECOND_NAME_LIST = new String[]
        {
            //
            // Mega Man
            //
            "Shield",       // DLN.000 Proto Man
            "Buster",       // DLN.001 Mega Man

            //
            // Mega Man 1
            //
            "Cutter",       // DLN.003 Cut Man
            "Arm",          // DLN.004 Guts Man
            "Slasher",      // DLN.005 Ice Man
            "Bomb",         // DLN.006 Bomb Man
            "Storm",        // DLN.007 Fire Man
            "Beam",         // DLN.008 Elec Man

            //
            // Mega Man 2
            //
            "Blade",        // DWN.009 Metal Man
            "Shooter",      // DWN.010 Air Man
            "Lead",         // DWN.011 Bubble Man
            "Boomerang",    // DWN.012 Quick Man
            "Bomber",       // DWN.013 Crash Man
            "Stopper",      // DWN.014 Flash Man
            "Fire",         // DWN.015 Heat Man
            "Shield",       // DWN.016 Wood Man

            //
            // Mega Man 3
            //
            "Cannon",       // DWN.017 Needle Man
            "Missile",      // DWN.018 Magnet Man
            "Laser",        // DWN.019 Gemini Man
            "Knuckle",      // DWN.020 Hard Man
            "Spin",         // DWN.021 Top Man
            "Snake",        // DWN.022 Snake Man
            "Shock",        // DWN.023 Spark Man
            "Blade",        // DWN.024 Shadow Man

            //
            // Mega Man 4
            //
            "Stopper",      // DWN.025 Bright Man
            "Flush",        // DWN.026 Toad Man
            "Bomb",         // DWN.027 Drill Man
            "Shot",         // DWN.028 Pharaoh Man
            "Boomerang",    // DWN.029 Ring Man
            "Crusher",      // DWN.030 Dust Man
            "Missile",      // DWN.031 Dive Man
            "Barrier",      // DWN.032 Skull Man

            //
            // Mega Man 5
            //
            "Hold",         // DWN.033 Gravity Man
            "Wave",         // DWN.034 Wave Man
            "Stone",        // DWN.035 Stone Man
            "Attack",       // DWN.036 Gyro Man
            "Crash",        // DWN.037 Star Man
            "Kick",         // DWN.038 Charge Man
            "Bomb",         // DWN.039 Napalm Man
            "Eye",          // DWN.040 Crystal Man

            //
            // Mega Man 6
            //
            "Attack",       // DWN.041 Blizzard Man
            "Flash",        // DWN.042 Centaur Man
            "Blast",        // DWN.043 Flame Man
            "Crusher",      // DWN.044 Knight Man
            "Barrier",      // DWN.045 Plant Man
            "Tomahawk",     // DWN.046 Tomahawk Man
            "Storm",        // DWN.047 Wind Man
            "Spear",        // DWN.048 Yamato Man

            //
            // Mega Man 7
            //
            "Cracker",      // DWN.049 Freeze Man
            "Shield",       // DWN.050 Junk Man
            "Wrap",         // DWN.051 Burst Man
            "Bolt",         // DWN.052 Cloud Man
            "Coil",         // DWN.053 Spring Man
            "Claw",         // DWN.054 Slash Man
            "Crush",        // DWN.055 Shade Man
            "Wheel",        // DWN.056 Turbo Man

            //
            // Mega Man 8
            //
            "Hold",         // DWN.057 Tengu Man
            "Crush",        // DWN.058 Astro Man
            "Vision",       // DWN.058 Astro Man
            "Sword",        // DWN.059 Sword Man
            "Claw",         // DWN.060 Clown Man
            "Sniper",       // DWN.061 Search Man
            "Wave",         // DWN.062 Frost Man
            "Bomb",         // DWN.063 Grenade Man
            "Balloon",      // DWN.064 Aqua Man

            //
            // Mega Man 9
            //
            "Shot",         // DLN.065 Concrete Man
            "Blow",         // DLN.066 Tornado Man
            "Trident",      // DLN.067 Splash Woman
            "Ball",         // DLN.068 Plug Man
            "Satellite",    // DLN.069 Jewel Man
            "Chaser",       // DLN.070 Hornet Man
            "Bazooka",      // DLN.071 Magma Man
            "Bomb",         // DLN.072 Galaxy Man

            //
            // Mega Man 10
            //
            "Blade",        // DWN.073 Blade Man
            "Shield",       // DWN.074 Pump Man
            "Bomb",         // DWN.075 Commando Man
            "Spike",        // DWN.076 Chill Man
            "Wool",         // DWN.077 Sheep Man
            "Striker",      // DWN.078 Strike Man
            "Cutter",       // DWN.079 Nitro Man
            "Blaze",        // DWN.080 Solar Man

            //
            // Mega Man 11
            //
            "Dropper",      // DWN.081 Block Man
            "Thunder",      // DWN.082 Fuse Man
            "Blast",        // DWN.083 Blast Man
            "Barrier",      // DWN.084 Acid Man
            "Storm",        // DWN.085 Tundra Man
            "Torch",        // DWN.086 Torch Man
            "Driver",       // DWN.087 Impact Man
            "Ball",         // DWN.088 Bounce Man

            //
            // Mega Man Killer
            //
            "Buster",       // MKN.001 Enker
            "Crusher",      // MKN.002 Punk
            "Cracker",      // MKN.003 Ballade

            //
            // Mega Man V
            //
            "Chaser",       // SRN.001 Terra
            "Buster",       // SRN.002 Mercury
            "Bomb",         // SRN.003 Venus
            "Missile",      // SRN.004 Mars
            "Shock",        // SRN.005 Jupiter
            "Hole",         // SRN.006 Saturn
            "Digger",       // SRN.007 Uranus
            "Dash",         // SRN.008 Pluto
            "Water",        // SRN.009 Neptune

            //
            // Mega Man & Bass
            //
            "Bolt",         // KGN.001 Dynamo Man
            "Wall",         // KGN.002 Cold Man
            "Drill",        // KGN.003 Ground Man
            "Mine",         // KGN.004 Pirate Man
            "Burner",       // KGN.005 Burner Man
            "Card",         // KGN.006 Magic Man
            "Buster",       // SWN.001 Bass

            //
            // Mega Man DOS
            //
            "Wave",         // ???.??? Sonic Man
            "Field",        // ???.??? Volt Man
            "Detonator",    // ???.??? Dyna Man

            //
            // Mega Man 3 DOS
            //
            "Cannon",       // ???.??? Bit Man
            "Launcher",     // ???.??? Blade Man
            "Stream",       // ???.??? Oil Man
            "Boomerang",    // ???.??? Shark Man
            "Shooter",      // ???.??? Wave Man
            "Arm",          // ???.??? Torch Man

            //
            // Speacial Weapons
            //
            "Adapter",
            "Coil",
            "Marine",
            "Jet",
            "Arrow",

            //
            // Mega Man X
            //
            "Ice",          // Chill Penguin
            "Spark",        // Spark Mandrill
            "Shield",       // Armored Armadillo
            "Torpedo",      // Launch Octopus
            "Cutter",       // Boomer Kuwanger
            "Sting",        // Sting Chameleon
            "Tornado",      // Storm Eagle
            "Wave",         // Flame Mammoth

            //
            // Mega Man X2
            //
            "Slicer",       // Overdrive Ostrich
            "Chain",        // Wire Sponge
            "Wheel",        // Wheel Gator
            "Splash",       // Bubble Crab
            "Burner",       // Flame Stag
            "Shot",         // Morph Moth
            "Mine",         // Magna Centipede
            "Hunter",       // Crystal Snail

            //
            // Mega Man X3
            //
            "Burst",        // Toxic Seahorse
            "Fang",         // Tunnel Rhino
            "Thunder",      // Volt Catfish
            "Blade",        // Crush Crawfish
            "Splasher",     // Neon Tiger
            "Well",         // Gravity Beetle
            "Bomb",         // Blast Hornet
            "Shield",       // Blizzard Buffalo

            //
            // Other
            //

            "Arc",
            "Axe",
            "Blaster",
            "Box",
            "Device",
            "Gun",
            "Hit",
            "Jab",
            "Kick",
            "Zip",
        };
    }
}
