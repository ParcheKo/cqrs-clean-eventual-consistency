using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SampleProject.Domain.Customers;
using SampleProject.Domain.Customers.Orders;
using SampleProject.Infrastructure.Database;
using SampleProject.Infrastructure.SeedWork;

namespace SampleProject.Infrastructure.Domain.Customers
{
    public class PersonRepository : IPersonRepository
    {
        private readonly OrdersContext _context;

        public PersonRepository(OrdersContext context)
        {
            this._context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task Add(Person person)
        {
            await this._context.Persons.AddAsync(person);
        }

        public async Task<bool> ExistsWithEmail(string email)
        {
            return await _context.Persons.AnyAsync(p => p.Email == email);
        }

        public async Task<Person> GetById(PersonId id)
        {
            return await this._context.Persons
                // .IncludePaths(
                //     PersonEntityTypeConfiguration.OrdersList,
                //     PersonEntityTypeConfiguration.OrderProducts
                // )
                .SingleAsync(x => x.Id == id);
        }
    }
}