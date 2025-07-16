namespace SeriAllPort
{
    public class AppSettings
    {
        public string LastComPort { get; set; } = string.Empty;
        public int LastBaudRate { get; set; } = 9600;

        public Guid ProfileId { get; set; } = Guid.Empty;
    }
}
