using Autofac;
using Orders.Application.Configuration.Emails;

namespace Orders.Infrastructure.Emails;

internal class EmailModule : Module
{
    private readonly IEmailSender _emailSender;
    private readonly EmailSettings _emailSettings;

    internal EmailModule(
        IEmailSender emailSender,
        EmailSettings emailSettings
    )
    {
        _emailSender = emailSender;
        _emailSettings = emailSettings;
    }

    internal EmailModule(EmailSettings emailSettings)
    {
        _emailSettings = emailSettings;
    }

    protected override void Load(ContainerBuilder builder)
    {
        if (_emailSender != null)
            builder.RegisterInstance(_emailSender);
        else
            builder.RegisterType<EmailSender>()
                .As<IEmailSender>()
                .InstancePerLifetimeScope();

        builder.RegisterInstance(_emailSettings);
    }
}