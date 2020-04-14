using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Process_Runner
{
    class Program
    {
        static int PollingDuration;

        static List<ProcessTasks> processes = new List<ProcessTasks>();

        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
            PollingDuration = args.Length == 1 && int.TryParse(args[0], out var polling) && polling >= 3000 ? polling : 10000;
            Process();
        }

        private static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            CurrentDomain_ProcessExit(sender, e);
            e.Cancel = false;
        }

        private static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            processes.ForEach(p => p.Kill(true));
        }

        static void Process()
        {
            while (true)
            {
                GetProcesses().Select(m => new ProcessTasks(m)).ToList().ForEach(p =>
                {
                    if (processes.Find(m => m.Name == p.Name) == null)
                        processes.Add(p);
                });

                Console.WriteLine($"Process Count - {processes.Count}");
                Parallel.ForEach(processes, p =>
                {
                    var _p = (ProcessTasks)p;
                    if (File.Exists(_p.StartInfo.FileName))
                    {
                        Console.WriteLine();
                        Console.WriteLine(DateTime.Now.ToString());
                        Console.Write($"Process - {_p.Name}");

                        if (_p.IsDead && _p.ExecutionCounter == 0)
                        {
                            if (_p.Settings != null)
                            {
                                // will work on the schedule part later
                            }
                            _p.Start();
                            Console.WriteLine($" Started with PID {_p.PID}");
                        }
                        else
                        {
                            Console.WriteLine($" Already Running with PID {_p.PID}");
                        }
                    }
                    else
                        processes.RemoveAll(m => m.Name == _p.Name);
                });
                Thread.Sleep(PollingDuration);
            }
        }

        public static IEnumerable<ProcessStartInfo> GetProcesses()
        {
            var directory = Path.Combine(Directory.GetCurrentDirectory(), "Jobs");
            Directory.CreateDirectory(directory);
            var processes = Directory.GetFiles(directory, "*.exe", SearchOption.AllDirectories).ToList();
            processes.AddRange(Directory.GetFiles(directory, "*.lnk", SearchOption.AllDirectories));

            return processes.Select(m => new ProcessStartInfo
            {
                FileName = m,
                UseShellExecute = true,
                RedirectStandardOutput = false,
                WindowStyle = ProcessWindowStyle.Normal
            });
        }
    }
}
