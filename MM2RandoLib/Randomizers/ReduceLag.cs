using System;

using MM2Randomizer.Patcher;
using MM2Randomizer.Enums;

namespace MM2Randomizer.Randomizers
{
    public class ReduceLag : IRandomizer
    {
        public ReduceLag() { }
        public void Randomize(Patch patch, Random r)
        {
            patch.Add((int)ESubroutineAddress.WasteAFrame, (byte)EInstruction.RTS, "Turn the 'waste a frame' subroutine into a NOP");
        }
    }

    
}
