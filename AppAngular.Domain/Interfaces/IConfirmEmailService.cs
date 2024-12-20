namespace AppAngular.Domain.Interfaces
{
    public interface IConfirmEmailService
    {
        Task SendEmailAsync(string email, string subject, string htmlMessage);

        Task SendEmailAsync(IEnumerable<string> emails, string subject, string htmlMessage);
    }
}
