using System.Collections.Generic;

namespace MM2Randomizer.Enums
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class EDmgVsEnemy
    {
        public int Address
        {
            get; private set;
        }

        public static Dictionary<int, EDmgVsEnemy> Addresses { get; set; }

        public static readonly EDmgVsEnemy DamageP = new EDmgVsEnemy(0x03E9A8);
        public static readonly EDmgVsEnemy DamageH = new EDmgVsEnemy(0x03EA24);
        public static readonly EDmgVsEnemy DamageA = new EDmgVsEnemy(0x03EA9C);
        public static readonly EDmgVsEnemy DamageW = new EDmgVsEnemy(0x03EB14);
        public static readonly EDmgVsEnemy DamageB = new EDmgVsEnemy(0x03EB8C);
        public static readonly EDmgVsEnemy DamageQ = new EDmgVsEnemy(0x03EC04);
        public static readonly EDmgVsEnemy DamageM = new EDmgVsEnemy(0x03ECF4);
        public static readonly EDmgVsEnemy DamageC = new EDmgVsEnemy(0x03EC7C);

        static EDmgVsEnemy()
        {
            Addresses = new Dictionary<int, EDmgVsEnemy>()
            {
                { DamageP.Address, DamageP },
                { DamageH.Address, DamageH },
                { DamageA.Address, DamageA },
                { DamageW.Address, DamageW },
                { DamageB.Address, DamageB },
                { DamageQ.Address, DamageQ },
                { DamageM.Address, DamageM },
                { DamageC.Address, DamageC },
            };
        }

        private EDmgVsEnemy(int address)
        {
            this.Address = address;
        }

        public static implicit operator int (EDmgVsEnemy eDmgVsEnemy)
        {
            return eDmgVsEnemy.Address;
        }

        public static implicit operator EDmgVsEnemy(int eDmgVsEnemy)
        {
            return Addresses[eDmgVsEnemy];
        }

        public static bool operator ==(EDmgVsEnemy a, EDmgVsEnemy b)
        {
            return (a.Address == b.Address);
        }

        public static bool operator !=(EDmgVsEnemy a, EDmgVsEnemy b)
        {
            return (a.Address != b.Address);
        }
        public static bool operator ==(int a, EDmgVsEnemy b)
        {
            return (a == b.Address);
        }

        public static bool operator !=(int a, EDmgVsEnemy b)
        {
            return (a != b.Address);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<EDmgVsEnemy> GetTables(bool includeBuster)
        {
            List<EDmgVsEnemy> list = new List<EDmgVsEnemy>()
            {
                DamageH,
                DamageA,
                DamageW,
                DamageB,
                DamageQ,
                DamageM,
                DamageC,
            };

            if (includeBuster)
            {
                list.Insert(0, DamageP);
            }

            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        public class Offset
        {
            public int Value
            {
                get; private set;
            }

            public static readonly Offset PicopicoKun;

            static Offset()
            {
                PicopicoKun = new Offset(0x6A);
            }

            private Offset(int offset)
            {
                this.Value = offset;
            }

            public static implicit operator int (Offset offset)
            {
                return offset.Value;
            }

            public static bool operator ==(Offset a, Offset b)
            {
                return (a.Value == b.Value);
            }

            public static bool operator !=(Offset a, Offset b)
            {
                return (a.Value != b.Value);
            }

            public static bool operator ==(int a, Offset b)
            {
                return (a == b.Value);
            }

            public static bool operator !=(int a, Offset b)
            {
                return (a != b.Value);
            }

            public override bool Equals(object obj)
            {
                return base.Equals(obj);
            }

            public override int GetHashCode()
            {
                return base.GetHashCode();
            }
        }
    }
}
