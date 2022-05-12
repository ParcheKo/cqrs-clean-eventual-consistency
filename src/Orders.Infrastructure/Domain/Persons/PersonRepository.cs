using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Orders.Domain.Persons;
using Orders.Infrastructure.WriteDatabase;

namespace Orders.Infrastructure.Domain.Persons;

public class PersonRepository : IPersonRepository
{
    private readonly OrdersContext _context;

    public PersonRepository(OrdersContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task Add(Person person)
    {
        await _context.Persons.AddAsync(person);
    }

    public async Task<bool> ExistsWithEmail(string email)
    {
        return await _context.Persons.AnyAsync(p => p.Email.Value == email);
    }

    public async Task<Person> GetById(PersonId id)
    {
        return await _context.Persons
            // .IncludePaths(
            //     PersonEntityTypeConfiguration.OrdersList,
            //     PersonEntityTypeConfiguration.OrderProducts
            // )
            .SingleAsync(x => x.Id == id);
    }
}