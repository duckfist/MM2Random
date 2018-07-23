using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MM2Randomizer.Randomizers.Stages.Components
{
    public class LevelComponent
    {
        public string Name { get; set; }
        public string StartAddress { get; set; }
        public string EndAddress { get; set; }
        public IList<LevelComponentVariation> Variations { get; set; }
    }

    public class LevelComponentVariation
    {
        public string Name { get; set; }
        public string[] TsaMap { get; set; }
        public string Sprites { get; set; }
    }

    public class ComponentManager
    {
        public IList<LevelComponent> LevelComponents { get; set; }
    }
}
