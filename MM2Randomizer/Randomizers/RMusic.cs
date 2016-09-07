using MM2Randomizer.Enums;
using MM2Randomizer.Patcher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MM2Randomizer.Randomizers
{
    public class RMusic : IRandomizer
    {
        public RMusic() { }

        public void Randomize(Patch p, Random r)
        {
            List<EMusicID> newBGMOrder = new List<EMusicID>();
            List<EMusicID> robos = new List<EMusicID>();

            // Select 2 replacement tracks for the 2 extra instance of the boring W3/4/5 theme
            robos.Add(EMusicID.Flash);
            robos.Add(EMusicID.Heat);
            robos.Add(EMusicID.Air);
            robos.Add(EMusicID.Wood);
            robos.Add(EMusicID.Quick);
            robos.Add(EMusicID.Metal);
            robos.Add(EMusicID.Clash);
            robos.Add(EMusicID.Bubble);
            robos.Shuffle(r);

            newBGMOrder.Add(EMusicID.Flash);
            newBGMOrder.Add(EMusicID.Heat);
            newBGMOrder.Add(EMusicID.Air);
            newBGMOrder.Add(EMusicID.Wood);
            newBGMOrder.Add(EMusicID.Quick);
            newBGMOrder.Add(EMusicID.Metal);
            newBGMOrder.Add(EMusicID.Clash);
            newBGMOrder.Add(EMusicID.Bubble);
            newBGMOrder.Add(EMusicID.Wily12);
            newBGMOrder.Add(EMusicID.Wily12);  // Wily 1/2 track will play twice
            newBGMOrder.Add(EMusicID.Wily345); // Wily 3/4/5 track only plays once
            newBGMOrder.Add(robos[0]);         // Add extra Robot Master tracks to the set
            newBGMOrder.Add(robos[1]);

            // Randomize tracks
            newBGMOrder.Shuffle(r);

            // Start writing at Heatman BGM ID, both J and U
            // Loop through BGM addresses Heatman to Wily 5 (Wily 6 still silent)
            for (int i = 0; i < newBGMOrder.Count; i++)
            {
                EMusicID bgm = newBGMOrder[i];
                p.Add(0x0381E0 + i, (byte)bgm, String.Format("BGM Stage {0}", i));
            }

            // Finally, fix Wily 5 track when exiting a Teleporter to be the selected Wily 5 track instead of default
            p.Add(0x038489, (byte)newBGMOrder.Last(), "BGM Wily 5 Teleporter Exit Fix");

        }
    }
}
