using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Ticketing.Api.Dtos;
using Ticketing.Application.Common;
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
            var result = await _useCase.CreateOrder(
                request.TicketId,
                request.BuyerId
            );

            if (!result.IsSuccess)
            {
                return result.Error switch
                {
                    ErrorType.TicketNotFound => NotFound("Ticket not found"),
                    ErrorType.TicketAlreadyReserved => Conflict("Ticket already reserved"),
                    _ => BadRequest()
                };
            }

            return CreatedAtAction(
                nameof(Create),
                new { id = result.Value!.OrderId },
                result.Value
            );
        }

    }
}
