using Orders.Domain.Persons;

namespace Orders.UnitTests.Customers;

public class PersonFactory
{
    public static Person Create()
    {
        var email = "customer@mail.com";
        var name = "Amir";

        return Person.From(
            email,
            name,
            true
        );
    }
}