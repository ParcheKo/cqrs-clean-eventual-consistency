using FluentValidation;
using Orders.Application.Persons.RegisterPerson;

namespace Orders.Application.Orders.RegisterOrder
{
    public class RegisterOrderCommandValidator : AbstractValidator<RegisterOrderCommand>
    {
        public RegisterOrderCommandValidator()
        {
            RuleFor(x => x.OrderDate).NotEmpty().WithMessage("OrderDate can not empty");
            RuleFor(x => x.OrderNo).NotEmpty().WithMessage("OrderNo can not empty");
            RuleFor(x => x.ProductName).NotEmpty().WithMessage("ProductName can not empty");
            RuleFor(x => x.Total).NotEmpty().WithMessage("Total can not empty");
            
            RuleFor(x => x.Price).NotEmpty().WithMessage("Price can not empty");
            RuleFor(x => x.Price).Must(BePositive).WithMessage("Price can not negative.");

            RuleFor(x => x.PersonEmail).NotEmpty().WithMessage("PersonEmail can not empty");
            RuleFor(p => p.PersonEmail).EmailAddress().WithMessage("PersonEmail is not in correct format");
        }

        private bool BePositive(decimal price) => price > 0;
    }
}