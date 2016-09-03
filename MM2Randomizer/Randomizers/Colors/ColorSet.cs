using System;
using System.Collections.Generic;

using MM2Randomizer.Enums;
using MM2Randomizer.Patcher;

namespace MM2Randomizer.Randomizers.Colors
{
    public class ColorSet
    {
        public int[] addresses;
        public List<EColorsHex[]> ColorBytes;
        public int Index; 

        public ColorSet()
        {
            ColorBytes = new List<EColorsHex[]>();
            Index = 0;
        }

        public void RandomizeAndWrite(Random rand, int setNumber)
        {
            Index = rand.Next(ColorBytes.Count);

            for (int i = 0; i < addresses.Length; i++)
            {
                Patch.Add(addresses[i], (byte)ColorBytes[Index][i], String.Format("Color Set {0} (Index Chosen: {1}) Value #{2}", setNumber, Index, i));
            }
        }
    }
}
