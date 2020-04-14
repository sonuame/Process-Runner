using System.Diagnostics;
using System.IO;

public class ProcessTasks : Process
{
    public string Name { get; set; }
    public int ExecutionCounter { get; set; }
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
        this.Name = Path.GetDirectoryName(pinfo.FileName);
    }
}