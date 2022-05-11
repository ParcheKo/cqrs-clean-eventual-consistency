using System.Threading.Tasks;
using Orders.Application.Configuration.Emails;

namespace Orders.Infrastructure.Emails;

public class EmailSender : IEmailSender
{
    public async Task SendEmailAsync(EmailMessage message)
    {
        // Integration with email service.
    }
}