namespace OrderMicroservices.Products.Application.DTOs
{
    public record ProductDto(
    Guid Id,
    string Name,
    string Description,
    decimal Price,
    string Currency,
    string Category,
    int StockQuantity,
    bool IsActive
);
}
