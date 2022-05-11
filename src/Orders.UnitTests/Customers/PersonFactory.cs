using NSubstitute;
using SampleProject.Domain.Customers;

namespace SampleProject.UnitTests.Customers
{
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
}