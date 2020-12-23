using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MM2Randomizer.Randomizers
{
    [Serializable]
    public class CountryName
    {
        [XmlElement("Enabled")]
        public Boolean Enabled { get; set; }

        [XmlElement("Name")]
        public String Name { get; set; }
    }

    [Serializable]
    [XmlRoot("CountryNameSet")]
    public class CountryNameSet : IEnumerable<CountryName>
    {
        [XmlArrayItem("CountryName", typeof(CompanyName))]
        public List<CountryName> CountryNames { get; set; } = new List<CountryName>();

        public IEnumerator<CountryName> GetEnumerator()
        {
            return ((IEnumerable<CountryName>)this.CountryNames).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.CountryNames.GetEnumerator();
        }

        public void Add(CountryName in_Element)
        {
            this.CountryNames.Add(in_Element);
        }
    }
}
