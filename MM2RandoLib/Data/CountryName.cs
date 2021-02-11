using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using MM2Randomizer.Extensions;

namespace MM2Randomizer.Data
{
    [Serializable]
    public class CountryName
    {
        [XmlElement("Enabled")]
        public Boolean Enabled { get; set; }

        [XmlElement("Prefix")]
        public String Prefix { get; set; } = "in";

        [XmlElement("Name")]
        public String Name { get; set; }

        public Byte[] GetFormattedPrefix()
        {
            String prefix = this.Prefix ?? String.Empty;

            // Truncate the prefix to the max length
            String truncatedPrefix = prefix.Substring(0, Math.Min(CountryName.MAX_PREFIX_LENGTH, prefix.Length));
            return truncatedPrefix.PadRight(CountryName.MAX_PREFIX_LENGTH).AsIntroString();
        }

        public Byte[] GetFormattedName()
        {
            String name = this.Name ?? String.Empty;

            // Truncate the prefix to the max length
            String truncatedName = name.Substring(0, Math.Min(CountryName.MAX_NAME_LENGTH, name.Length));
            return truncatedName.PadCenter(CountryName.MAX_NAME_LENGTH).AsIntroString();
        }


        private const Int32 MAX_PREFIX_LENGTH = 3;
        private const Int32 MAX_NAME_LENGTH = 25;
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
