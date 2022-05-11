using Orders.Application.Configuration.Commands;

namespace Orders.Application.Persons.RegisterPerson
{
    public class RegisterPersonCommand : CommandBase<PersonDto>
    {
        public string Email { get; }

        public string Name { get; }

        public RegisterPersonCommand(
            string email,
            string name
        )
        {
            this.Email = email;
            this.Name = name;
        }
    }
}