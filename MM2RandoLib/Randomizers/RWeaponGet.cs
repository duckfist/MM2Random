using MM2Randomizer.Enums;
using MM2Randomizer.Patcher;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MM2Randomizer.Randomizers
{
    public class RWeaponGet : IRandomizer
    {
        private List<ERMWeaponValueBit> NewWeaponOrder;

        public RWeaponGet()
        {
            NewWeaponOrder = new List<ERMWeaponValueBit>()
            {
                ERMWeaponValueBit.HeatMan,
                ERMWeaponValueBit.AirMan,
                ERMWeaponValueBit.WoodMan,
                ERMWeaponValueBit.BubbleMan,
                ERMWeaponValueBit.QuickMan,
                ERMWeaponValueBit.FlashMan,
                ERMWeaponValueBit.MetalMan,
                ERMWeaponValueBit.CrashMan
            }.Select(s => s).ToList();
        }

        /// <summary>
        /// Shuffle which Robot Master awards which weapon.
        /// </summary>
        public void Randomize(Patch Patch, Random r)
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
            NewWeaponOrder.Shuffle(r);

            // Create table for which weapon is awarded by which robot master
            // This also affects which portrait is blacked out on the stage select
            // This also affects which teleporter deactivates after defeating a Wily 5 refight boss
            for (Int32 i = 0; i < 8; i++)
            {
                Patch.Add((Int32)(ERMStageWeaponAddress.HeatMan + i), (Byte)NewWeaponOrder[i], $"{(EDmgVsBoss.Offset)i} Weapon Get");
            }

            // Create a copy of the default weapon order table to be used by teleporter function
            // This is needed to fix teleporters breaking from the new weapon order.
            // Unused space at end of bank
            Patch.Add(0x03f310, (Byte)ERMWeaponValueBit.HeatMan, "Custom Array of Default Weapon Order");
            Patch.Add(0x03f311, (Byte)ERMWeaponValueBit.AirMan, "Custom Array of Default Weapon Order");
            Patch.Add(0x03f312, (Byte)ERMWeaponValueBit.WoodMan, "Custom Array of Default Weapon Order");
            Patch.Add(0x03f313, (Byte)ERMWeaponValueBit.BubbleMan, "Custom Array of Default Weapon Order");
            Patch.Add(0x03f314, (Byte)ERMWeaponValueBit.QuickMan, "Custom Array of Default Weapon Order");
            Patch.Add(0x03f315, (Byte)ERMWeaponValueBit.FlashMan, "Custom Array of Default Weapon Order");
            Patch.Add(0x03f316, (Byte)ERMWeaponValueBit.MetalMan, "Custom Array of Default Weapon Order");
            Patch.Add(0x03f317, (Byte)ERMWeaponValueBit.CrashMan, "Custom Array of Default Weapon Order");

            // Change function to call $f300 instead of $c279 when looking up defeated refight boss to
            // get our default weapon table, fixing the teleporter softlock
            Patch.Add(0x03843b, 0x00, "Teleporter Fix Custom Function Call Byte 1");
            Patch.Add(0x03843c, 0xf3, "Teleporter Fix Custom Function Call Byte 2");

            // Create table for which stage is selectable on the stage select screen (independent of it being blacked out)
            for (Int32 i = 0; i < 8; i++)
            {
                Patch.Add((Int32)(ERMStageSelect.FirstStageInMemory + i), (Byte)NewWeaponOrder[i], "Selectable Stage Fix for Random Weapon Get");
            }
        }

        public void FixPortraits(ref Byte[] portraitBG_x, ref Byte[] portraitBG_y)
        {
            // Since the acquired-weapons table's elements are powers of two, get a new array of their 0-7 index
            Int32[] newWeaponIndex = GetShuffleIndexPermutation();

            // Permute portrait x/y values via the shuffled acquired-weapons array 
            Byte[] cpy = new Byte[8];

            for (Int32 i = 0; i < 8; i++)
            {
                cpy[newWeaponIndex[i]] = portraitBG_y[i];
            }

            Array.Copy(cpy, portraitBG_y, 8);

            for (Int32 i = 0; i < 8; i++)
            {
                cpy[newWeaponIndex[i]] = portraitBG_x[i];
            }

            Array.Copy(cpy, portraitBG_x, 8);
        }

        /// <summary>
        /// Get an array of the shuffled acquired-weapons' 0-7 index, since the original table's elements are bitwise/powers of 2.
        /// Uses the field <see cref="NewWeaponOrder"/>. Must be called after <see cref="Randomize(Patch, Random)"/>.
        /// </summary>
        /// <returns>An array of the new locations of the 8 awarded weapons. The index represents the original robot master index,
        /// in the order H A W B Q F M C. The value represents the index of the new location.
        /// </returns>
        public Int32[] GetShuffleIndexPermutation()
        {
            Int32[] newWeaponIndex = new Int32[8];
            for (Int32 i = 0; i < 8; i++)
            {
                Int32 j = 0;
                Byte val = (Byte)NewWeaponOrder[i];
                while (val != 0)
                {
                    val = (Byte)(val >> 1);
                    j++;
                }
                newWeaponIndex[i] = j - 1;
            }
            return newWeaponIndex;
        }
    }
}
