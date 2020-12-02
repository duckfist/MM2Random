using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using MM2Randomizer;
using Avalonia.Input;
using System.Linq;

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
            ComboBox comboBoxPlayerSprite = this.Find<ComboBox>("ComboBox_PlayerSprite");
            comboBoxPlayerSprite.Items = Enum.GetValues(typeof(PlayerSprite));
            comboBoxPlayerSprite.SelectedIndex = 0;

            // Set up drag and drop for the rom file path text box
            TextBox textBoxRomFile = this.Find<TextBox>("TextBox_RomFile");
            DragDrop.SetAllowDrop(textBoxRomFile, true);
            textBoxRomFile.AddHandler(DragDrop.DragOverEvent, this.DragOver);
            textBoxRomFile.AddHandler(DragDrop.DropEvent, this.Drop);
        }


        private void DragOver(Object sender, DragEventArgs in_DragEventArgs)
        {
            // Only allow Copy or Link as Drop Operations.

            // Only allow if the dragged data contains text or filenames.
            if (true == in_DragEventArgs.Data.Contains(DataFormats.Text) ||
                true == in_DragEventArgs.Data.Contains(DataFormats.FileNames))
            {
                in_DragEventArgs.DragEffects = in_DragEventArgs.DragEffects & (DragDropEffects.Copy | DragDropEffects.Link);
            }
            else
            {
                in_DragEventArgs.DragEffects = DragDropEffects.None;
            }
        }


        private void Drop(Object in_Sender, DragEventArgs in_DragEventArgs)
        {
            TextBox romFile = in_Sender as TextBox;

            if (true == in_DragEventArgs.Data.Contains(DataFormats.Text))
            {
                romFile.Text = in_DragEventArgs.Data.GetText();
            }
            else if (true == in_DragEventArgs.Data.Contains(DataFormats.FileNames))
            {
                romFile.Text = in_DragEventArgs.Data.GetFileNames().First();
            }
        }
    }
}
