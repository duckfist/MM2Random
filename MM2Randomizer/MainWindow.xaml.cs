using System;
using System.Windows;

using MM2Randomizer.Utilities;

namespace MM2Randomizer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainWindowViewModel ViewModel;

        public MainWindow()
        {
            InitializeComponent();

            ViewModel = new MainWindowViewModel()
            {
                IsJapanese = false,
                Is8StagesRandom = true,
                IsWeaponsRandom = true,
                IsItemsRandom = true,
                IsTeleportersRandom = true,
                IsColorsRandom = true,
                IsWeaponBehaviorRandom = true,
                IsWeaknessRandom = true,
                IsWeaknessEasy = true,
                IsWeaknessHard = false,
                IsEnemiesRandom = true,
                IsBGMRandom = true,
                IsWeaponNamesRandom = true,
                FastText = true,
                BurstChaserMode = false
            };

            DataContext = ViewModel;
            RandomMM2.Settings = ViewModel;
        }

        private void btnCreateROM_Click(object sender, RoutedEventArgs e)
        {
            int seed = -1;
            bool useRandomSeed = true;

            // Check if textbox contains a valid seed string
            if (!String.IsNullOrEmpty(tbxSeed.Text))
            {
                try
                {
                    int base10 = SeedConvert.ConvertBase26To10(tbxSeed.Text);
                    seed = base10;
                    useRandomSeed = false;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Exception in parsing Seed. Using random seed. Message:/n" + ex.ToString());
                }
            }

            if (!useRandomSeed)
            {
                // Use the provided seed so that a specific ROM may be generated.
                RandomMM2.Seed = seed;
            }
            else
            {
                // A random seed will be chosen later.
                RandomMM2.Seed = -1;
            }
            
            // Perform randomization based on settings, then generate the ROM.
            RandomMM2.Randomize();
            string seedAlpha = SeedConvert.ConvertBase10To26(RandomMM2.Seed);
            tbxSeed.Text = String.Format("{0}", seedAlpha);
        }

        private void chkJapanese_Checked(object sender, RoutedEventArgs e)
        {
            chkWeaponNames.IsChecked = false;
        }
    }
}
