using System;
using System.Collections.Generic;

using MM2Randomizer.Enums;
using MM2Randomizer.Patcher;

namespace MM2Randomizer.Randomizers
{
    public class RWeaponNames : IRandomizer
    {
        public static readonly int MAX_CHARS = 12;
        public static readonly int offsetLetters = 0x037e22;
        public static readonly int offsetAtomicFire = 0x037e2e;

        public RWeaponNames() { }

        public void Randomize(Patch p, Random r)
        {
            // Write in new weapon names
            for (int i = 0; i < 8; i++)
            {
                int offset = offsetAtomicFire + i * 0x10;

                string name = GetRandomName(r);
                char[] chars = name.ToCharArray();

                for (int j = 0; j < MAX_CHARS; j++)
                {
                    if (j < chars.Length)
                    {
                        byte b = Convert.ToByte(chars[j]);
                        p.Add(offset + j, b, String.Format("Weapon Name {0} Char #{1}: {2}", ((EDmgVsBoss.Offset)i).Name, j, chars[j].ToString()));

                    }
                    else
                    {
                        p.Add(offset + j, Convert.ToByte('@'), String.Format("Weapon Name {0} Char #{1}: @", ((EDmgVsBoss.Offset)i).Name, j));
                    }
                }
            }

            // Erase "Boomerang" for now
            for (int i = 0; i < 10; i++)
            {
                p.Add(0x037f5e + i, Convert.ToByte('@'), String.Format("Quick Boomerang Name Erase Char #{0}: @", i));
            }

            // Write in new weapon letters
            for (int i = 0; i < 8; i++)
            {
                int randLetter = 0x41 + r.Next(26);
                p.Add(offsetLetters + i, (byte)randLetter, String.Format("Weapon Get {0} Letter: {1}", ((EDmgVsBoss.Offset)i).Name, Convert.ToChar(randLetter).ToString()));
            }
        }

        private string GetRandomName(Random r)
        {
            string finalName = "";

            // Start with random list
            int l = r.Next(1);
            string name0, name1;
            if (l == 0)
            {
                // Get random name from first list
                int random = r.Next(Names0.Length);
                name0 = Names0[random];

                // From second list, get subset of names with valid character count
                int charsLeft = MAX_CHARS - name0.Length - 1; // 1 space
                List<string> names1Left = new List<string>();
                for (int j = 0; j < Names1.Length; j++)
                {
                    if (Names1[j].Length <= charsLeft)
                    {
                        names1Left.Add(Names1[j]);
                    }
                }

                // Get random name from modified second list
                if (names1Left.Count > 0)
                {
                    random = r.Next(names1Left.Count);
                    name1 = names1Left[random];
                }
                else
                {
                    name1 = "";
                }
            }
            else
            {
                // Get random name from second list
                int random = r.Next(Names1.Length);
                name1 = Names1[random];

                // From first list, get subset of names with valid character count
                int charsLeft = MAX_CHARS - name1.Length - 1; // 1 space
                List<string> names0Left = new List<string>();
                for (int j = 0; j < Names0.Length; j++)
                {
                    if (Names0[j].Length <= charsLeft)
                    {
                        names0Left.Add(Names0[j]);
                    }
                }

                // Get random name from modified first list
                if (names0Left.Count > 0)
                {
                    random = r.Next(names0Left.Count);
                    name0 = names0Left[random];
                }
                else
                {
                    name0 = "";
                }
            }

            // Handle cases for only one name
            if (name0.Length == 0)
            {
                finalName = name1;
            }
            else if (name1.Length == 0)
            {
                finalName = name0;
            }
            // Concatenate final name
            else
            {
                finalName = name0 + "@" + name1;
            }

            return finalName;
        }

        static string[] Names0 = new string[]
            {
                "TIME",
                "MEGA",
                "SUPER",
                "METAL",
                "ATOMIC",
                "AIR",
                "WILY",
                "ROLL",
                "RUSH",
                "FRANKERZ",
                "WATER",
                "GUTS",
                "ELEC",
                "GEMINI",
                "COSSACK",
                "TOAD",
                "HYPER",
                "TIME",
                "CRASH",
                "LEAF",
                "QUICK",
                "DRILL",
                "FLAME",
                "PLANT",
                "KNIGHT",
                "SILVER",
                "JUNK",
                "THUNDER",
                "WILD",
                "NOISE",
                "SLASH",
                "TORNADO",
                "ASTRO",
                "CLOWN",
                "SOLAR",
                "CHILL",
                "TRIPLE",
                "REBOUND",
                "NUDUA",
                "JOKA",
                "ELLO",
                "COOLKID",
                "PROTO",
                "PLUG",
                "RTA",
                "MASH",
                "BIG",
                "CUT",
                "URN",
                "TWITCH",
                "PRO",
                "ION",
                "AUTO",
                "BEAT",
            };

        static string[] Names1 = new string[]
        {
            "BLAST",
            "BLASTER",
            "FIRE",
            "CUTTER",
            "BLADE",
            "STOPPER",
            "GUN",
            "CANNON",
            "HIT",
            "SHOT",
            "COIL",
            "SHOOTER",
            "BOMB",
            "BOMBER",
            "LASER",
            "FLUSH",
            "BEAM",
            "DASH",
            "MISSILE",
            "STORM",
            "CUTTER",
            "SHIELD",
            "KNUCKLE",
            "SNAKE",
            "SHOCK",
            "SPIN",
            "CRUSHER",
            "HOLD",
            "EYE",
            "ATTACK",
            "KICK",
            "STONE",
            "WAVE",
            "SPEAR",
            "CLAW",
            "BALL",
            "TRIDENT",
            "WOOL",
            "SPIKE",
            "BLAZE",
            "STRIKER",
            "WALL",
            "BALLOON",
            "MARINE",
            "WIRE",
            "BURNER",
            "BUSTER",
            "ZIP",
            "GLITCH",
            "ADAPTER",
            "RAID",
            "DEVICE",
            "BOX",
            "AXE",
            "ARC",
            "JAB",
            "RESET",
            "STRAT",
        };
    }
}
