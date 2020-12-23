using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MM2Randomizer.Randomizers
{
    [Serializable]
    public class Enemy
    {
        [XmlElement("Index")]
        public String Index { get; set; }

        [XmlElement("StageNumber")]
        public String StageNumber { get; set; }

        [XmlElement("RoomNumber")]
        public String RoomNumber { get; set; }

        [XmlElement("ScreenNumber")]
        public String ScreenNumber { get; set; }

        [XmlElement("IsActive")]
        public Boolean IsActive { get; set; }

        [XmlElement("EnemyId")]
        public String EnemyId { get; set; }

        [XmlElement("PositionX")]
        public String PositionX { get; set; }

        [XmlElement("PositionY")]
        public String PositionY { get; set; }

        [XmlElement("PositionAir")]
        public String PositionAir { get; set; }

        [XmlElement("PositionGround")]
        public String PositionGround { get; set; }

        [XmlElement("FaceRight")]
        public Boolean FaceRight { get; set; }
    }

    [Serializable]
    [XmlRoot("EnemySet")]
    public class EnemySet : IEnumerable<Enemy>
    {
        [XmlArrayItem("Enemy", typeof(Enemy))]
        public List<Enemy> Enemies { get; set; } = new List<Enemy>();

        public IEnumerator<Enemy> GetEnumerator()
        {
            return ((IEnumerable<Enemy>)this.Enemies).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.Enemies.GetEnumerator();
        }

        public void Add(Enemy in_Element)
        {
            this.Enemies.Add(in_Element);
        }
    }
}
