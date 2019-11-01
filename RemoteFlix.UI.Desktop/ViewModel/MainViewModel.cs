using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using RemoteFlix.Base;
using RemoteFlix.Base.Classes;
using RemoteFlix.Base.Helpers;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Net;
using System.Windows;
using System.Windows.Input;

namespace RemoteFlix.UI.Desktop.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private RemoteFlixServer Server;

        public string ServerAddress { get; }
        public ObservableCollection<Log> Logs { get; }

        public ICommand CopyAddressToClipboardCommand { get; }
        public ICommand ApplicationShuttingDownCommand { get; }
        public ICommand StartServerCommand { get; }

        public MainViewModel()
        {
            CopyAddressToClipboardCommand = new RelayCommand(() =>
            {
                Clipboard.SetText(ServerAddress);
            });

            ApplicationShuttingDownCommand = new RelayCommand(() =>
            {
                Server.Stop();
            });

            StartServerCommand = new RelayCommand(StartServer);

            if (IsInDesignMode)
            {
                ServerAddress = $"http://127.0.0.1:{RemoteFlixServer.PORT}";
            }
            else
            {
                Logs = new ObservableCollection<Log>();
                Logger.Instance.Logs.CollectionChanged += LogReceived;

                ServerAddress = $"http://{NetworkHelper.GetLocalIPAddress()}:{RemoteFlixServer.PORT}";
            }
        }

        private void StartServer()
        {
            try
            {
                Server = new RemoteFlixServer();
                Server.Start();
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
        }

        private void LogReceived(object sender, NotifyCollectionChangedEventArgs e)
        {
            // It is safe to do this since there's only one method to modify the collection and it 
            // adds (one) log. So every time the collection changes it is an addition of one item.
            var log = e.NewItems[0] as Log;

            Application.Current.Dispatcher.Invoke(() => Logs.Add(log));
        }
    }
}