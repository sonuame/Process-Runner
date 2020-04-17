using Newtonsoft.Json;
using Process_Runner;
using System;
using System.Diagnostics;
using System.IO;

public class ProcessTasks : Process
{
    public int ExecutionCounter { get; set; }
    private string settingsFile;
    public JobSettings Settings;
    public string Name => new DirectoryInfo(Path.GetDirectoryName(this.settingsFile)).Name;
    public int PID
    {
        get
        {
            try
            {
                return base.Id;
            }
            catch { return 0; }
        }
    }
    public bool IsDead
    {
        get
        {
            try
            {
                return base.HasExited;
            }
            catch (System.Exception)
            {
                return true;
            }
        }
    }
    public ProcessTasks(string settingsFile)
    {
        this.settingsFile = settingsFile;
        this.Settings = JsonConvert.DeserializeObject<JobSettings>(File.ReadAllText(settingsFile));

        base.StartInfo = new ProcessStartInfo
        {
            FileName = this.Settings.Command,
            Arguments = this.Settings.Args,
            WorkingDirectory = string.IsNullOrEmpty(this.Settings.CWD) ? Path.GetDirectoryName(settingsFile) : this.Settings.CWD,
            UseShellExecute = true,
            RedirectStandardOutput = false,
            WindowStyle = ProcessWindowStyle.Hidden
        };
    }

    public JobSettings GetProcessSettings()
    {
        var path = this.settingsFile;
        try
        {
            var dir = Path.GetDirectoryName(path);
            var pname = Path.GetFileNameWithoutExtension(path);
            var settingsFile = Path.Combine(dir, pname + ".json");
            this.Settings = JsonConvert.DeserializeObject<JobSettings>(File.ReadAllText(settingsFile));
            return this.Settings;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return null;
        }
    }
}