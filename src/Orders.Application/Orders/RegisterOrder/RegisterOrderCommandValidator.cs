using FluentValidation;
using Orders.Application.Persons.RegisterPerson;

namespace Orders.Application.Orders.RegisterOrder
{
    public class RegisterOrderCommandValidator : AbstractValidator<RegisterOrderCommand>
    {
        public RegisterOrderCommandValidator()
        {
            RuleFor(x => x.OrderDate).NotEmpty().WithMessage("OrderDate is empty");
            RuleFor(x => x.PersonEmail).NotEmpty().WithMessage("PersonEmail is empty");
            RuleFor(x => x.OrderNo).NotEmpty().WithMessage("OrderNo is empty");
            RuleFor(x => x.ProductName).NotEmpty().WithMessage("ProductName is empty");
            RuleFor(x => x.Total).NotEmpty().WithMessage("Total is empty");
            RuleFor(x => x.Price).NotEmpty().WithMessage("Price is empty");
        }
    }
}