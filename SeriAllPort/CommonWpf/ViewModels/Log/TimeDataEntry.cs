namespace CommonWpf.ViewModels.Log
{
    public class LogEntry
    {
        public DateTime Time { get; set; }
        public string Message { get; set; }

        public LogEntry(DateTime time, string message)
        {
            Time = time;
            Message = message;
        }
    }
}
