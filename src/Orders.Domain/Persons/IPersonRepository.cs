using System.Threading.Tasks;

namespace Orders.Domain.Persons;

public interface IPersonRepository
{
    Task<Person> GetById(PersonId id);

    Task Add(Person person);
    Task<bool> ExistsWithEmail(string email);
}