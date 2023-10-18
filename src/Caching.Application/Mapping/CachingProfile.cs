using AutoMapper;
using Caching.Application.Dtos;
using Caching.Application.Features.Commands;
using Caching.Domain.Entities;

namespace Caching.Application.Mapping
{
    public class CachingProfile : Profile
    {
        public CachingProfile()
        {
            CreateMap<Product, ProductResponse>().ReverseMap();
            CreateMap<AddProductCommand, Product>().ReverseMap();
        }
    }
}
