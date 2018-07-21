﻿using System;
using System.Windows;
using System.Diagnostics;
using System.IO;
using System.Windows.Media;
using System.Windows.Input;
using Microsoft.Win32;

using MM2Randomizer;
using MM2Randomizer.Utilities;

namespace MM2RandoHost
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        RandoSettings ViewModel;

        public MainWindow()
        {
            InitializeComponent();

            ViewModel = new RandoSettings();
            DataContext = ViewModel;
            RandomMM2.Settings = ViewModel;
        }

        private void btnCreateROM_Click(object sender, RoutedEventArgs e)
        {
            int seed = -1;
            // Check if textbox contains a valid seed string
            if (!String.IsNullOrEmpty(tbxSeed.Text))
            {
                try
                {
                    int base10 = SeedConvert.ConvertBase26To10(tbxSeed.Text);
                    seed = base10;

                    // Use the provided seed so that a specific ROM may be generated.
                    RandomMM2.Seed = seed;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Exception in parsing Seed. Using random seed. Message:/n" + ex.ToString());
                    seed = -1;
                }
            } else //if no text entered, use random seed
            {
                RandomMM2.Seed = -1;
            }

            // Perform randomization based on settings, then generate the ROM.
            RandomMM2.RandomizerCreate(true);
            UpdateSeedString(e);
        }

        private void UpdateSeedString(RoutedEventArgs e)
        {
            string seedAlpha = SeedConvert.ConvertBase10To26(RandomMM2.Seed);
            ViewModel.SeedString = String.Format("{0}", seedAlpha);
            Debug.WriteLine("\nSeed: " + seedAlpha + "\n");

            // Create log file if left shift is pressed while clicking
            if (Keyboard.IsKeyDown(Key.LeftShift))
            {
                string logFileName = (ViewModel.IsJapanese) ? "RM2" : "MM2";
                logFileName = String.Format("{0}-RNG-{1}.log", logFileName, seedAlpha);
                using (StreamWriter sw = new StreamWriter(logFileName, false))
                {
                    sw.WriteLine("Mega Man 2 Randomizer");
                    sw.WriteLine(String.Format("Version {0}", ViewModel.AssemblyVersion.ToString()));
                    sw.WriteLine(String.Format("Seed {0}\n", seedAlpha));
                    sw.WriteLine(RandomMM2.randomStages.ToString());
                    sw.WriteLine(RandomMM2.randomWeaponBehavior.ToString());
                    sw.WriteLine(RandomMM2.randomEnemyWeakness.ToString());
                    sw.WriteLine(RandomMM2.randomWeaknesses.ToString());
                    sw.Write(RandomMM2.Patch.GetStringSortedByAddress());
                }
            }
        }

        private void chkJapanese_Checked(object sender, RoutedEventArgs e)
        {
            chkWeaponNames.IsChecked = false;
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
                trySetSourcePath(dlg.FileName);
            }
        }
        
        private void Window_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                trySetSourcePath(files[0]);
            }
            BorderShowHandler(false, e);
        }

        private void TextBox_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                trySetSourcePath(files[0]);
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

        private void trySetSourcePath(string path)
        {
            // TODO error handling
            ViewModel.SourcePath = path;

            tbxSource.Focus();
            tbxSource.SelectionStart = tbxSource.Text.Length;
        }

        private void chkBurstChaser_Checked(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Caution: The randomizer is not balanced for Burst Chaser mode.\nIf it is your first time playing, consider using the default settings.");
        }
    }
}
