using System;
using System.Collections.Generic;

using MM2Randomizer.Enums;
using MM2Randomizer.Patcher;

namespace MM2Randomizer.Randomizers.Colors
{
    public class ColorSet
    {
        public Int32[] addresses;
        public List<EColorsHex[]> ColorBytes;
        public Int32 Index; 

        public ColorSet()
        {
            ColorBytes = new List<EColorsHex[]>();
            Index = 0;
        }

        public void RandomizeAndWrite(Patch patch, Random rand, Int32 setNumber)
        {
            Index = rand.Next(ColorBytes.Count);

            for (Int32 i = 0; i < addresses.Length; i++)
            {
                patch.Add(addresses[i], (Byte)ColorBytes[Index][i], String.Format("Color Set {0} (Index Chosen: {1}) Value #{2}", setNumber, Index, i));
            }
        }
    }
}
