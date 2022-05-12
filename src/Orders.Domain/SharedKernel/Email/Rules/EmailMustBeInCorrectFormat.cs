using System.Net.Mail;
using Orders.Domain.SeedWork;

namespace Orders.Domain.SharedKernel.Email.Rules;

public class EmailMustBeInCorrectFormat : IBusinessRule
{
    private readonly string _value;

    public EmailMustBeInCorrectFormat(string value)
    {
        _value = value;
    }

    public string Message => "Email is not in correct format";

    public bool IsBroken() =>
        !MailAddress.TryCreate(
            _value,
            out var _
        );
}