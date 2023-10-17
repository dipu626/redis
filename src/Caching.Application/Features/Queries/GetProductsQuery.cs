using Base.Application.Dtos.Responses;
using Caching.Domain.Entities;
using MediatR;

namespace Caching.Application.Features.Queries
{
    public class GetProductsQuery : IRequest<CommonResponse<Product>>
    {
    }
}
