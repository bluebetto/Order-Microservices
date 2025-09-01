using AutoMapper;
using MediatR;
using OrderMicroservices.Products.Application.DTOs;
using OrderMicroservices.Products.Infra.Repositories;

namespace OrderMicroservices.Products.Application.Queries.GetProduct
{
    public class GetProductQueryHandler : IRequestHandler<GetProductQuery, ProductDto?>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public GetProductQueryHandler(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<ProductDto?> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(request.ProductId, cancellationToken);
            return product == null ? null : _mapper.Map<ProductDto>(product);
        }
    }
}
