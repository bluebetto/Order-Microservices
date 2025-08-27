namespace OrderMicroservices.Common
{
    public class BaseEntity
    {
        public Guid Id { get; protected set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        protected void SetUpdated()
        {
            UpdatedAt = DateTime.UtcNow;
        }
    } 
}
