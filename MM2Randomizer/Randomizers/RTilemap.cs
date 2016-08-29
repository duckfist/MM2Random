using System.Collections.Generic;
using System.IO;

using MM2Randomizer.Enums;

namespace MM2Randomizer.Randomizers
{
    public class RTilemap
    {
        public RTilemap()
        {
            ChangeW4FloorsBeforeSpikes();
            ChangeW4FloorsSpikePit();
        }

        private static void ChangeW4FloorsBeforeSpikes()
        {
            // Choose 2 of the 5 32x32 tiles to be fake
            int tileA = RandomMM2.Random.Next(5);
            int tileB = RandomMM2.Random.Next(4);

            // Make sure 2nd tile chosen is different
            if (tileB == tileA) tileB++;

            using (var stream = new FileStream(RandomMM2.DestinationFileName, FileMode.Open, FileAccess.ReadWrite))
            {
                for (int i = 0; i < 5; i++)
                {
                    stream.Position = 0x00CB5C + i * 8;

                    if (i == tileA || i == tileB)
                    {
                        stream.WriteByte(0x94); // Fake tile
                    }
                    else
                    {
                        stream.WriteByte(0x85); // Solid tile
                    }
                }
            }
        }

        private static void ChangeW4FloorsSpikePit()
        {
            // 5 tiles, but since two adjacent must construct a gap, 4 possible gaps.  Choose 1 random gap.
            int gap = RandomMM2.Random.Next(4);
            
            using (var stream = new FileStream(RandomMM2.DestinationFileName, FileMode.Open, FileAccess.ReadWrite))
            {
                for (int i = 0; i < 4; i++)
                {
                    stream.Position = 0x00CB9A + i * 8;

                    if (i == gap)
                    {
                        stream.WriteByte(0x9B); // Gap on right side
                        stream.Position += 7;
                        stream.WriteByte(0x9C); // Gap on left side
                        ++i; // skip next tile since we just drew it
                    }
                    else
                    {
                        stream.WriteByte(0x9D); // Solid tile
                    }
                    
                }
            }
        }

    }
}
