using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


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
            };

            DataContext = ViewModel;
            RandomMM2.Settings = ViewModel;
        }

        private void btnCreateROM_Click(object sender, RoutedEventArgs e)
        {
            int seed;
            if (Int32.TryParse(tbxSeed.Text, out seed) && seed >= 0)
            {
                RandomMM2.Seed = seed;
            }
            else
            {
                RandomMM2.Seed = -1;
            }
            
            RandomMM2.Randomize();
            tbxSeed.Text = String.Format("{0}", RandomMM2.Seed);
        }
    }
}
