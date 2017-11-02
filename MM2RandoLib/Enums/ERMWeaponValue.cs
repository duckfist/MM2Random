using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MM2Randomizer.Enums
{
    public enum ERMWeaponValueBit
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
        HeatMan = 0x01,
        AirMan = 0x02,
        WoodMan = 0x04,
        BubbleMan = 0x08,
        QuickMan = 0x10,
        FlashMan = 0x20,
        MetalMan = 0x40,
        CrashMan = 0x80
    }

    public enum EWeaponIndex
    {
        Heat = 0x01,
        Air = 0x02,
        Wood = 0x03,
        Bubble = 0x04,
        Quick = 0x05,
        Clash = 0x06,
        Metal = 0x07,
    }
}
