using Base.Application.Dtos.Responses;
using MediatR;

namespace Caching.Application.Features.Commands
{
    public class AddProductCommand : IRequest<CommonResponse<bool>>
    {
        public string ProductName { get; set; } = string.Empty;
        public string ProductDescription { get; set; } = string.Empty;
        public int Stock { get; set; } = 0;
    }
}
