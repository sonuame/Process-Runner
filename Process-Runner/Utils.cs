using System;
using System.IO;
using System.Threading;

namespace Process_Runner
{
    class Utils
    {
        private static ReaderWriterLockSlim _readWriteLock = new ReaderWriterLockSlim();

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

        public static void LogToPath(string path, string content)
        {
            _readWriteLock.EnterWriteLock();
            var directory = Directory.GetCurrentDirectory();
            try
            {
                File.AppendAllLines(path, new string[] { content });
            }
            finally
            {
                // Release lock
                _readWriteLock.ExitWriteLock();
            }
        }
    }
}
