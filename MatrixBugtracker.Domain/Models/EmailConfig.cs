namespace MatrixBugtracker.Domain.Models
{
    public class EmailConfig
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string DestinationOverride { get; set; } // required to test sending an email. 
    }
}
