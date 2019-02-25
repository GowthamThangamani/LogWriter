using System;
using System.IO;
using System.Threading.Tasks;

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

        bool IsToWriteInConsole = true;

        string DateTimeNow { get { return DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffffffK"); } }
        WriteLogInFile _writeLogInFileInstant { get; }
        /// <summary>
        /// The log format.
        /// 0 - DateTime
        /// 1 - LogLevel
        /// 2 - Tag
        /// 3 - Location
        /// 4 - Message
        /// </summary>
        string LogFormat = "[{0}] {1} - {2} - {3} - {4} ";

        public LogWriter(LogLevel logLevelUpto, WriteLogInFile writeLogInFileInstant)
        {
            _logLevel = logLevelUpto;
            _writeLogInFileInstant = writeLogInFileInstant;
        }



        public void WriteLog(LogLevel LogType, string[] message, string tag = "", string Location = "")
        {
            string logBuilded = string.Format(LogFormat,
                                            DateTimeNow,
                                            LogType.ToString(),
                                            tag, Location,
                                            string.Join(Environment.NewLine, message)
                                           );

            Write(logBuilded);
        }

        public void WriteLog(LogLevel LogType, string message, string tag = "", string Location = "")
        {
            string logBuilded = string.Format(LogFormat,
                                            DateTimeNow,
                                            LogType.ToString(),
                                            tag, Location,
                                             message
                                           );

            Write(logBuilded);
        }

        public void WriteLog(LogLevel LogType, Exception ex, string tag = "", string Location = "")
        {
            string logBuilded = string.Format(LogFormat,
                                            DateTimeNow,
                                            LogType.ToString(),
                                            tag, Location,
                                              ex.Message
                                              + Environment.NewLine
                                              + ex.InnerException != null ?
                                              ex.InnerException.Message + Environment.NewLine + ex.InnerException.StackTrace + Environment.NewLine : ""
                                              + ex.StackTrace
                                           );

            Write(logBuilded);
        }

        void Write(string log)
        {
            if (IsToWriteInConsole)
            {
                WriteINConsole(log);
            }

            _writeLogInFileInstant.Write(log);
        }


        void WriteINConsole(string mgs)
        {
            Console.WriteLine(mgs);
        }

    }


    public class WriteLogInFile
    {
        public static string LOCAL_LOG_FIELS_BASE_NAME = "log_";
        public static string LOCAL_LOG_FILE_NAME => DateTime.Now.ToString("yyyy_MM_dd") + ".log";
        public static int NO_OF_FILE_LIMIT = 5;
        string _logFolderPath { get; }

        public WriteLogInFile(string LogFolderPath)
        {
            _logFolderPath = LogFolderPath;
        }

        public void Write(string str)
        {
            try
            {
                var filePath = GettingWriteLogFileName();
                File.AppendAllText(filePath, str + Environment.NewLine);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("+++++++++++++++++++++++");
                System.Diagnostics.Debug.WriteLine("Error in Writing File");
                System.Diagnostics.Debug.WriteLine("+++++++++++++++++++++++");
                System.Diagnostics.Debug.WriteLine(ex);
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }


        string GettingWriteLogFileName()
        {
            if (!Directory.Exists(_logFolderPath))
            {
                Directory.CreateDirectory(_logFolderPath);
            }
            var fileName = LOCAL_LOG_FIELS_BASE_NAME + LOCAL_LOG_FILE_NAME;
            return Path.Combine(_logFolderPath, fileName);
        }

        void CleanUpLogFiles(string document)
        {
            try
            {
                var filesInDirectory = Directory.GetFiles(document);

                if (filesInDirectory.Length > NO_OF_FILE_LIMIT)
                {
                    FileInfo fileToBeDeleted = null;

                    foreach (var file in filesInDirectory)
                    {
                        if (fileToBeDeleted == null)
                        {
                            fileToBeDeleted = new FileInfo(file);
                        }
                        else
                        {
                            var tempFile = new FileInfo(file);
                            if (tempFile.LastWriteTime > fileToBeDeleted.LastWriteTime)
                            {
                                fileToBeDeleted = tempFile;
                            }
                        }

                    }
                    fileToBeDeleted.Delete();
                }
            }
            catch (Exception ex)
            {
                Console.Write("Exception In CleanUpLog");
                Console.Write(ex.Message);
                Console.Write(ex.StackTrace);
            }
        }
    }
}
