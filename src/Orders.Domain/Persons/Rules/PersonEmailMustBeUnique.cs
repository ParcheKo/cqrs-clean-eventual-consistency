using SampleProject.Domain.SeedWork;

namespace SampleProject.Domain.Customers.Rules
{
    public class PersonEmailMustBeUnique : IBusinessRule
    {
        private readonly bool _isUnique;

        public PersonEmailMustBeUnique(bool isUnique)
        {
            _isUnique = isUnique;
        }

        public bool IsBroken() => !_isUnique;

        public string Message => "Person with this email already exists.";
    }
}