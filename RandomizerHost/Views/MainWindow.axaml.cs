using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using MM2Randomizer;

namespace RandomizerHost.Views
{
    public class MainWindow : Window
    {
        //
        // Constructor
        //

        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }


        //
        // Initialization
        //

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            // Set up the custom sprite ComboBox
            ComboBox cbxPlayerSprite = this.Find<ComboBox>("cbxPlayerSprite");
            cbxPlayerSprite.Items = Enum.GetValues(typeof(PlayerSprite));
            cbxPlayerSprite.SelectedIndex = 0;
        }
    }
}
