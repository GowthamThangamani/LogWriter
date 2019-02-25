using System;
using System.IO;
using System.Threading.Tasks;

namespace LogWriter
{
    class Program
    {
        static LogWriter LogWriter;
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var path = new WriteLogInFile(Path.Combine("."));
            LogWriter = new LogWriter(LogLevel.All, path);


            for (int i = 0; i < 100; i++)
            {
                int taskNum = i;
                Task task = new Task(() =>
                {
                    WriteLog("Task " + taskNum);
                });

                task.Start();

            }

            Console.ReadLine();
        }

        static void WriteLog(string Tag, int times = 100000)
        {
            for (int i = 0; i < times; i++)
            {
                LogWriter.WriteLog(LogLevel.Error, "Test mgs", Location: "Program.WriteLog", tag: Tag);
            }
        }

    }
}
