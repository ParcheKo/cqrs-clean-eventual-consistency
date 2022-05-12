using Orders.Domain.SeedWork;
using Orders.Domain.SharedKernel.Email.Rules;

namespace Orders.Domain.SharedKernel.Email;

public class Email : ValueObject
{
    public string Value { get; private set; }

    private Email(string value)
    {
        Value = value;
    }

    public static Email Of(string value)
    {
        CheckRule(new EmailMustBeInCorrectFormat(value));

        return new Email(value);
    }
}