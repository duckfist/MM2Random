using System;
using System.Collections.Generic;

using MM2Randomizer.Enums;

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

        public void RandomizeAndWrite(System.IO.FileStream stream, Random rand)
        {
            Index = rand.Next(ColorBytes.Count);

            for (int i = 0; i < addresses.Length; i++)
            {
                stream.Position = addresses[i];
                stream.WriteByte((byte)ColorBytes[Index][i]);
            }
        }
    }
}
