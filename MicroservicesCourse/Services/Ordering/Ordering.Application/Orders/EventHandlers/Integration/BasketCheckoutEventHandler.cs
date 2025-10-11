using BuildingBlocks.Messaging.Events;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Dtos;
using Ordering.Application.Orders.Commands.CreateOrder;
using Ordering.Domain.Enums;

namespace Ordering.Application.Orders.EventHandlers.Integration;

public class BasketCheckoutEventHandler(ISender mediator, ILogger<BasketCheckoutEventHandler> logger)
    : IConsumer<BasketCheckoutEvent>
{
    public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
    {
        logger.LogInformation($"Integration event handled: {context.Message.GetType().Name}");

        var command = MapToCreateOrderCommand(context.Message);
        await mediator.Send(command);
    }

    private CreateOrderCommand MapToCreateOrderCommand(BasketCheckoutEvent basketCheckoutEvent)
    {
        var addressDto = new AddressDto(
            basketCheckoutEvent.FirstName,
            basketCheckoutEvent.LastName,
            basketCheckoutEvent.EmailAddress,
            basketCheckoutEvent.AddressLine,
            basketCheckoutEvent.Country,
            basketCheckoutEvent.State,
            basketCheckoutEvent.ZipCode);

        var paymentDto = new PaymentDto(
            basketCheckoutEvent.CardName,
            basketCheckoutEvent.CardNumber,
            basketCheckoutEvent.Expiration,
            basketCheckoutEvent.CVV,
            basketCheckoutEvent.PaymentMethod);

        var orderId = Guid.NewGuid();

        var orderDto = new OrderDto(
            Id: orderId, 
            CustomerId: basketCheckoutEvent.CustomerId,
            OrderName: basketCheckoutEvent.UserName,
            ShippingAddress: addressDto,
            BillingAddress: addressDto,
            Payment: paymentDto,
            Status: OrderStatus.Pending,
            OrderItems: basketCheckoutEvent.Items
                .Select(bi => new OrderItemDto(orderId, bi.ProductId, bi.Quantity, bi.Price))
                .ToList());

        return new CreateOrderCommand(orderDto);
    }
}