namespace Process_Runner
{
    public enum JobSchedule
    {
        Polling,
        Daily,
        Weekly,
        Monthly,
        Yearly
    }

    public class Schedule
    {
        public JobSchedule Type { get; set; }
        public string Value { get; set; }
    }

    public class JobSettings
    {
        public string Command { get; set; }
        public string User { get; set; }
        public string Args { get; set; }
        public string CWD { get; set; }
        public string StdErr { get; set; }
        public string StdOut { get; set; }
        public Schedule Schedule { get; set; }
    }
}
