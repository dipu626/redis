using AutoMapper;
using Caching.Application.Dtos;
using Caching.Domain.Entities;

namespace Caching.Application.Mapping
{
    public class CachingProfile : Profile
    {
        public CachingProfile()
        {
            CreateMap<Product, ProductResponse>().ReverseMap();
        }
    }
}
