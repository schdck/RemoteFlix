using MahApps.Metro.Controls;
using System.ComponentModel;
using System.Windows.Controls;

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
            // Hide();

            InitializeComponent();
        }

        public void BringToForeground()
        {
            if (WindowState == System.Windows.WindowState.Minimized || Visibility == System.Windows.Visibility.Hidden)
            {
                Show();
                WindowState = System.Windows.WindowState.Normal;
            }

            Activate();
            Topmost = true;
            Topmost = false;
            Focus();
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
