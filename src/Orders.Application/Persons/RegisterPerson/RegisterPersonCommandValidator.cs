﻿using FluentValidation;

namespace Orders.Application.Persons.RegisterPerson
{
    public class RegisterPersonCommandValidator : AbstractValidator<RegisterPersonCommand>
    {
        public RegisterPersonCommandValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is empty");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is empty");
        }
    }
}