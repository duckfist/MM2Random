using System.Collections.Generic;
using System.IO;

using MM2Randomizer.Enums;
using MM2Randomizer.Patcher;
using System;

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

            for (int i = 0; i < 5; i++)
            {
                if (i == tileA || i == tileB)
                {
                    Patch.Add(0x00CB5C + i * 8, 0x94, String.Format("Wily 4 Room 4 Tile {0} (fake)", i));
                }
                else
                {
                    Patch.Add(0x00CB5C + i * 8, 0x85, String.Format("Wily 4 Room 4 Tile {0} (solid)", i));
                }
            }
        }

        private static void ChangeW4FloorsSpikePit()
        {
            // 5 tiles, but since two adjacent must construct a gap, 4 possible gaps.  Choose 1 random gap.
            int gap = RandomMM2.Random.Next(4);
            for (int i = 0; i < 4; i++)
            {
                if (i == gap)
                {
                    Patch.Add(0x00CB9A + i * 8, 0x9B, String.Format("Wily 4 Room 5 Tile {0} (gap on right)", i));
                    Patch.Add(0x00CB9A + i * 8 + 7, 0x9C, String.Format("Wily 4 Room 5 Tile {0} (gap on left)", i));
                    ++i; // skip next tile since we just drew it
                }
                else
                {
                    Patch.Add(0x00CB9A + i * 8, 0x9D, String.Format("Wily 4 Room 5 Tile {0} (solid)", i));
                }
            }
        }
    }
}
