using System;
using MM2Randomizer.Enums;

namespace MM2Randomizer.Randomizers.Stages
{
    /// <summary>
    /// This Object encapsulates the relevant ROM offsets for properties of each 
    /// selectable Robot Master portrait on the Stage Select screen.
    /// </summary>
    public class StageFromSelect
    {
        public String PortraitName;
        public ERMPortraitText TextAddress;
        public String TextValues;
        public ERMPortraitAddress PortraitAddress;
        public Int32 PortraitDestinationOriginal;
        public Int32 PortraitDestinationNew;
        public ERMStageClearAddress StageClearAddress;
        public Int32 StageClearDestinationOriginal;
        public Int32 StageClearDestinationNew;
    }
}
