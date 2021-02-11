using System;
using MM2Randomizer.Enums;

namespace MM2Randomizer.Randomizers.Enemies
{
    public class EnemyInstance
    {
        public Boolean HasIDChanged
        {
            get { return EnemyID != EnemyIDPrev; }
        }

        public Int32 Offset { get; set; }
        public Int32 StageNum { get; set; }
        public Int32 RoomNum { get; set; }
        public Int32 ScreenNum { get; set; }
        public Boolean IsActive { get; set; }
        public Int32 EnemyID { get; set; }
        public Int32 EnemyIDPrev { get; set; } // for reference only
        public Int32 X { get; set; }
        public Int32 YOriginal { get; set; } // for reference only
        public Int32 YAir { get; set; }
        public Int32 YGround { get; set; }
        public Boolean IsFaceRight { get; set; }
        
        //public Boolean IsRandomiziable { get; set; }
        //public List<Byte> PatternTableAddresses { get; set; }
        //public List<Int32> SpriteBankRows { get; set; }
        
        public EnemyInstance(Int32 offset, Int32 stage, Int32 room, Int32 screen, Boolean isActive, Int32 id, Int32 xOriginal, Int32 yOriginal, Int32 yAir, Int32 yGround, Boolean faceRight)
        {
            Offset = offset;
            StageNum = stage;
            RoomNum = room;
            ScreenNum = screen;
            IsActive = isActive;
            EnemyID = id;
            EnemyIDPrev = id;
            X = xOriginal;
            YOriginal = yOriginal;
            YAir = yAir;
            YGround = yGround;
            IsFaceRight = faceRight;
        }

        public Boolean HasNewActivator()
        {
            if (!HasIDChanged)
            {
                return false;
            }

            switch ((EEnemyID)EnemyID)
            {
                case EEnemyID.Pipi_Activator:
                {
                    return true;
                }

                case EEnemyID.Mole_Activator:
                {
                    return true;
                }

                case EEnemyID.Claw_Activator:
                {
                    return true;
                }

                case EEnemyID.Kukku_Activator:
                {
                    return true;
                }

                case EEnemyID.M445_Activator:
                {
                    return true;
                }

                default:
                {
                    return false;
                }
            }
        }
    }
}