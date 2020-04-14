using Newtonsoft.Json;
using Process_Runner;
using System;
using System.Diagnostics;
using System.IO;

public class ProcessTasks : Process
{
    public int ExecutionCounter { get; set; }
    private JobSettings settings;
    public JobSettings Settings => this.settings ?? GetProcessSettings(this.StartInfo.FileName);
    public string Name
    {
        get
        {
            try
            {
                return "process-runner-" + base.ProcessName;
            }
            catch { return "process-runner-" + Path.GetFileNameWithoutExtension(base.StartInfo.FileName); }
        }
    }
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
    public ProcessTasks(ProcessStartInfo pinfo)
    {
        base.StartInfo = pinfo;
        if (this.Settings != null)
        {
            var pdir = Path.GetDirectoryName(pinfo.FileName);

            base.StartInfo.Arguments = this.Settings.Args;
            base.StartInfo.WorkingDirectory = string.IsNullOrEmpty(this.Settings.CWD) ? pdir : this.Settings.CWD;
            if (Path.GetExtension(pinfo.FileName).ToLower() == ".lnk")
                base.StartInfo.WorkingDirectory = null;
        }

        //base.StartInfo.Arguments += " > " + Path.GetFileNameWithoutExtension(pinfo.FileName) + ".log";
    }

    public JobSettings GetProcessSettings(string path)
    {
        try
        {
            var dir = Path.GetDirectoryName(path);
            var pname = Path.GetFileNameWithoutExtension(path);
            var settingsFile = Path.Combine(dir, pname + ".json");
            this.settings = JsonConvert.DeserializeObject<JobSettings>(File.ReadAllText(settingsFile));
            return this.settings;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return null;
        }
    }
}