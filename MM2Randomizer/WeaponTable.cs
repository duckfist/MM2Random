using MM2Randomizer.Enums;

namespace MM2Randomizer
{
    /// <summary>
    /// This object encapsulates 
    /// </summary>
    public class WeaponTable
    {
        /// <summary>
        /// The name of the weapon.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The unique ID of the weapon, which is referred to by the rest of the ROM.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Pointer to the damage table for this weapon.  This exact address refers to the
        /// damage done by this weapon against Heat Man, with the subsequent addresses 
        /// referring to the next Robot Masters.
        ///   Address + 0: Heat
        ///   Address + 1: Air
        ///   Address + 2: Wood
        ///   Address + 3: Bubble
        ///   Address + 4: Quick
        ///   Address + 5: Flash
        ///   Address + 6: Metal
        ///   Address + 7: Clash
        /// </summary>
        public ERMWeaponAddress Address { get; set; }

        /// <summary>
        /// The damage values used by this weapon against each Robot Master.  They will be
        /// inserted into the offset provided by "Address".
        ///   [0] = Heat
        ///   [1] = Air
        ///   [2] = Wood
        ///   [3] = Bubble
        ///   [4] = Quick
        ///   [5] = Flash
        ///   [6] = Metal
        ///   [7] = Clash
        /// </summary>
        public int[] RobotMasters { get; set; }


    }
}
