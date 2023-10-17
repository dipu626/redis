using Base.Application.Attributes;
using Base.Application.Dtos.Responses;
using Base.Shared.Constants;
using Caching.Application.Dtos;
using Caching.Application.Features.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Caching.Presentation.Controllers.ProductController
{
    [ApiController]
    [ControllerNameAtrribute("Product.Query")]
    public class QueryController
    {
        private readonly IMediator _mediator;

        public QueryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet(EndpointRoutes.Action_GetProducts)]
        public async Task<CommonResponse<ProductResponse>> GetProductsAsync([FromQuery] GetProductsQuery query)
        {
            return await _mediator.Send(query);
        }

        [HttpGet(EndpointRoutes.Action_GetProduct)]
        public async Task<CommonResponse<ProductResponse>> GetProductAsync([FromQuery] GetProductQuery query)
        {
            return await _mediator.Send(query);
        }
    }
}
