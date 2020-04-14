namespace Process_Runner
{
    public enum JobSchedule
    {
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
        public string Args { get; set; }
        public string CWD { get; set; }
        public Schedule Schedule { get; set; }
    }
}
