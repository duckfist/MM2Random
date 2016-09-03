using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MM2Randomizer.Patcher
{
    public static class Patch
    {
        public static Dictionary<int, ChangeByteRecord> Bytes = new Dictionary<int, ChangeByteRecord>();

        public static void Add(int address, byte value, string note = "")
        {
            ChangeByteRecord newByte = new ChangeByteRecord(address, value, note);

            // Either replace the byte at the given address, or add it if it doesn't exist
            if (Bytes.ContainsKey(address))
            {
                Bytes[address] = newByte;
            }
            else
            {
                Bytes.Add(address, newByte);
            }
        }

        public static void ApplyPatch(FileStream stream)
        {
            foreach (KeyValuePair<int, ChangeByteRecord> kvp in Bytes)
            {
                stream.Position = kvp.Key;
                stream.WriteByte(kvp.Value.Value);
            }
        }

        public static void GetStringSortedByAddress()
        {
            var sortDict = from kvp in Bytes orderby kvp.Key ascending select kvp;
            foreach (KeyValuePair<int, ChangeByteRecord> kvp in sortDict)
            {
                ChangeByteRecord b = kvp.Value;
                System.Diagnostics.Debug.WriteLine(
                    "0x{0:X6}\t{1:X2}\t{2}",
                    b.Address,
                    b.Value,
                    b.Note);
            }
        }

        public static void GetString()
        {
            foreach (KeyValuePair<int, ChangeByteRecord> kvp in Bytes)
            {
                ChangeByteRecord b = kvp.Value;
                System.Diagnostics.Debug.WriteLine(
                    "0x{0:X6}\t{1:X2}\t{2}",
                    b.Address,
                    b.Value,
                    b.Note);
            }
        }
    }
}
