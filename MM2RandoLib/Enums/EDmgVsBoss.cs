using System.Collections.Generic;

namespace MM2Randomizer.Enums
{
    public sealed class EDmgVsBoss
    {
        public string WeaponName
        {
            get; private set;
        }

        public int Address
        {
            get; private set;
        }

        public static Dictionary<int, EDmgVsBoss> Addresses;

        //Japanese
        public static readonly EDmgVsBoss Buster          = new EDmgVsBoss(0x02E933, "Buster");
        public static readonly EDmgVsBoss AtomicFire      = new EDmgVsBoss(0x02E941, "Atomic Fire");
        public static readonly EDmgVsBoss AirShooter      = new EDmgVsBoss(0x02E94F, "Air Shooter");
        public static readonly EDmgVsBoss LeafShield      = new EDmgVsBoss(0x02E95D, "Leaf Shield");
        public static readonly EDmgVsBoss BubbleLead      = new EDmgVsBoss(0x02E96B, "Bubble Lead");
        public static readonly EDmgVsBoss QuickBoomerang  = new EDmgVsBoss(0x02E979, "Quick Boomerang");
        public static readonly EDmgVsBoss TimeStopper     = new EDmgVsBoss(0x02C049, "Time Stopper");
        public static readonly EDmgVsBoss MetalBlade      = new EDmgVsBoss(0x02E995, "Metal Blade");
        public static readonly EDmgVsBoss ClashBomber     = new EDmgVsBoss(0x02E987, "Clash Bomber");
        
        //English
        public static readonly EDmgVsBoss U_DamageP = new EDmgVsBoss(0x2e952, "Buster");
        public static readonly EDmgVsBoss U_DamageH = new EDmgVsBoss(0x2e960, "Atomic Fire");
        public static readonly EDmgVsBoss U_DamageA = new EDmgVsBoss(0x2e96e, "Air Shooter");
        public static readonly EDmgVsBoss U_DamageW = new EDmgVsBoss(0x2e97c, "Leaf Shield");
        public static readonly EDmgVsBoss U_DamageB = new EDmgVsBoss(0x2e98a, "Bubble Lead");
        public static readonly EDmgVsBoss U_DamageQ = new EDmgVsBoss(0x2e998, "Quick Boomerang");
        public static readonly EDmgVsBoss U_DamageF = new EDmgVsBoss(0x2C049, "Time Stopper");
        public static readonly EDmgVsBoss U_DamageM = new EDmgVsBoss(0x2e9b4, "Metal Blade");
        public static readonly EDmgVsBoss U_DamageC = new EDmgVsBoss(0x2e9a6, "Clash Bomber");

        static EDmgVsBoss()
        {
            Addresses = new Dictionary<int, EDmgVsBoss>()
            {
                { U_DamageP.Address, U_DamageP },
                { U_DamageH.Address, U_DamageH },
                { U_DamageA.Address, U_DamageA },
                { U_DamageW.Address, U_DamageW },
                { U_DamageB.Address, U_DamageB },
                { U_DamageQ.Address, U_DamageQ },
                { U_DamageF.Address, U_DamageF },
                { U_DamageM.Address, U_DamageM },
                { U_DamageC.Address, U_DamageC },
            };
        }

        private EDmgVsBoss(int address, string name)
        {
            this.Address = address;
            this.WeaponName = name;
        }

        public static implicit operator int (EDmgVsBoss eDmgVsBoss)
        {
            return eDmgVsBoss.Address;
        }

        public static implicit operator EDmgVsBoss (int eDmgVsBoss)
        {
            return Addresses[eDmgVsBoss];
        }

        public override string ToString()
        {
            return WeaponName;
        }

        /// <summary>
        /// Get a list of pointers to weapon damage tables against bosses, sorted by boss order
        /// </summary>
        /// <param name="includeBuster"></param>
        /// <param name="includeTimeStopper"></param>
        /// <returns></returns>
        public static List<EDmgVsBoss> GetTables(bool includeBuster, bool includeTimeStopper)
        {
            List<EDmgVsBoss> tables = new List<EDmgVsBoss>();
            if (includeBuster) tables.Add(U_DamageP);
            tables.Add(U_DamageH);
            tables.Add(U_DamageA);
            tables.Add(U_DamageW);
            tables.Add(U_DamageB);
            tables.Add(U_DamageQ);
            if (includeTimeStopper) tables.Add(U_DamageF);
            tables.Add(U_DamageM);
            tables.Add(U_DamageC);
            return tables;
        }

        /// <summary>
        /// 
        /// </summary>
        public class Offset
        {
            public string Name
            {
                get; private set;
            }

            public int Value
            {
                get; private set;
            }

            public static Dictionary<int, Offset> Offsets;

            public static readonly Offset Dragon    = new Offset(0x08, "Dragon");
            public static readonly Offset Guts      = new Offset(0x0A, "Guts");
            public static readonly Offset Machine   = new Offset(0x0C, "Machine");
            public static readonly Offset Alien     = new Offset(0x0D, "Alien");
            public static readonly Offset Heat      = new Offset(0x00, "Heat");
            public static readonly Offset Air       = new Offset(0x01, "Air");
            public static readonly Offset Wood      = new Offset(0x02, "Wood");
            public static readonly Offset Bubble    = new Offset(0x03, "Bubble");
            public static readonly Offset Quick     = new Offset(0x04, "Quick");
            public static readonly Offset Flash     = new Offset(0x05, "Flash");
            public static readonly Offset Metal     = new Offset(0x06, "Metal");
            public static readonly Offset Clash     = new Offset(0x07, "Clash");

            static Offset()
            {
                Offsets = new Dictionary<int, Offset>()
                {
                    { Dragon.Value  , Dragon  },
                    { Guts.Value    , Guts    },
                    { Machine.Value , Machine },
                    { Alien.Value   , Alien   },
                    { Heat.Value    , Heat    },
                    { Air.Value     , Air     },
                    { Wood.Value    , Wood    },
                    { Bubble.Value  , Bubble  },
                    { Quick.Value   , Quick   },
                    { Flash.Value   , Flash   },
                    { Metal.Value   , Metal   },
                    { Clash.Value   , Clash   },
                };
            }

            private Offset(int offset, string name)
            {
                this.Name = name;
                this.Value = offset;
            }

            public static implicit operator int (Offset offset)
            {
                return offset.Value;
            }

            public static implicit operator Offset(int offset)
            {
                return Offsets[offset];
            }

            public override string ToString()
            {
                return Name;
            }
        }


    }
}
