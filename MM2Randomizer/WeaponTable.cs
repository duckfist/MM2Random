using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MM2Randomizer
{
    public class WeaponTable
    {
        public string Name { get; set; }
        public int ID { get; set; }
        // pointer to the damage table for this weapon at offset 0 (Heat Man)
        public int Address { get; set; } 

        public int[] RobotMasters { get; set; }

        //public int VsHeat0 { get; set; }
        //public int VsAir1 { get; set; }
        //public int VsWood2 { get; set; }
        //public int VsBubble3 { get; set; }
        //public int VsQuick4 { get; set; }
        //public int VsFlash5 { get; set; }
        //public int VsMetal6 { get; set; }
        //public int VsClash7 { get; set; }
    }
}
