using FluentValidation;
using OrdersTask.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersTask.Application.Validators
{
    public class CreateOrderValidator : AbstractValidator<CreateOrderDto>
    {
        public CreateOrderValidator()
        {
            RuleFor(x => x.CustomerName)
                .NotEmpty().WithMessage("Customer name is required")
                .MaximumLength(100).WithMessage("Customer name must not exceed 100 characters");

            RuleFor(x => x.Product)
                .NotEmpty().WithMessage("Product is required")
                .MaximumLength(200).WithMessage("Product must not exceed 200 characters");

            RuleFor(x => x.Amount)
                .GreaterThan(0).WithMessage("Amount must be greater than zero")
                .LessThanOrEqualTo(999999.99m).WithMessage("Amount must not exceed 999,999.99");
        }
    }
}
