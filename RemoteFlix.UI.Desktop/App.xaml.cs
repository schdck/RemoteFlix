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
        private Mutex ApplicationMutex;
        private NotifyIcon ApplicationIcon;
        private EventWaitHandle EventWaitHandle;

        private void OnStartup(object sender, StartupEventArgs e)
        {
            ApplicationMutex = new Mutex(true, "RemoteFlix.UI.Desktop.Mutex", out bool createdNew);
            EventWaitHandle = new EventWaitHandle(false, EventResetMode.AutoReset, "RemoteFlix.UI.Desktop.EventWaitHandle");

            GC.KeepAlive(ApplicationMutex);

            if (!createdNew)
            {
                EventWaitHandle.Set();

                Current.Shutdown();

                return;
            }

            CreateTaskBarIcon();

            var thread = new Thread(() =>
            {
                while (EventWaitHandle.WaitOne())
                {
                    Current.Dispatcher.BeginInvoke(
                        (Action)(() => ((MainWindow) Current.MainWindow).BringToForeground()));
                }
            });

            thread.IsBackground = true;
            thread.Start();
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
