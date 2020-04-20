using RemoteFlix.Base;
using RemoteFlix.Base.Classes;
using System;
using System.Drawing;
using System.Net;
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

        private Icon RemoteFlixIcon;
        private Icon RemoteflixAlertIcon;

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

            var startedServer = StartServer();
            CreateTaskBarIcon(startedServer);

            ListenToEvents();
        }

        private void OnExit(object sender, ExitEventArgs args)
        {
            try
            {
                if (RemoteFlixServer.Instance.IsRunning)
                    RemoteFlixServer.Instance.Stop();
            }
            catch (Exception e)
            {
                Logger.Instance.Log(Base.Enums.LogLevel.Error, $"{e.GetType()} while stopping the server. Message: '{e.Message}'");
            }

            try
            {
                if (ApplicationIcon != null)
                {
                    ApplicationIcon.Visible = false;
                    ApplicationIcon.Dispose();
                }
            }
            catch (Exception e)
            {
                Logger.Instance.Log(Base.Enums.LogLevel.Error, $"{e.GetType()} while disposing application icon. Message: '{e.Message}'");
            }
        }

        private void ListenToEvents()
        {
            var thread = new Thread(() =>
            {
                while (EventWaitHandle.WaitOne())
                {
                    Current.Dispatcher.BeginInvoke(
                        (Action)(() => ((MainWindow)Current.MainWindow).BringToForeground()));
                }
            });

            thread.IsBackground = true;
            thread.Start();
        }

        private bool StartServer()
        {
            try
            {
                RemoteFlixServer.Instance.Start();
                
                return true;
            }
            catch (HttpListenerException e)
            {
                // Running the following command seems to fix this exception
                // netsh http add urlacl url="http://+:50505/" user=[username]
                Logger.Instance.Log(Base.Enums.LogLevel.Error, $"{e.GetType()} while starting the server. Try running 'netsh http add urlacl url=\"http://+:{RemoteFlixServer.PORT}/\" user={Environment.UserName}' from an elevated command prompt.");
            }
            catch (Exception e)
            {
                Logger.Instance.Log(Base.Enums.LogLevel.Error, $"{e.GetType()} while starting the server. Message: '{e.Message}'");
            }

            return false;
        }

        private void CreateTaskBarIcon(bool startedServer)
        {
            using (var stream = GetResourceStream(new Uri("pack://application:,,,/RemoteFlix.UI.Desktop;component/Resources/remoteflix.ico")).Stream)
            {
                RemoteFlixIcon = new Icon(stream);
            }

            using (var stream = GetResourceStream(new Uri("pack://application:,,,/RemoteFlix.UI.Desktop;component/Resources/remoteflix_alert.ico")).Stream)
            {
                RemoteflixAlertIcon = new Icon(stream);
            }

            ApplicationIcon = new NotifyIcon
            {
                Icon = startedServer ? RemoteFlixIcon : RemoteflixAlertIcon,
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
                Current.Shutdown();
            }));
        }
    }
}
