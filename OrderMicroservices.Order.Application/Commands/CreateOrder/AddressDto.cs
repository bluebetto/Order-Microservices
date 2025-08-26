namespace OrderMicroservices.Order.Application.Commands.CreateOrder
{
    public record AddressDto(
            string Street,
            string Number,
            string Complement,
            string City,
            string State,
            string ZipCode,
            string Country
        );
}
