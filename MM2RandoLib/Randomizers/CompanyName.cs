using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MM2Randomizer.Randomizers
{
    [Serializable]
    public class CompanyName
    {
        [XmlElement("Enabled")]
        public Boolean Enabled { get; set; }

        [XmlElement("Name")]
        public String Name { get; set; }
    }

    [Serializable]
    [XmlRoot("CompanyNameSet")]
    public class CompanyNameSet : IEnumerable<CompanyName>
    {
        [XmlArrayItem("CompanyName", typeof(CompanyName))]
        public List<CompanyName> CompanyNames { get; set; } = new List<CompanyName>();

        public IEnumerator<CompanyName> GetEnumerator()
        {
            return ((IEnumerable<CompanyName>)this.CompanyNames).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.CompanyNames.GetEnumerator();
        }

        public void Add(CompanyName in_Element)
        {
            this.CompanyNames.Add(in_Element);
        }
    }
}
