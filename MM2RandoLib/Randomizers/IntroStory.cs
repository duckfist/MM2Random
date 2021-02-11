using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Text;
using MM2Randomizer.Extensions;

namespace MM2Randomizer.Randomizers
{
    [Serializable]
    public class IntroStory
    {
        [XmlElement("Enabled")]
        public Boolean Enabled { get; set; }

        [XmlElement("Title")]
        public String Title { get; set; }

        [XmlArrayItem("Line", typeof(String))]
        public List<String> Lines { get; set; }

        public Byte[] GetFormattedString()
        {
            StringBuilder sb = new StringBuilder();

            Int32 currentLine = 0;

            // Add lines from the config Object
            if (null != this.Lines)
            {
                foreach (String line in this.Lines)
                {
                    // If the number of lines exceeds the max line length,
                    // stop processing lines
                    if (currentLine++ >= IntroStory.MAX_LINES)
                    {
                        break;
                    }

                    // Truncate each line to the max line length per line
                    String truncatedLine = line.Substring(0, Math.Min(IntroStory.MAX_LINE_LENGTH, line.Length));
                    sb.Append(truncatedLine.PadRight(IntroStory.MAX_LINE_LENGTH));
                }
            }

            // Fill in the rest of the lines if there were less than 10
            for (Int32 fillLine = currentLine; fillLine < IntroStory.MAX_LINES; ++fillLine)
            {
                sb.Append(' ', IntroStory.MAX_LINE_LENGTH);
            }

            return sb.ToString().AsIntroString();
        }


        //
        // Constants
        //

        private const Int32 MAX_LINE_LENGTH = 27;
        private const Int32 MAX_LINES = 10;
    }

    [Serializable]
    [XmlRoot("IntroStorySet")]
    public class IntroStorySet : IEnumerable<IntroStory>
    {
        [XmlArrayItem("IntroStory", typeof(IntroStory))]
        public List<IntroStory> IntroStories { get; set; } = new List<IntroStory>();

        public IEnumerator<IntroStory> GetEnumerator()
        {
            return ((IEnumerable<IntroStory>)this.IntroStories).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.IntroStories.GetEnumerator();
        }

        public void Add(IntroStory in_Element)
        {
            this.IntroStories.Add(in_Element);
        }
    }
}
