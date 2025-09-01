using AutoMapper;
using MediatR;
using OrderMicroservices.Products.Application.DTOs;
using OrderMicroservices.Products.Infra.Repositories;

namespace OrderMicroservices.Products.Application.Queries.GetProduct
{
    public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, GetProductsResult>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public GetProductsQueryHandler(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<GetProductsResult> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            var products = string.IsNullOrEmpty(request.Category)
                ? await _productRepository.GetAllAsync(request.Page, request.PageSize, cancellationToken)
                : await _productRepository.GetByCategoryAsync(request.Category, cancellationToken);

            var productDtos = _mapper.Map<List<ProductDto>>(products);

            return new GetProductsResult(
                productDtos,
                productDtos.Count,
                request.Page,
                request.PageSize
            );
        }
    }
}
