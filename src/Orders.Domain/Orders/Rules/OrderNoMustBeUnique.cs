using SampleProject.Domain.SeedWork;

namespace SampleProject.Domain.Customers.Rules
{
    public class OrderNoMustBeUnique : IBusinessRule
    {
        private readonly bool _isUnique;

        public OrderNoMustBeUnique(bool isUnique)
        {
            _isUnique = isUnique;
        }

        public bool IsBroken() => !_isUnique;

        public string Message => "Order with this order no. already exists.";
    }
}