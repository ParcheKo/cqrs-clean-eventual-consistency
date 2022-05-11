using NSubstitute;
using NUnit.Framework;
using SampleProject.Domain.Customers;
using SampleProject.Domain.Customers.Rules;
using SampleProject.UnitTests.SeedWork;

namespace SampleProject.UnitTests.Customers
{
    [TestFixture]
    public class PersonTests : TestBase
    {
        class PersonCreation : PersonTests
        {
            [Test]
            public void should_succeed_when_email_is_unique()
            {
                const string email = "testEmail@email.com";
                const bool emailIsUnique = true;
                const string name = "Amir";
                Person.From(
                    email,
                    name,
                    emailIsUnique
                );
            }

            [Test]
            public void should_fail_when_email_is_duplicated()
            {
                const string email = "testEmail@email.com";
                const bool emailIsUnique = false;
                const string name = "Amir";

                AssertBrokenRule<PersonEmailMustBeUnique>(
                    () =>
                    {
                        Person.From(
                            email,
                            name,
                            emailIsUnique
                        );
                    }
                );
            }
        }
    }
}