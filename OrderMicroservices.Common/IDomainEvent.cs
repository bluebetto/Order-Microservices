namespace OrderMicroservices.Common
{
    public interface IDomainEvent
    {
        Guid Id => Guid.NewGuid();
        DateTime OccurredAt => DateTime.UtcNow;
    }
}
