using System;
using System.Collections.Generic;
using System.IO;

namespace MM2Randomizer.Randomizers
{
    public class RWeaponNames
    {
        public static readonly int MAX_CHARS = 12;
        public static readonly int offsetLetters = 0x037e22;
        public static readonly int offsetAtomicFire = 0x037e2e;

        public RWeaponNames()
        {
            using (var stream = new FileStream(RandomMM2.DestinationFileName, FileMode.Open, FileAccess.ReadWrite))
            {
                // Write in new weapon names
                for (int i = 0; i < 8; i++)
                {
                    stream.Position = offsetAtomicFire + i * 0x10;

                    string name = GetRandomName();
                    char[] chars = name.ToCharArray();

                    for (int j = 0; j < MAX_CHARS; j++)
                    {
                        if (j < chars.Length)
                        {
                            byte b = Convert.ToByte(chars[j]);
                            stream.WriteByte(b);
                        }
                        else
                        {
                            stream.WriteByte(Convert.ToByte('@'));
                        }
                    }
                }

                // Erase "Boomerang" for now
                stream.Position = 0x037f5e;
                for (int i = 0; i < 10; i++)
                {
                    stream.WriteByte(Convert.ToByte('@'));
                }

                // Write in new weapon letters
                stream.Position = offsetLetters;
                for (int i = 0; i < 8; i ++)
                {
                    int randLetter = 0x41 + RandomMM2.Random.Next(26);
                    stream.WriteByte((byte)randLetter);
                }
            }
        }

        private string GetRandomName()
        {
            string finalName = "";

            // Start with random list
            int l = RandomMM2.Random.Next(1);
            string name0, name1;
            if (l == 0)
            {
                // Get random name from first list
                int random = RandomMM2.Random.Next(Names0.Length);
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
                    random = RandomMM2.Random.Next(names1Left.Count);
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
                int random = RandomMM2.Random.Next(Names1.Length);
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
                    random = RandomMM2.Random.Next(names0Left.Count);
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
                "WILY",
                "ROLL",
                "RUSH",
                "FRANKERZ",
                "WATER",
                "GUTS",
                "ELECT",
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
                "NUDUA",
                "JOKA",
                "COOLKID",
                "PROTO",
                "PLUG",
                "RTA",
                "MASH",
            };

        static string[] Names1 = new string[]
        {
                "BLASTER",
                "FIRE",
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
                "WALL",
                "BALLOON",
                "MARINE",
                "WIRE",
                "BURNER",
                "BUSTER",
                "SPIKE",
                "ZIP",
                "GLITCH",
                "ADAPTER",
        };
    }
}
