using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MM2Randomizer.Enums
{
    public enum ERMWeaponAddress
    {
        //Japanese
        Buster          = 0x02E933,
        AtomicFire      = 0x02E941,
        AirShooter      = 0x02E94F,
        LeafShield      = 0x02E95D,
        BubbleLead      = 0x02E96B,
        QuickBoomerang  = 0x02E979,
        TimeStopper     = 0x02C049,
        MetalBlade      = 0x02E995,
        ClashBomber     = 0x02E987,

        //English
        Eng_Buster          = 0x2e952,
        Eng_AtomicFire      = 0x2e960,
        Eng_AirShooter      = 0x2e96e,
        Eng_LeafShield      = 0x2e97c,
        Eng_BubbleLead      = 0x2e98a,
        Eng_QuickBoomerang  = 0x2e998,
        Eng_TimeStopper     = 0x2C049,
        Eng_MetalBlade      = 0x2e9b4,
        Eng_ClashBomber     = 0x2e9a6,

        Offset_Dragon = 0x08,
        Offset_Guts = 0x0A,
        Offset_Machine = 0x0C,
        Offset_Alien = 0x0D,

        Offset_Heat = 0x00,
        Offset_Air = 0x01,
        Offset_Wood = 0x02,
        Offset_Bubble = 0x03,
        Offset_Quick = 0x04,
        Offset_Flash = 0x05,
        Offset_Metal = 0x06,
        Offset_Clash = 0x07,
    }
}
