using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using RemoteFlix.Base;
using RemoteFlix.Base.Classes;
using RemoteFlix.Base.Helpers;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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

            if (IsInDesignMode)
            {
                ServerAddress = $"http://127.0.0.1:{RemoteFlixServer.PORT}";
            }
            else
            {
                Logs = new ObservableCollection<Log>();
                Logger.Instance.Logs.CollectionChanged += LogReceived;

                ServerAddress = $"http://{NetworkHelper.GetLocalIPAddress()}:{RemoteFlixServer.PORT}";

                Server = new RemoteFlixServer();
                Server.Start();
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