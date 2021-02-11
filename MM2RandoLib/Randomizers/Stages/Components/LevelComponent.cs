using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MM2Randomizer.Randomizers.Stages.Components
{
    public class LevelComponent
    {
        public String Name { get; set; }
        public String StartAddress { get; set; }
        public String EndAddress { get; set; }
        public IList<LevelComponentVariation> Variations { get; set; }
    }

    public class LevelComponentVariation
    {
        public String Name { get; set; }
        public String[] TsaMap { get; set; }
        public String Sprites { get; set; }
    }

    public class ComponentManager
    {
        public IList<LevelComponent> LevelComponents { get; set; }
    }
}
