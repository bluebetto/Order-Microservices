namespace OrderMicroservices.Common.ValueObjects
{
    public record Money(decimal Amount, string Currency)
    {
        public static Money Zero(string currency) => new(0, currency);

        public static Money operator +(Money left, Money right)
        {
            if (left.Currency != right.Currency)
                throw new InvalidOperationException("Cannot add amounts with different currencies.");
            return new Money(left.Amount + right.Amount, left.Currency);
        }
    }
}
