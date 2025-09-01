using FluentValidation;

namespace OrderMicroservices.Products.Application.Commands.UpdateStock
{
    public class UpdateStockCommandValidator : AbstractValidator<UpdateStockCommand>
    {
        public UpdateStockCommandValidator()
        {
            RuleFor(x => x.ProductId)
                .NotEmpty()
                .WithMessage("Product ID is required");

            RuleFor(x => x.Quantity)
                .GreaterThan(0)
                .WithMessage("Quantity must be greater than zero");

            RuleFor(x => x.Operation)
                .IsInEnum()
                .WithMessage("Invalid stock operation");
        }
    }
}
