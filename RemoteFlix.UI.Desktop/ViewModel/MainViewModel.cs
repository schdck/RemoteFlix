using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using RemoteFlix.Base;
using RemoteFlix.Base.Classes;
using RemoteFlix.Base.Helpers;
using RemoteFlix.UI.Desktop.Helpers;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Net;
using System.Windows;
using System.Windows.Input;

namespace RemoteFlix.UI.Desktop.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public string ServerAddress { get; }
        public string RemoteFlixVersion => "1.0-beta2";
        public ObservableCollection<Log> Logs { get; }

        public ICommand CopyAddressToClipboardCommand { get; }
        public ICommand SetupEnvironmentCommand { get; }
        public ICommand ReportErrorCommand { get; }

        public MainViewModel()
        {
            CopyAddressToClipboardCommand = new RelayCommand(() =>
            {
                Clipboard.SetText(ServerAddress);
            });

            ReportErrorCommand = new RelayCommand(() =>
            {
                Process.Start("https://github.com/schdck/RemoteFlix/issues/new");
            });

            SetupEnvironmentCommand = new RelayCommand(SetupEnvironment);

            if (IsInDesignMode)
            {
                ServerAddress = $"http://127.0.0.1:{RemoteFlixServer.PORT}";
            }
            else
            {
                Logs = new ObservableCollection<Log>(Logger.Instance.Logs);
                ((INotifyCollectionChanged) Logger.Instance.Logs).CollectionChanged += LogReceived;

                ServerAddress = $"http://{NetworkHelper.GetLocalIPAddress()}:{RemoteFlixServer.PORT}";
            }
        }

        private void SetupEnvironment()
        {
            try
            {
                Logger.Instance.Log(Base.Enums.LogLevel.Message, $"Running 'netsh http add urlacl url=\"http://+:{RemoteFlixServer.PORT}/\" sddl=D:(A;;GX;;;S-1-1-0)'");
                var output = CmdHelper.RunAsAdmin($"cmd /c netsh http add urlacl url=\"http://+:{RemoteFlixServer.PORT}/\" sddl=D:(A;;GX;;;S-1-1-0)");
                Logger.Instance.Log(Base.Enums.LogLevel.Message, $"Command prompt threw no exceptions. Output was:\n{output}");
            }
            catch(Exception e)
            {
                Logger.Instance.Log(Base.Enums.LogLevel.Error, $"{e.GetType()} while reserving the URL. Message: '{e.Message}'");
            }
            try
            {
                Logger.Instance.Log(Base.Enums.LogLevel.Message, $"Running 'netsh advfirewall firewall add rule name=\"RemoteFlix_Port\" localport={RemoteFlixServer.PORT} direction=in action=allow protocol=tcp'");
                var output = CmdHelper.RunAsAdmin($"cmd /c netsh advfirewall firewall add rule name=\"RemoteFlix_Port\" localport={RemoteFlixServer.PORT} direction=in action=allow protocol=tcp");
                Logger.Instance.Log(Base.Enums.LogLevel.Message, $"Command prompt threw no exceptions. Output was:\n{output}");
            }
            catch (Exception e)
            {
                Logger.Instance.Log(Base.Enums.LogLevel.Error, $"{e.GetType()} while adding the firewall port. Message: '{e.Message}'");
            }
        }

        private void LogReceived(object sender, NotifyCollectionChangedEventArgs e)
        {
            // It is safe to do this since there's only one method to modify the collection and it 
            // adds *one* log. So every time the collection changes it is an addition of one item.
            var log = e.NewItems[0] as Log;

            Application.Current.Dispatcher.Invoke(() => Logs.Add(log));
        }
    }
}