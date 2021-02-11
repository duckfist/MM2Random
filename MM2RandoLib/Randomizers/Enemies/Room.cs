using System;
using System.Collections.Generic;
using MM2Randomizer.Enums;

namespace MM2Randomizer.Randomizers.Enemies
{
    public class Room
    {
        public List<EnemyInstance> EnemyInstances { get; set; }
        public Int32 RoomNum { get; set; }

        public Room(Int32 roomNum)
        {
            RoomNum = roomNum;
            EnemyInstances = new List<EnemyInstance>();
        }
        
        public EEnemyID? GetActivatorIfOneHasBeenAdded()
        {
            foreach (EnemyInstance enemy in EnemyInstances)
            {
                // This instance hasn't been replaced; keep old activator type if one is there
                if (!enemy.HasIDChanged)
                {
                    continue;
                }

                EEnemyID id = (EEnemyID)enemy.EnemyID;

                if (id == EEnemyID.Pipi_Activator ||
                    id == EEnemyID.Mole_Activator ||
                    id == EEnemyID.Claw_Activator ||
                    id == EEnemyID.M445_Activator ||
                    id == EEnemyID.Kukku_Activator)
                {
                    return id;
                }
            }

            return null;
        }
    }
}
