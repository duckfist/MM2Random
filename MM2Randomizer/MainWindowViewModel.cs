namespace MM2Randomizer
{
    public class MainWindowViewModel
    {
        /// <summary>
        /// Use ROM Rockman 2 (J).nes if true, Mega Man 2 (U).nes if false.
        /// </summary>
        public bool IsJapanese { get; set; }

        /// <summary>
        /// If True, the Robot Master stages will be shuffled and will not be indicated by the
        /// portraits on the Stage Select screen.
        /// </summary>
        public bool Is8StagesRandom { get; set; }

        /// <summary>
        /// If True, the weapons awarded from each Robot Master is shuffled.
        /// </summary>
        public bool IsWeaponsRandom { get; set; }

        /// <summary>
        /// If True, Items 1, 2, and 3 will be awarded from random Robot Masters.
        /// </summary>
        public bool IsItemsRandom { get; set; }

        /// <summary>
        /// If true, in Wily 5, the Robot Master locations in each teleporter is randomized.
        /// </summary>
        public bool IsTeleportersRandom { get; set; }

        /// <summary>
        /// If True, the damage each weapon does against each Robot Master is changed. The manner in
        /// which it is changed depends on if IsWeaknessEasy is True or if IsWeaknessHard is True.
        /// </summary>
        public bool IsWeaknessRandom { get; set; }

        /// <summary>
        /// If True, and if IsWeaknessRandom is True, the damage tables for each weapon are shuffled.
        /// </summary>
        public bool IsWeaknessEasy { get; set; }

        /// <summary>
        /// NOT IMPLEMENTED
        /// If True, and if IsWeaknessRandom is True, the damage tables for each weapon are filled
        /// with random values, within practical tolerances for each weapon.
        /// </summary>
        public bool IsWeaknessHard { get; set; }

        /// <summary>
        /// TODO
        /// </summary>
        public bool IsColorsRandom { get;  set; }
    }
}
