namespace OrderMicroservices.Common.ValueObjects
{
    public record Stock(int Quantity)
    {
        public bool IsAvailable => Quantity > 0;

        public static implicit operator int(Stock stock) => stock.Quantity;
        public static implicit operator Stock(int quantity) => new(quantity);
    }
}
