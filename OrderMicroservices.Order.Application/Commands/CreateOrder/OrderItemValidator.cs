using FluentValidation;

namespace OrderMicroservices.Order.Application.Commands.CreateOrder
{
    public class OrderItemValidator : AbstractValidator<OrderItemDto>
    {
        public OrderItemValidator() {
            RuleFor(x => x.ProductId)
                .NotEmpty()
                .WithMessage("Product is required");

            RuleFor(x => x.ProductName)
                .NotEmpty()
                .WithMessage("Product name is required");

            RuleFor(x => x.Price)
                .GreaterThan(0)
                .WithMessage("Product price must be greater than Zero");

            RuleFor(x => x.Quantity)
                .GreaterThan(0)
                .WithMessage("Product quantity must be greater than Zero");
        }
    }
}
