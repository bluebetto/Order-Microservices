using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderMicroservices.Products.Application.Commands.UpdateStock
{
    public record UpdateStockResult(
        Guid ProductId,
        int PreviousStock,
        int NewStock
    );
}
