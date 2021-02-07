using System;

namespace MM2Randomizer.Randomizers
{
    public class WeaponName
    {
        //
        // Constructors
        //

        public WeaponName(String in_Name, String in_ExtendedName, String in_FullName, Char in_WeaponLetter)
        {
            this.mName = in_Name ?? throw new ArgumentNullException(nameof(in_Name));
            this.mFullName = in_FullName ?? throw new ArgumentNullException(nameof(in_ExtendedName));
            this.mExtendedName = in_ExtendedName;
            this.mLetter = in_WeaponLetter;
        }


        //
        // Properties
        //

        public String Name
        {
            get
            {
                return this.mName;
            }
        }

        public String ExtendedName
        {
            get
            {
                return this.mExtendedName;
            }
        }

        public String FullName
        {
            get
            {
                return this.mFullName;
            }
        }

        public Char Letter
        {
            get
            {
                return this.mLetter;
            }
        }


        //
        // Private Data Members
        //

        private String mName;
        private String mExtendedName;
        private String mFullName;
        private Char mLetter;
    }
}
