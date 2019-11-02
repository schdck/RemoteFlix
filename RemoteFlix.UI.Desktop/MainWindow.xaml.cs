using MahApps.Metro.Controls;
using RemoteFlix.UI.Desktop.ViewModel;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Controls;
using System.Windows.Forms;

namespace RemoteFlix.UI.Desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private bool AutoScroll = true;

        public MainWindow()
        {
            Icon icon;

            using (var stream = System.Windows.Application.GetResourceStream(new Uri("pack://application:,,,/RemoteFlix.UI.Desktop;component/Resources/remoteflix.ico")).Stream)
            {
                icon = new Icon(stream);
            }

            var systemTrayIcon = new NotifyIcon
            {
                Icon = icon,
                Visible = true,
                ContextMenu = new System.Windows.Forms.ContextMenu()
            };

            systemTrayIcon.DoubleClick += (s, e) =>
            {
                Show();
            };

            systemTrayIcon.ContextMenu.MenuItems.Add(new System.Windows.Forms.MenuItem("Show", (s, e) =>
            {
                Show();
            }));

            systemTrayIcon.ContextMenu.MenuItems.Add(new System.Windows.Forms.MenuItem("Exit", (s, e) =>
            {
                var viewModel = DataContext as MainViewModel;

                viewModel.ApplicationShuttingDownCommand.Execute(null);

                System.Windows.Application.Current.Shutdown();
            }));

            Hide();

            InitializeComponent();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;

            Hide();

            base.OnClosing(e);
        }

        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            var scroll = e.Source as ScrollViewer;

            if (e.ExtentHeightChange == 0)
            {
                if (scroll.VerticalOffset == scroll.ScrollableHeight)
                {
                    AutoScroll = true;
                }
                else
                {
                    AutoScroll = false;
                }
            }

            if (AutoScroll && e.ExtentHeightChange != 0)
            {
                scroll.ScrollToVerticalOffset(scroll.ExtentHeight);
            }
        }
    }
}
