using MM2Randomizer.Enums;
using MM2Randomizer.Patcher;
using System;
using System.Collections.Generic;

namespace MM2Randomizer.Randomizers
{
    public class RTeleporters : IRandomizer
    {
        public RTeleporters() { }

        public void Randomize(Patch patch, Random r)
        {
            // Create list of default teleporter position values
            List<byte[]> coords = new List<byte[]>
            {
                new byte[]{ 0x20, 0x3B }, // Teleporter X, Y (top-left)
                new byte[]{ 0x20, 0x7B },
                new byte[]{ 0x20, 0xBB },
                new byte[]{ 0x70, 0xBB },
                new byte[]{ 0x90, 0xBB },
                new byte[]{ 0xE0, 0x3B },
                new byte[]{ 0xE0, 0x7B },
                new byte[]{ 0xE0, 0xBB }
            };

            // Randomize them
            coords.Shuffle(r);

            // Write the new x-coordinates
            for (int i = 0; i < coords.Count; i++)
            {
                byte[] location = coords[i];
                patch.Add((int)(EMiscAddresses.WarpXCoordinateStartAddress + i), location[0], String.Format("Teleporter {0} X-Pos", i));
            }

            // Write the new y-coordinates
            for (int i = 0; i < coords.Count; i++)
            {
                byte[] location = coords[i];
                patch.Add((int)(EMiscAddresses.WarpYCoordinateStartAddress + i), location[1], String.Format("Teleporter {0} Y-Pos", i));
            }

            // These values will be copied over to $04b0 (y) and $0470 (x), which will be checked
            // for in real time to determine where Mega will teleport to
        }
    }
}
