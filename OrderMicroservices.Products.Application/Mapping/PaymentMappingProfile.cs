using AutoMapper;
using OrderMicroservices.Products.Application.DTOs;
using OrderMicroservices.Products.Domain.Entities;

namespace OrderMicroservices.Products.Application.Mapping
{
    public class ProductMappingProfile : Profile
    {
        public ProductMappingProfile()
        {
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price.Amount))
                .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.Price.Currency))
                .ForMember(dest => dest.StockQuantity, opt => opt.MapFrom(src => src.Stock.Quantity));
        }
    }
}
