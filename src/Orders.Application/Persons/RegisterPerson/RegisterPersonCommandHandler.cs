using System.Threading;
using System.Threading.Tasks;
using Orders.Application.Configuration.Commands;
using Orders.Domain.Persons;
using Orders.Domain.SeedWork;
using Orders.Domain.SharedKernel.Email;

namespace Orders.Application.Persons.RegisterPerson;

public class RegisterPersonCommandHandler : ICommandHandler<RegisterPersonCommand, PersonDto>
{
    private readonly IPersonRepository _personRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterPersonCommandHandler(
        IPersonRepository personRepository,
        IUnitOfWork unitOfWork
    )
    {
        _personRepository = personRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<PersonDto> Handle(
        RegisterPersonCommand request,
        CancellationToken cancellationToken
    )
    {
        var noOneRegisteredWithTheEmail = !await _personRepository.ExistsWithEmail(request.Email);
        var person = Person.From(
            Email.Of(request.Email),
            request.Name,
            noOneRegisteredWithTheEmail
        );

        await _personRepository.Add(person);

        await _unitOfWork.CommitAsync(cancellationToken);

        return new PersonDto { Id = person.Id.Value };
    }
}