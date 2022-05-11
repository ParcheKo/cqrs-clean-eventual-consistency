using Orders.Domain.Persons;

namespace Orders.UnitTests.Orders;

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