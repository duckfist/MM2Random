using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.VisualTree;
using MM2Randomizer;
using MM2Randomizer.Utilities;
using RandomizerHost.ViewModels;
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
            KeyboardDevice.Instance.PropertyChanged += this.Instance_PropertyChanged;

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

            ComboBox cbxPlayerSprite = this.Find<ComboBox>("cbxPlayerSprite");

            cbxPlayerSprite.Items = Enum.GetValues(typeof(PlayerSprite));
            cbxPlayerSprite.SelectedIndex = 0;
        }


        //
        // Events
        //

        public async void BrowseButton_OnClick(Object in_Sender, RoutedEventArgs in_Args)
        {
            if (in_Sender is Button button && button.GetVisualRoot() is Window window)
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();

                openFileDialog.AllowMultiple = false;

                openFileDialog.Filters.Add(
                    new FileDialogFilter()
                    {
                        Name = @"ROM Image",
                        Extensions = new List<String>()
                        {
                            @"nes"
                        }
                    });

                openFileDialog.Title = @"Open Mega Man 2 (U) NES ROM File";

                // Call the ShowDialog method to show the dialog box.
                String exePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
                String exeDir = Path.GetDirectoryName(exePath);
                openFileDialog.Directory = exeDir;

                String[] dialogResult = await openFileDialog.ShowAsync(window);

                // Process input if the user clicked OK.
                if (dialogResult.Length > 0)
                {
                    //TryFile(dlg.FileName);
                    //SetTextBoxFocusToEnd();
                }
            }
            else
            {
                throw new Exception();
            }
        }

        public void CreateRomButton_OnClick(Object in_Sender, RoutedEventArgs in_Args)
        {
            int seed = -1;

            /*
            // Check if textbox contains a valid seed string
            if (false == String.IsNullOrEmpty(this.SeedTextBox.Text))
            {
                try
                {
                    // Use the provided seed so that a specific ROM may be generated.
                    seed = SeedConvert.ConvertBase26To10(this.SeedTextBox.Text);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Exception in parsing Seed. Using random seed. Message:/n" + ex.ToString());
                    seed = -1;
                }
            }
            */

            // Perform randomization based on settings, then generate the ROM.
            //MainWindowViewModel.PerformRandomization(seed, Keyboard.IsKeyDown(Key.LeftShift));
        }

        private void Instance_PropertyChanged(Object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void CreateRandomButton_OnClick(Object in_Sender, RoutedEventArgs in_Args)
        {
            //MainWindowViewModel.PerformRandomization(-1, Keyboard.IsKeyDown(Key.LeftShift));
        }

        public void OpenFolderButton_OnClick(Object in_Sender, RoutedEventArgs in_Args)
        {
            if (RandomMM2.RecentlyCreatedFileName != "")
            {
                try
                {
                    Process.Start("explorer.exe", string.Format("/select,\"{0}\"", RandomMM2.RecentlyCreatedFileName));
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                    Process.Start("explorer.exe", string.Format("/select,\"{0}\"", System.Reflection.Assembly.GetExecutingAssembly().Location));
                }
            }
            else
            {
                Process.Start("explorer.exe", string.Format("/select,\"{0}\"", System.Reflection.Assembly.GetExecutingAssembly().Location));
            }
        }

        public void chkBurstChaser_Checked(Object in_Sender, RoutedEventArgs in_Args)
        {
        }
    }
}
