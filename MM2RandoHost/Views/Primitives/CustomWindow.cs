using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MM2RandoHost.Views.Primitives
{
    public class CustomWindow : Window
    {
        public Action OnMaximizeOrRestore { get; set; }

        protected void TitleBarClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                if (WindowState == WindowState.Normal)
                    WindowMaximize();
                else
                    WindowRestore();
            }
        }

        protected void MinimizeClick(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        protected void RestoreClick(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Normal)
                WindowMaximize();
            else
                WindowRestore();
        }

        protected void CloseClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        public override void OnApplyTemplate()
        {
            System.Windows.Shapes.Rectangle hitArea = GetTemplateChild("windowTitleBarClickRegion") as System.Windows.Shapes.Rectangle;
            if (hitArea != null)
                hitArea.MouseLeftButtonDown += TitleBarClick;

            Button restoreButton = GetTemplateChild("restoreButton") as Button;
            if (restoreButton != null)
                restoreButton.Click += RestoreClick;

            Button closeButton = GetTemplateChild("closeButton") as Button;
            if (closeButton != null)
                closeButton.Click += CloseClick;

            base.OnApplyTemplate();
        }

        public void WindowMaximize()
        {
            WindowState = WindowState.Maximized;
            if (OnMaximizeOrRestore != null) OnMaximizeOrRestore.Invoke();
        }
        public void WindowRestore()
        {
            WindowState = WindowState.Normal;
            if (OnMaximizeOrRestore != null) OnMaximizeOrRestore.Invoke();
        }
    }
}
