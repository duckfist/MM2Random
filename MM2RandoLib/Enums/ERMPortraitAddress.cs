using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MM2Randomizer.Enums
{
    public enum ERMPortraitAddress
    {
        // StageSelect  Address    Value
        // -----------------------------
        // Bubble Man   0x034670   3
        // Air Man      0x034671   1
        // Quick Man    0x034672   4
        // Wood Man     0x034673   2
        // Crash Man    0x034674   7
        // Flash Man    0x034675   5
        // Metal Man    0x034676   6
        // Heat Man     0x034677   0

        HeatMan = 0x034677,
        
        AirMan = 0x034671,
        
        WoodMan = 0x034673,
        
        BubbleMan = 0x034670,
        
        QuickMan = 0x034672,
        
        FlashMan = 0x034675,
        
        MetalMan = 0x034676,
        
        CrashMan = 0x034674
    }
}
