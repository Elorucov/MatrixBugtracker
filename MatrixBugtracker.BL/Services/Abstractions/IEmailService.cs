namespace MatrixBugtracker.BL.Services.Abstractions
{
    public interface IEmailService
    {
        Task SendMailAsync(string destination, string subject, string text);
    }
}
