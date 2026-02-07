namespace Ticketing.Application.Dtos;

public record CreateOrderResult(
    Guid OrderId,
    Guid TicketId,
    DateTime ExpiresAt
);
