using FluentValidation;
using System;

namespace Ordering.Application.Features.Orders.Commands.CheckoutOrder
{
    public class CheckoutOrderCommandValidator : AbstractValidator<CheckoutOrderCommand>
    {
        public CheckoutOrderCommandValidator()
        {
            RuleFor(p => p.UserName)
                .NotEmpty()
                .WithMessage("{UserName} is required.")
                .NotNull()
                .MaximumLength(60).WithMessage("UserName must not exceed 60 characters.");

            RuleFor(_ => _.EmailAddress)
                .NotEmpty()
                .WithMessage("EmailAdress is required.");

            RuleFor(_ => _.TotalPrice)
                .NotEmpty().WithMessage("Totalprice is required.")
                .GreaterThan(0).WithMessage("TotalPrice should be greated than zero.");
        }
    }
}
