using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MM2Randomizer.Randomizers.Enemies
{
    public class StageRoom
    {
        public int SpriteBankID { get; set; }
        public int RoomNum { get; set; }
        public List<EnemyInstance> Enemies {get;set;}

        public bool HasEnemies() { return Enemies.Count > 0; }
        //public bool IsRandomizable()
        //{
        //    foreach (EnemyInstance e in Enemies)
        //    {
        //        if (e.IsRandomiziable)
        //            return true;
        //    }
        //    return false;
        //}


        public StageRoom(int spriteBank)
        {
            SpriteBankID = spriteBank;
            Enemies = new List<EnemyInstance>();
        }
    }
}
