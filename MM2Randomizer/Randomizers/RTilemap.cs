using MM2Randomizer.Patcher;

using System;

namespace MM2Randomizer.Randomizers
{
    public class RTilemap : IRandomizer
    {
        public RTilemap() { }

        public void Randomize(Patch p, Random r)
        {
            ChangeW4FloorsBeforeSpikes(p, r);
            ChangeW4FloorsSpikePit(p, r);
        }

        private static void ChangeW4FloorsBeforeSpikes(Patch Patch, Random r)
        {
            // Choose 2 of the 5 32x32 tiles to be fake
            int tileA = r.Next(5);
            int tileB = r.Next(4);

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

        private static void ChangeW4FloorsSpikePit(Patch Patch, Random r)
        {
            // 5 tiles, but since two adjacent must construct a gap, 4 possible gaps.  Choose 1 random gap.
            int gap = r.Next(4);
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
