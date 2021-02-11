using System;
using System.Collections.Generic;

using MM2Randomizer.Patcher;
using MM2Randomizer.Enums;

namespace MM2Randomizer.Randomizers
{
    public class RItemGet : IRandomizer
    {
        public RItemGet() { }

        /// <summary>
        /// Shuffle which Robot Master awards Items 1, 2, and 3.
        /// </summary>
        public void Randomize(Patch patch, Random r)
        {
            // 0x03C291 - Item # from Heat Man
            // 0x03C292 - Item # from Air Man
            // 0x03C293 - Item # from Wood Man
            // 0x03C294 - Item # from Bubble Man
            // 0x03C295 - Item # from Quick Man
            // 0x03C296 - Item # from Flash Man
            // 0x03C297 - Item # from Metal Man
            // 0x03C298 - Item # from Crash Man

            List<EItemNumber> newItemOrder = new List<EItemNumber>();

            for (Byte i = 0; i < 5; i++)
            {
                newItemOrder.Add(EItemNumber.None);
            }

            newItemOrder.Add(EItemNumber.One);
            newItemOrder.Add(EItemNumber.Two);
            newItemOrder.Add(EItemNumber.Three);
            newItemOrder.Shuffle(r);

            for (Int32 i = 0; i < 8; i++)
            {
                patch.Add((Int32)EItemStageAddress.HeatMan + i, (Byte)newItemOrder[i], String.Format("{0}man Item Get", ((EDmgVsBoss.Offset)i).ToString()));
            }
        }
    }
}
