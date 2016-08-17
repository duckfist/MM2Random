using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

namespace MM2Randomizer.Randomizers.Enemies
{
    class StagesTest
    {
        List<EnemyInstance> AllEnemyInstances = new List<EnemyInstance>();

        public StagesTest()
        {
            using (StreamReader sr = new StreamReader("enemylist.csv"))
            {
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    string[] args = line.Split(new char[] { ',' });

                    EnemyInstance enemy = new EnemyInstance(
                        Convert.ToInt32(args[0], 16),
                        Convert.ToInt32(args[1], 16),
                        Convert.ToInt32(args[2], 16),
                        Convert.ToInt32(args[3], 16),
                        Int32.Parse(args[4]));
                    AllEnemyInstances.Add(enemy);
                }
            }


        }
    }
}
