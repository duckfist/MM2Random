using System;
using System.Collections.Generic;
using System.Text;
using MM2Randomizer.Extensions;
using MM2Randomizer.Patcher;

namespace MM2Randomizer.Randomizers.Enemies
{
    public class REnemyWeaknesses : IRandomizer
    {
        private readonly static int EnemyDamageAddressP = 0x03E9A8;
        private readonly static int EnemyDamageAddressH = 0x03EA24;
        private readonly static int EnemyDamageAddressA = 0x03EA9C;
        private readonly static int EnemyDamageAddressW = 0x03EB14;
        private readonly static int EnemyDamageAddressB = 0x03EB8C;
        private readonly static int EnemyDamageAddressQ = 0x03EC04;
        private readonly static int EnemyDamageAddressC = 0x03EC7C;
        private readonly static int EnemyDamageAddressM = 0x03ECF4;

        // NOTE: Will have to change these indices if enemies are added/removed from enemyweaknesses.csv!
        private readonly static int EnemyIndexInShotArray_Friender = 8;

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
            EnemyWeaknessSet enemyWeaknessSet = Properties.Resources.EnemyWeaknessSet.Deserialize<EnemyWeaknessSet>();

            foreach (EnemyWeakness enemyWeakness in enemyWeaknessSet)
            {
                if (true == enemyWeakness.Enabled)
                {
                    enemyNames.Add(enemyWeakness.Name);
                    offsets.Add(Convert.ToInt32(enemyWeakness.Offset, 16));
                    shotP.Add(Byte.Parse(enemyWeakness.Buster));
                    shotH.Add(Byte.Parse(enemyWeakness.Heat));
                    shotA.Add(Byte.Parse(enemyWeakness.Air));
                    shotW.Add(Byte.Parse(enemyWeakness.Wood));
                    shotB.Add(Byte.Parse(enemyWeakness.Bubble));
                    shotQ.Add(Byte.Parse(enemyWeakness.Quick));
                    shotC.Add(Byte.Parse(enemyWeakness.Crash));
                    shotM.Add(Byte.Parse(enemyWeakness.Metal));
                }
            }

            shotP.Shuffle(r);
            shotH.Shuffle(r);
            shotA.Shuffle(r);
            shotW.Shuffle(r);
            shotB.Shuffle(r);
            shotQ.Shuffle(r);
            shotC.Shuffle(r);
            shotM.Shuffle(r);

            // Force Buster to always do 1 damage to minibosses
            shotP[EnemyIndexInShotArray_Friender] = 0x01;

            // To each enemy...
            for (int i = 0; i < offsets.Count; i++)
            {
                // ...apply each weapon's damage
                p.Add(EnemyDamageAddressP + offsets[i], shotP[i], $"{enemyNames[i]} damage from P");
                p.Add(EnemyDamageAddressH + offsets[i], shotH[i], $"{enemyNames[i]} damage from H");
                p.Add(EnemyDamageAddressA + offsets[i], shotA[i], $"{enemyNames[i]} damage from A");
                p.Add(EnemyDamageAddressW + offsets[i], shotW[i], $"{enemyNames[i]} damage from W");
                p.Add(EnemyDamageAddressB + offsets[i], shotB[i], $"{enemyNames[i]} damage from B");
                p.Add(EnemyDamageAddressQ + offsets[i], shotQ[i], $"{enemyNames[i]} damage from Q");
                p.Add(EnemyDamageAddressC + offsets[i], shotC[i], $"{enemyNames[i]} damage from C");
                p.Add(EnemyDamageAddressM + offsets[i], shotM[i], $"{enemyNames[i]} damage from M");

                // Furthermore, there are 3 enemy types that need a second array of damage values 
                // - Shrink (instance vs. spawner)
                // - Mole (moving up vs. moving down)
                // - Shotman (facing left vs. facing right)
                // The corresponding auxiliary types are omitted from the shuffle.
                // Instead, assign common weaknesses for a more consistent playing experience.
                // Each auxiliary type occurs at the next offset, offsets[i] + 1

                // Shrink 0x00 apply same damage to Shrink Spawner 0x01
                // Mole (Up) 0x48 apply same damage to Mole (Down) 0x49
                // Shotman (Left) 0x4B apply same damage to Shotman (Right) 0x4C
                if (offsets[i] == 0x00 || offsets[i] == 0x48 || offsets[i] == 0x4B)
                {
                    p.Add(EnemyDamageAddressP + offsets[i] + 1, shotP[i], $"{enemyNames[i]} damage from P");
                    p.Add(EnemyDamageAddressH + offsets[i] + 1, shotH[i], $"{enemyNames[i]} damage from H");
                    p.Add(EnemyDamageAddressA + offsets[i] + 1, shotA[i], $"{enemyNames[i]} damage from A");
                    p.Add(EnemyDamageAddressW + offsets[i] + 1, shotW[i], $"{enemyNames[i]} damage from W");
                    p.Add(EnemyDamageAddressB + offsets[i] + 1, shotB[i], $"{enemyNames[i]} damage from B");
                    p.Add(EnemyDamageAddressQ + offsets[i] + 1, shotQ[i], $"{enemyNames[i]} damage from Q");
                    p.Add(EnemyDamageAddressC + offsets[i] + 1, shotC[i], $"{enemyNames[i]} damage from C");
                    p.Add(EnemyDamageAddressM + offsets[i] + 1, shotM[i], $"{enemyNames[i]} damage from M");
                }
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
