using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MM2Randomizer
{
    public class ColorSet
    {
        public int[] addresses;
        public List<byte[]> ColorBytes;
        public int Index; 

        public ColorSet()
        {
            ColorBytes = new List<byte[]>();
            Index = 0;
        }

        public void RandomizeAndWrite(System.IO.FileStream stream, Random rand)
        {
            Index = rand.Next(ColorBytes.Count);

            for (int i = 0; i < addresses.Length; i++)
            {
                stream.Position = addresses[i];
                stream.WriteByte(ColorBytes[Index][i]);
            }
        }
    }
}
