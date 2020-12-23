using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MM2Randomizer.Randomizers
{
    [Serializable]
    public class EnemyWeakness
    {
        [XmlElement("Enabled")]
        public Boolean Enabled { get; set; }

        [XmlElement("Name")]
        public String Name { get; set; }

        [XmlElement("Offset")]
        public String Offset { get; set; }

        [XmlElement("Buster")]
        public String Buster { get; set; }

        [XmlElement("Heat")]
        public String Heat { get; set; }

        [XmlElement("Air")]
        public String Air { get; set; }

        [XmlElement("Wood")]
        public String Wood { get; set; }

        [XmlElement("Bubble")]
        public String Bubble { get; set; }

        [XmlElement("Quick")]
        public String Quick { get; set; }

        [XmlElement("Crash")]
        public String Crash { get; set; }

        [XmlElement("Metal")]
        public String Metal { get; set; }
    }

    [Serializable]
    [XmlRoot("EnemyWeaknessSet")]
    public class EnemyWeaknessSet : IEnumerable<EnemyWeakness>
    {
        [XmlArrayItem("EnemyWeakness", typeof(EnemyWeakness))]
        public List<EnemyWeakness> EnemyWeaknesses { get; set; } = new List<EnemyWeakness>();

        public IEnumerator<EnemyWeakness> GetEnumerator()
        {
            return ((IEnumerable<EnemyWeakness>)this.EnemyWeaknesses).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.EnemyWeaknesses.GetEnumerator();
        }

        public void Add(EnemyWeakness in_Element)
        {
            this.EnemyWeaknesses.Add(in_Element);
        }
    }
}
