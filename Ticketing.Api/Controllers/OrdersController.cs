using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Ticketing.Api.Dtos;
using Ticketing.Application.UseCases;

namespace Ticketing.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly CreateOrderUseCase _useCase;
        
        public OrdersController (CreateOrderUseCase usecase)
        {
            _useCase = usecase;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrderRequest request)
        {
            var orderid = await _useCase.CreateOrder(request.TicketId, request.BuyerId);
            if (orderid is null)
                return Conflict("Ticket not available");


            return CreatedAtAction(
            nameof(Create),
                new { id = orderid.OrderId },
                null
            );

        }
    }
}
