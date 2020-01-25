using System.Threading.Tasks;

namespace SmartRegistry.Web.Interfaces
{
    public interface IEmailSender
    {
        Task HandleConfirmationEmailSendAsync(string email, string subject, string message);
        Task<Task> SendEmailAsync(string recipientName, string recipientEmailAddress, string subject, string body);

        Task SendReportAsync(byte[] file);
    }
}
