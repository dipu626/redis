using Base.Application.Dtos.Responses;
using MediatR;

namespace Caching.Application.Features.Commands
{
    public class DeleteProductCommand : IRequest<CommonResponse<bool>>
    {
        public int Id { get; set; } = 0;
    }
}
