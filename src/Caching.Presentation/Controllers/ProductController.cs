using Base.Application.Dtos.Responses;
using Caching.Application.Features.Queries;
using Caching.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Caching.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController
    {
        private readonly IMediator _mediator;

        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("products")]
        public async Task<CommonResponse<Product>> GetProductsAsync([FromQuery] GetProductsQuery query)
        {
            return await _mediator.Send(query);
        }

    }
}
