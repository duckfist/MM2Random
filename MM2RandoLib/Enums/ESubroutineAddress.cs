using System;
using System.Collections.Generic;
using System.Text;

namespace MM2Randomizer.Enums
{
    public enum ESubroutineAddress
    {
        // This subroutine appears to just
        // busy loop to waste a frame, used for example
        // under water to waste every 5th frame.
        WasteAFrame = 0x03c0e7,
        // This is the address of the branch instruction
        // responsible for audio processing when an NMI occurs in the middle of
        // a bank switch.
        ChangeBankBNE = 0x3C02F,
    }
}
