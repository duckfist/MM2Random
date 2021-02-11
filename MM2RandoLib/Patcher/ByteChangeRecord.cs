using System;

namespace MM2Randomizer.Patcher
{
    public class ChangeByteRecord
    {
        public Int32 Address { get; set; }
        public Byte Value { get; set; }
        public String Note { get; set; }

        public ChangeByteRecord(Int32 address, Byte value, String note = "")
        {
            Address = address;
            Value = value;
            Note = note;
        }
    }
}
