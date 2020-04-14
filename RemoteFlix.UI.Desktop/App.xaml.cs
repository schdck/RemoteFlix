using RemoteFlix.UI.Desktop.ViewModel;
using System;
using System.Drawing;
using System.Threading;
using System.Windows;
using System.Windows.Forms;

namespace RemoteFlix.UI.Desktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        private NotifyIcon ApplicationIcon;

        private void OnStartup(object sender, StartupEventArgs e)
        {
            CreateTaskBarIcon();
        }

        private void CreateTaskBarIcon()
        {
            Icon icon;

            using (var stream = GetResourceStream(new Uri("pack://application:,,,/RemoteFlix.UI.Desktop;component/Resources/remoteflix.ico")).Stream)
            {
                icon = new Icon(stream);
            }

            ApplicationIcon = new NotifyIcon
            {
                Icon = icon,
                Visible = true,
                ContextMenu = new ContextMenu()
            };

            ApplicationIcon.DoubleClick += (s, e) =>
            {
                ((MainWindow)Current.MainWindow).BringToForeground();
            };

            ApplicationIcon.ContextMenu.MenuItems.Add(new MenuItem("Show", (s, e) =>
            {
                ((MainWindow)Current.MainWindow).BringToForeground();
            }));

            ApplicationIcon.ContextMenu.MenuItems.Add(new MenuItem("Exit", (s, e) =>
            {
                var viewModel = ((MainWindow)Current.MainWindow).DataContext as MainViewModel;

                viewModel.ApplicationShuttingDownCommand.Execute(null);

                Current.Shutdown();
            }));
        }

        private void OnExit(object sender, ExitEventArgs e)
        {
            if(ApplicationIcon != null)
            {
                ApplicationIcon.Visible = false;
                ApplicationIcon.Dispose();
            }
        }
    }
}
