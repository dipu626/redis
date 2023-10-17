using Base.Application.Dtos.Responses;
using Caching.Application.Dtos;
using MediatR;

namespace Caching.Application.Features.Queries
{
    public class GetProductQuery : IRequest<CommonResponse<ProductResponse>>
    {
        public int Id { get; set; } = 0;
    }
}
