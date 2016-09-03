namespace MM2Randomizer.Patcher
{
    public class ChangeByteRecord
    {
        public int Address { get; set; }
        public byte Value { get; set; }
        public string Note { get; set; }

        public ChangeByteRecord(int address, byte value, string note = "")
        {
            Address = address;
            Value = value;
            Note = note;
        }
    }
}
