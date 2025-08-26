using FluentValidation;

namespace OrderMicroservices.Orders.Application.Commands.CreateOrder
{
    public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderCommandValidator()
        {
            RuleFor(x => x.CustomerId)
                .NotEmpty()
                .WithMessage("Customer is Required");

            RuleFor(x => x.Items)
                .NotEmpty()
                .WithMessage("Orders must have at least one item");

            RuleForEach(x => x.Items) // Changed from RuleFor to RuleForEach
                .SetValidator(new OrderItemValidator());

            RuleFor(x => x.Address)
                .NotNull()
                .SetValidator(new AddressValidator());
        }
    }
}
