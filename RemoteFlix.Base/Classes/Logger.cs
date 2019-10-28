using RemoteFlix.Base.Enums;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;

namespace RemoteFlix.Base.Classes
{
    public class Logger
    {
        private static readonly Lazy<Logger> Lazy = new Lazy<Logger>(() => new Logger());

        public static Logger Instance => Lazy.Value;

        readonly ObservableCollection<Log> _Logs;

        public INotifyCollectionChanged Logs { get; }

        private Logger()
        {
            _Logs = new ObservableCollection<Log>();
            Logs = new ReadOnlyObservableCollection<Log>(_Logs);
        }

        public void Log(LogLevel logLevel, string message)
        {
            var log = new Log(logLevel, message);

            lock (this)
            {
                _Logs.Add(log);

                using(var writer = new StreamWriter(Path.Combine(Path.GetTempPath(), "remoteflix.log.txt"), true))
                {
                    writer.WriteLine(log);
                }
            }
        }
    }
}
