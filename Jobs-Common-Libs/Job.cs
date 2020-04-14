using System;
using System.IO;
using System.Threading;

namespace Jobs_Common_Libs
{
    public abstract class Job
    {
        public static string ProcessName = System.Diagnostics.Process.GetCurrentProcess().ProcessName;
        private static ReaderWriterLockSlim _readWriteLock = new ReaderWriterLockSlim();

        public static void WriteLine(string content)
        {
            Log(ProcessName, content);
            Console.WriteLine(content);
        }

        public static void Log(string name, string content)
        {
            _readWriteLock.EnterWriteLock();
            var directory = Directory.GetCurrentDirectory();
            try
            {
                File.AppendAllLines(Path.Combine(directory, name + ".log"), new string[] { Environment.NewLine, DateTime.Now.ToString(), content });
            }
            finally
            {
                // Release lock
                _readWriteLock.ExitWriteLock();
            }
        }
    }
}
