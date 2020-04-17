using System;
using System.Collections.Generic;
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
                processes.RemoveAll(m => m.IsDead);

                GetProcesses().ForEach(p =>
                {
                    var _p = processes.Find(m => m.Name == p.Name);
                    if (_p == null) processes.Add(p);
                });

                Console.WriteLine($"Process Count - {processes.Count}");
                Parallel.ForEach<ProcessTasks>(processes, p =>
                {
                    Task.Run(() =>
                    {
                        if (p.IsDead && p.ExecutionCounter == 0)
                        {
                            if (p.Settings != null)
                            {
                                // will work on the schedule part later
                            }
                            p.Start();
                            p.BeginOutputReadLine();
                            p.BeginErrorReadLine();
                            Console.WriteLine($"{DateTime.Now}\t{p.Name} Started with PID {p.PID}");
                        }
                        else
                        {
                            Console.WriteLine($"{DateTime.Now}\t{p.Name} Already Running with PID {p.PID}");
                        }
                    });

                });
                Thread.Sleep(PollingDuration);
            }
        }

        public static List<ProcessTasks> GetProcesses()
        {
            var directory = Path.Combine(Directory.GetCurrentDirectory(), "Jobs");
            Directory.CreateDirectory(directory);
            var _processes = new List<ProcessTasks>();

            Directory
                .EnumerateDirectories(directory, "*.*", SearchOption.TopDirectoryOnly)
                .ToList()
                .ForEach(dir =>
            {
                var proc = Path.Combine(dir, (new DirectoryInfo(dir)).Name + ".json");
                _processes.Add(new ProcessTasks(proc));
            });

            return _processes;
        }
    }
}
