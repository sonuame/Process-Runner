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
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            WindowStyle = ProcessWindowStyle.Hidden,
            UserName = this.Settings.User,
            Domain = this.Settings.User.Contains("@") ? null : this.Settings.Domain
        };

        if (Path.GetDirectoryName(base.StartInfo.FileName) == "")
            base.StartInfo.FileName = Path.Combine(base.StartInfo.WorkingDirectory, base.StartInfo.FileName);

        if (string.IsNullOrEmpty(this.Settings.StdOut))
            this.Settings.StdOut = Path.Combine(base.StartInfo.WorkingDirectory, this.Name + ".log");

        if (!string.IsNullOrEmpty(this.Settings.StdOut) && Path.GetDirectoryName(this.Settings.StdOut) == "")
            this.Settings.StdOut = Path.Combine(base.StartInfo.WorkingDirectory, this.Settings.StdOut + ".log");


        if (string.IsNullOrEmpty(this.Settings.StdErr))
            this.Settings.StdErr = Path.Combine(base.StartInfo.WorkingDirectory, this.Name + ".log");

        if (!string.IsNullOrEmpty(this.Settings.StdErr) && Path.GetDirectoryName(this.Settings.StdErr) == "")
            this.Settings.StdErr = Path.Combine(base.StartInfo.WorkingDirectory, this.Settings.StdErr + ".log");


        base.OutputDataReceived += ProcessTasks_OutputDataReceived;
        base.ErrorDataReceived += ProcessTasks_ErrorDataReceived;

    }

    private void ProcessTasks_ErrorDataReceived(object sender, DataReceivedEventArgs e)
    {
        if (e.Data != null)
        {
            Utils.LogToPath(this.Settings.StdErr, e.Data);
            Console.WriteLine($"Process \"{this.Name}\" Error");
            Console.WriteLine(e.Data);
        }
    }

    private void ProcessTasks_OutputDataReceived(object sender, DataReceivedEventArgs e)
    {
        if (e.Data != null)
            Utils.LogToPath(this.Settings.StdOut, e.Data);
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