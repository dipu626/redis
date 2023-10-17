using Base.Application.Dtos.Responses;
using Caching.Application.Dtos;
using MediatR;

namespace Caching.Application.Features.Queries
{
    public class GetProductsQuery : IRequest<CommonResponse<ProductResponse>>
    {
    }
}
