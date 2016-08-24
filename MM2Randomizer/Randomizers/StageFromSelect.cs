using MM2Randomizer.Enums;

namespace MM2Randomizer.Randomizers
{
    /// <summary>
    /// This object encapsulates the relevant ROM offsets for properties of each 
    /// selectable Robot Master portrait on the Stage Select screen.
    /// </summary>
    public class StageFromSelect
    {
        public string PortraitName;
        public ERMPortraitAddress PortraitAddress;
        public int PortraitDestinationOriginal;
        public int PortraitDestinationNew;
        public ERMStageClearAddress StageClearAddress;
        public int StageClearDestinationOriginal;
        public int StageClearDestinationNew;
    }
}
