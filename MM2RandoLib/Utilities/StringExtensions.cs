using System;
using System.IO;
using System.Xml.Serialization;

namespace MM2Randomizer.Utilities
{
    public static class StringExtensions
    {
        public static T Deserialize<T>(this String in_Resource)
        {
            T returnValue;

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));

            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (StreamWriter streamWriter = new StreamWriter(memoryStream))
                {
                    streamWriter.Write(in_Resource);
                    streamWriter.Flush();
                    memoryStream.Position = 0;

                    returnValue = (T)xmlSerializer.Deserialize(memoryStream);

                    streamWriter.Close();
                }
            }

            return returnValue;
        }
    }
}
