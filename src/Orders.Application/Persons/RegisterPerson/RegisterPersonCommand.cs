using MediatR;
using SampleProject.Application.Configuration.Commands;

namespace SampleProject.Application.Customers.RegisterCustomer
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