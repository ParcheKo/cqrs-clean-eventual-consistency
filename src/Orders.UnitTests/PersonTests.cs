using NUnit.Framework;
using Orders.Domain.Persons;
using Orders.Domain.Persons.Rules;
using Orders.Domain.SharedKernel.Email;
using Orders.UnitTests.SeedWork;

namespace Orders.UnitTests;

[TestFixture]
public class PersonTests : TestBase
{
    private class PersonRegistration : PersonTests
    {
        [Test]
        public void should_succeed_when_email_is_unique()
        {
            var email = Email.Of("testEmail@email.com");
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
            var email = "testEmail@email.com";
            const bool emailIsUnique = false;
            const string name = "Amir";

            AssertBrokenRule<PersonEmailMustBeUnique>(
                () =>
                {
                    Person.From(
                        Email.Of(email),
                        name,
                        emailIsUnique
                    );
                }
            );
        }
    }

    class WhenPersonRegisteredSuccessfully : PersonTests
    {
        [Test]
        public void person_registration_must_have_been_notified()
        {
            var email = "testEmail@email.com";
            const bool emailIsUnique = true;
            const string name = "Amir";
            var person = Person.From(
                Email.Of(email),
                name,
                emailIsUnique
            );

            AssertPublishedDomainEvent<PersonRegistered>(person);
        }
    }
}