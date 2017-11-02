using System;

namespace MM2Randomizer.Utilities
{
    public sealed class TitleChars
    {
        public byte ID
        {
            get; private set;
        }
        public char Value
        {
            get; private set;
        }
        
        public static readonly TitleChars NUM_0 = new TitleChars(0xA0, '0');
        public static readonly TitleChars NUM_1 = new TitleChars(0xA7, '1');
        public static readonly TitleChars NUM_2 = new TitleChars(0xA2, '2');
        public static readonly TitleChars NUM_3 = new TitleChars(0xA8, '3');
        public static readonly TitleChars NUM_4 = new TitleChars(0xA9, '4');
        public static readonly TitleChars NUM_5 = new TitleChars(0xAA, '5');
        public static readonly TitleChars NUM_6 = new TitleChars(0xAB, '6');
        public static readonly TitleChars NUM_7 = new TitleChars(0xA6, '7');
        public static readonly TitleChars NUM_8 = new TitleChars(0xA1, '8');
        public static readonly TitleChars NUM_9 = new TitleChars(0xA5, '9');
        public static readonly TitleChars PERIOD = new TitleChars(0xDC, '.');
        public static readonly TitleChars SPACE = new TitleChars(0x00, ' ');

        private TitleChars(byte patternID, char character)
        {
            this.ID = patternID;
            this.Value = character;
        }

        public override String ToString()
        {
            return Value.ToString();
        }

        public static TitleChars GetChar(char c)
        {
            switch (c)
            {
                case '0':
                    return TitleChars.NUM_0;
                case '1':
                    return TitleChars.NUM_1;
                case '2':
                    return TitleChars.NUM_2;
                case '3':
                    return TitleChars.NUM_3;
                case '4':
                    return TitleChars.NUM_4;
                case '5':
                    return TitleChars.NUM_5;
                case '6':
                    return TitleChars.NUM_6;
                case '7':
                    return TitleChars.NUM_7;
                case '8':
                    return TitleChars.NUM_8;
                case '9':
                    return TitleChars.NUM_9;
                case '.':
                    return TitleChars.PERIOD;
                default:
                    return TitleChars.SPACE;
            }
        }
    }
}
