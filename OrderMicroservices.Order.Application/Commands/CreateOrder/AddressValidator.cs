using FluentValidation;

namespace OrderMicroservices.Order.Application.Commands.CreateOrder
{
    public class AddressValidator : AbstractValidator<AddressDto>
    {
        public AddressValidator() { 
            RuleFor(x => x.Street).NotEmpty();
            RuleFor(x => x.Number).NotEmpty();
            RuleFor(x => x.District).NotEmpty();
            RuleFor(x => x.City).NotEmpty();
            RuleFor(x => x.State).NotEmpty();
            RuleFor(x => x.ZipCode).NotEmpty();
            RuleFor(x => x.Country).NotEmpty();
        }
    }
}
