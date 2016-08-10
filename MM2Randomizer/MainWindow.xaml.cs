using System;
using System.Windows;

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
                IsJapanese = true,
                Is8StagesRandom = false,
                IsWeaponsRandom = true,
                IsItemsRandom = true,
                IsTeleportersRandom = true,
                IsColorsRandom = true,
                IsWeaknessRandom = true,
                IsWeaknessEasy = true,
                IsWeaknessHard = false,
                IsEnemiesRandom = true,
            };

            DataContext = ViewModel;
            RandomMM2.Settings = ViewModel;
        }

        private void btnCreateROM_Click(object sender, RoutedEventArgs e)
        {
            int seed;
            if (Int32.TryParse(tbxSeed.Text, out seed) && seed >= 0)
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
            tbxSeed.Text = String.Format("{0}", RandomMM2.Seed);
        }
    }
}
