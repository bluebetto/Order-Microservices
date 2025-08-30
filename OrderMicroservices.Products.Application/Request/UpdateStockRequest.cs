using OrderMicroservices.Products.Domain.Enums;

namespace OrderMicroservices.Products.Application.Request
{
    public record UpdateStockRequest(int Quantity, StockOperation Operation);
}
