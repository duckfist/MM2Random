using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MM2Randomizer.Enums
{
    public enum ERMStageWeapon
    {
        // StageBeat    Address    Value
        // -----------------------------
        // Heat Man     0x03C289   1
        // Air Man      0x03C28A   2
        // Wood Man     0x03C28B   4
        // Bubble Man   0x03C28C   8
        // Quick Man    0x03C28D   16
        // Flash Man    0x03C28E   32
        // Metal Man    0x03C28F   64
        // Crash Man    0x03C290   128

        HeatMan = 0x03C289,

        AirMan = 0x03C28A,

        WoodMan = 0x03C28B,

        BubbleMan = 0x03C28C,

        QuickMan = 0x03C28D,

        FlashMan = 0x03C28E,

        MetalMan = 0x03C28F,

        CrashMan = 0x03C290,
    }
}
