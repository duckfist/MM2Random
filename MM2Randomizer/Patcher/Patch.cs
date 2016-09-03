using System.Collections.Generic;

namespace MM2Randomizer.Patcher
{
    public static class Patch
    {
        public static List<ChangeByteRecord> Bytes = new List<ChangeByteRecord>();

        public static void Add(int address, byte value, string note = "")
        {
            Bytes.Add(new ChangeByteRecord(address, value, note));
        }

        public static void GetString()
        {
            foreach (ChangeByteRecord b in Bytes)
            {
                System.Diagnostics.Debug.WriteLine(
                    "0x{0:X6}\t{1:X2}\t{2}",
                    b.Address,
                    b.Value,
                    b.Note);
            }
        }
    }
}
