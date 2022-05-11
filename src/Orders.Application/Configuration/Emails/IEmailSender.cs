using System.Threading.Tasks;

namespace Orders.Application.Configuration.Emails;

public interface IEmailSender
{
    Task SendEmailAsync(EmailMessage message);
}