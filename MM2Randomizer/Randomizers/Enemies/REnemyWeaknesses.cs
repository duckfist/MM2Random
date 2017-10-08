using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MM2Randomizer.Patcher;

namespace MM2Randomizer.Randomizers.Enemies
{
    public class REnemyWeaknesses : IRandomizer
    {
        public static int EnemyDamageAddressP = 0x03E9A8;
        public static int EnemyDamageAddressH = 0x03EA24;
        public static int EnemyDamageAddressA = 0x03EA9C;
        public static int EnemyDamageAddressW = 0x03EB14;
        public static int EnemyDamageAddressB = 0x03EB8C;
        public static int EnemyDamageAddressQ = 0x03EC04;
        public static int EnemyDamageAddressC = 0x03EC7C;
        public static int EnemyDamageAddressM = 0x03ECF4;

        private StringBuilder debug = new StringBuilder();
        private List<string> enemyNames = new List<string>();
        private List<int> offsets = new List<int>();
        private List<byte> shotP = new List<byte>();
        private List<byte> shotH = new List<byte>();
        private List<byte> shotA = new List<byte>();
        private List<byte> shotW = new List<byte>();
        private List<byte> shotB = new List<byte>();
        private List<byte> shotQ = new List<byte>();
        private List<byte> shotC = new List<byte>();
        private List<byte> shotM = new List<byte>();

        public REnemyWeaknesses() { }

        public override string ToString()
        {
            return debug.ToString();
        }

        public void Randomize(Patch p, Random r)
        {
            string[] lines = Properties.Resources.enemyweakness.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            foreach (string line in lines)
            {
                if (line.StartsWith("#")) continue; // Ignore comment lines

                string[] cols = line.Split(new char[] { ',' });

                enemyNames.Add(cols[0]); // Enemy name
                offsets.Add(Convert.ToInt32(cols[1], 16)); // Offset
                shotP.Add(byte.Parse(cols[2])); // Buster
                shotH.Add(byte.Parse(cols[3])); // Heat
                shotA.Add(byte.Parse(cols[4])); // Air
                shotW.Add(byte.Parse(cols[5])); // Wood
                shotB.Add(byte.Parse(cols[6])); // Bubble
                shotQ.Add(byte.Parse(cols[7])); // Quick
                shotC.Add(byte.Parse(cols[8])); // Clash
                shotM.Add(byte.Parse(cols[9])); // Metal
            }

            shotP.Shuffle(r);
            shotH.Shuffle(r);
            shotA.Shuffle(r);
            shotW.Shuffle(r);
            shotB.Shuffle(r);
            shotQ.Shuffle(r);
            shotC.Shuffle(r);
            shotM.Shuffle(r);

            for (int i = 0; i < offsets.Count; i++)
            {
                p.Add(EnemyDamageAddressP + offsets[i], shotP[i], $"{enemyNames[i]} damage from P");
                p.Add(EnemyDamageAddressH + offsets[i], shotH[i], $"{enemyNames[i]} damage from H");
                p.Add(EnemyDamageAddressA + offsets[i], shotA[i], $"{enemyNames[i]} damage from A");
                p.Add(EnemyDamageAddressW + offsets[i], shotW[i], $"{enemyNames[i]} damage from W");
                p.Add(EnemyDamageAddressB + offsets[i], shotB[i], $"{enemyNames[i]} damage from B");
                p.Add(EnemyDamageAddressQ + offsets[i], shotQ[i], $"{enemyNames[i]} damage from Q");
                p.Add(EnemyDamageAddressC + offsets[i], shotC[i], $"{enemyNames[i]} damage from C");
                p.Add(EnemyDamageAddressM + offsets[i], shotM[i], $"{enemyNames[i]} damage from M");
            }

            // Format nice debug table
            debug.AppendLine("Enemy Weaknesses:");
            debug.AppendLine("\t\t\t\t\t\tP\tH\tA\tW\tB\tQ\tM\tC:");
            debug.AppendLine("--------------------------------------------------------");
            for (int i = 0; i < offsets.Count; i++)
            {
                debug.AppendLine($"{enemyNames[i]}\t{shotP[i]}\t{shotH[i]}\t{shotA[i]}\t{shotW[i]}\t{shotB[i]}\t{shotQ[i]}\t{shotC[i]}\t{shotM[i]}");
            }
            debug.Append(Environment.NewLine);
        }
    }
}
