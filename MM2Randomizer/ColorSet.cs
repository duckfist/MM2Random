using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MM2Randomizer
{
    public class ColorSet
    {
        public int[,] addresses;
        public List<byte[,]> ColorBytes;
        public int Index; 

        public ColorSet()
        {
            ColorBytes = new List<byte[,]>();
            Index = 0;
        }

        public void RandomizeAndWrite(System.IO.FileStream stream, Random rand)
        {
            Index = rand.Next(ColorBytes.Count);

            for (int i = 0; i < addresses.GetLength(0); i++)
            {
                for (int j = 0; j < addresses.GetLength(1); j++)
                {
                    stream.Position = addresses[i, j];
                    stream.WriteByte(ColorBytes[Index][i,j]);
                }
            }
        }
    }
}
