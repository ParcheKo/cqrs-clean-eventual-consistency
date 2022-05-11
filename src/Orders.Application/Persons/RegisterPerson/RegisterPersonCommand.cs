using Orders.Application.Configuration.Commands;

namespace Orders.Application.Persons.RegisterPerson;

public class RegisterPersonCommand : CommandBase<PersonDto>
{
    public RegisterPersonCommand(
        string email,
        string name
    )
    {
        Email = email;
        Name = name;
    }

    public string Email { get; }

    public string Name { get; }
}