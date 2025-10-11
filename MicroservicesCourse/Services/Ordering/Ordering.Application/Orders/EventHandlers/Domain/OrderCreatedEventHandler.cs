using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement;
using Ordering.Application.Extensions;
using Ordering.Domain.Events;

namespace Ordering.Application.Orders.EventHandlers.Domain;

public class OrderCreatedEventHandler(
    IPublishEndpoint publisher, 
    IFeatureManager featureManager, 
    ILogger<OrderCreatedEventHandler> logger) 
    : INotificationHandler<OrderCreatedEvent>
{
    public async Task Handle(OrderCreatedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Domain event handled: {notification.GetType().Name}");

        if (!await featureManager.IsEnabledAsync("OrderFulfillment"))
            return;

        var orderCreatedIntegrationEvent = notification.order.ToOrderDto();
        await publisher.Publish(orderCreatedIntegrationEvent, cancellationToken);
    }
}