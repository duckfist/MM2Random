using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MM2Randomizer.Randomizers.Enemies
{
    public class Stage
    {
        public int SpriteBankIDRoom0Address { get; set; }
        public int SpriteBank0Address { get; set; }

        public List<StageRoom> Rooms;
        public List<EnemyType> Enemies;

        public Stage()
        {
            Rooms = new List<StageRoom>();
            Enemies = new List<EnemyType>();
        }
    }
}
