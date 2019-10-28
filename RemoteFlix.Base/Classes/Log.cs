using RemoteFlix.Base.Enums;
using System;

namespace RemoteFlix.Base.Classes
{
    public class Log
    {
        public LogLevel LogLevel { get; }
        public DateTime DateTime { get; set; }
        public string Message { get; }

        public Log(LogLevel logLevel, string message)
        {
            DateTime = DateTime.Now;
            LogLevel = logLevel;
            Message = message;
        }

        public override string ToString()
        {
            return $"[{DateTime}][{LogLevel}] {Message}";
        }
    }
}
