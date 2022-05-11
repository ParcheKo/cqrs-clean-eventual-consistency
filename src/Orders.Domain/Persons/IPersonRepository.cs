using System;
using System.Threading.Tasks;

namespace SampleProject.Domain.Customers.Orders
{
    public interface IPersonRepository
    {
        Task<Person> GetById(PersonId id);

        Task Add(Person person);
        Task<bool> ExistsWithEmail(string email);
    }
}