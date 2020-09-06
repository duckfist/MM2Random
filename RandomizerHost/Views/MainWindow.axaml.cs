using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.VisualTree;

namespace RandomizerHost.Views
{
    public class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

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
    }
}
