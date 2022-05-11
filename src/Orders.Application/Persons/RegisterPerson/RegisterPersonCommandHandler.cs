using System.Threading;
using System.Threading.Tasks;
using SampleProject.Application.Configuration.Commands;
using SampleProject.Domain.Customers;
using SampleProject.Domain.Customers.Orders;
using SampleProject.Domain.SeedWork;

namespace SampleProject.Application.Customers.RegisterCustomer
{
    public class RegisterPersonCommandHandler : ICommandHandler<RegisterPersonCommand, PersonDto>
    {
        private readonly IPersonRepository _personRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RegisterPersonCommandHandler(
            IPersonRepository personRepository,
            IUnitOfWork unitOfWork
        )
        {
            this._personRepository = personRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<PersonDto> Handle(
            RegisterPersonCommand request,
            CancellationToken cancellationToken
        )
        {
            var noOneRegisteredWithTheEmail = !await _personRepository.ExistsWithEmail(request.Email);
            var person = Person.From(
                request.Email,
                request.Name,
                noOneRegisteredWithTheEmail
            );

            await this._personRepository.Add(person);

            await this._unitOfWork.CommitAsync(cancellationToken);

            return new PersonDto { Id = person.Id.Value };
        }
    }
}