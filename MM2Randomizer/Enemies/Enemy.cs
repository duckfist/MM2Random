using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MM2Randomizer.Enemies
{
    public class Enemy
    {
        public string Name { get; set; }
        public byte ID { get; set; }
        public List<byte> PatternTableAddresses { get; set; }
        public List<int> SpriteBankRows { get; set; }

        public Enemy(string name, byte id, List<byte> patternTableAddresses, List<int> spriteBankRows)
        {
            Name = name;
            ID = id;
            PatternTableAddresses = patternTableAddresses;
            SpriteBankRows = spriteBankRows;
        }
    }
}
