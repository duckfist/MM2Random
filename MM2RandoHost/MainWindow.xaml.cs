using System;
using System.Windows;
using System.Diagnostics;
using System.IO;
using System.Windows.Media;
using System.Windows.Input;
using Microsoft.Win32;

using MM2Randomizer;
using MM2Randomizer.Utilities;

using MM2RandoHost.ViewModels;
using MM2RandoHost.Views.Primitives;

namespace MM2RandoHost
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : CustomWindow
    {
        // TODO Remove
        MainWindowViewModel ViewModel;

        public MainWindow()
        {
            InitializeComponent();

            // TODO Remove
            ViewModel = DataContext as MainWindowViewModel;
        }

        private void btnCreateROM_Click(object sender, RoutedEventArgs e)
        {
            int seed = -1;

            // Check if textbox contains a valid seed string
            if (!String.IsNullOrEmpty(tbxSeed.Text))
            {
                try
                {
                    // Use the provided seed so that a specific ROM may be generated.
                    seed = SeedConvert.ConvertBase26To10(tbxSeed.Text);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Exception in parsing Seed. Using random seed. Message:/n" + ex.ToString());
                    seed = -1;
                }
            }

            // Perform randomization based on settings, then generate the ROM.
            ViewModel.PerformRandomization(seed, Keyboard.IsKeyDown(Key.LeftShift));
        }

        private void btnCreateRandom_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.PerformRandomization(-1, Keyboard.IsKeyDown(Key.LeftShift));
        }

        private void btnOpenFolder_Click(object sender, RoutedEventArgs e)
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

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "ROM image (.nes)|*.nes";
            dlg.Title = "Open Mega Man 2 (U) NES ROM File";

            // Call the ShowDialog method to show the dialog box.
            string exePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string exeDir = Path.GetDirectoryName(exePath);
            dlg.InitialDirectory = exeDir;

            bool? userClickedOK = dlg.ShowDialog();

            // Process input if the user clicked OK.
            if (userClickedOK == true)
            {
                TryFile(dlg.FileName);
                SetTextBoxFocusToEnd();
            }
        }

        private void tbxSource_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (ViewModel != null)
                TryFile(ViewModel.RandoSettings.SourcePath);
        }

        private void Window_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                TryFile(files[0]);
                SetTextBoxFocusToEnd();
            }
            BorderShowHandler(false, e);
        }

        private void TextBox_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                TryFile(files[0]);
                SetTextBoxFocusToEnd();
            }
            BorderShowHandler(false, e);
        }

        private void Window_DragEnter(object sender, DragEventArgs e)
        {
            BorderShowHandler(true, e);
        }
        
        private void Window_DragLeave(object sender, DragEventArgs e)
        {
            BorderShowHandler(false, e);
        }

        private void TextBox_PreviewDragOver(object sender, DragEventArgs e)
        {
            e.Handled = true;
            BorderShowHandler(true, e);
        }

        private void BorderShowHandler(bool show, DragEventArgs e)
        {
            if (!show)
            {
                border.BorderBrush = new SolidColorBrush(Colors.Transparent);
                return;
            }

            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                border.BorderBrush = new SolidColorBrush(Colors.Red);
            }
            else
            {
                new SolidColorBrush(Colors.Transparent);
            }
        }

        private void SetTextBoxFocusToEnd()
        {
            tbxSource.Focus();
            tbxSource.SelectionStart = tbxSource.Text.Length;
        }

        private void TryFile(string filename)
        {
            ViewModel.IsShowingHint = false;
            ViewModel.RandoSettings.ValidateFile(filename);
        }

        private void chkBurstChaser_Checked(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Caution: The randomizer is not balanced for Burst Chaser mode.\nIf it is your first time playing, consider using the default settings.");
        }

        private void CustonWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == System.Windows.Input.MouseButton.Left)
                this.DragMove();
        }
    }
}
