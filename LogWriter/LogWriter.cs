using System;
namespace LogWriter
{

    public enum LogLevel
    {
        Info = 0, // Device or user information
        Action = 1, // If user Click or navigate to something
        Debug = 2, // Used to debug application to find error.
        Error = 3, // Error occured in applicatoin
        Console = 4,
        All = 5, // all type of information.
    }
    public class LogWriter
    {

        LogLevel _logLevel { get; }

        public LogWriter(LogLevel logLevel)
        {
            _logLevel = logLevel;
        }

        public
    }
}
