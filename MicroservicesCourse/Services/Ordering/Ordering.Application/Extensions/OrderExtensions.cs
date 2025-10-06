using Ordering.Application.Dtos;
using Ordering.Domain.Models;

namespace Ordering.Application.Extensions;

public static class OrderExtensions
{
    public static List<OrderDto> ToOrderDtoList(this IEnumerable<Order> orders)
    {
        var list = new List<OrderDto>();
        
        foreach (var order in orders)
        {
            list.Add(order.ToOrderDto());
        }

        return list;
    }

    public static OrderDto ToOrderDto(this Order order)
    {
        var shippingAddress = new AddressDto(
            order.ShippingAddress.LastName,
            order.ShippingAddress.FirstName,
            order.ShippingAddress.EmailAddress,
            order.ShippingAddress.AddressLine,
            order.ShippingAddress.Country,
            order.ShippingAddress.State,
            order.ShippingAddress.ZipCode);
        
        var billingAddress = new AddressDto(
            order.BillingAddress.FirstName,
            order.BillingAddress.LastName,
            order.BillingAddress.EmailAddress,
            order.BillingAddress.AddressLine,
            order.BillingAddress.Country,
            order.BillingAddress.State,
            order.BillingAddress.ZipCode);

        var payment = new PaymentDto(
            order.Payment.CardName,
            order.Payment.CardNumber,
            order.Payment.Expiration,
            order.Payment.Cvv,
            order.Payment.PaymentMethod);

        var orderDto = new OrderDto(
            Id: order.Id.Value,
            CustomerId: order.CustomerId.Value,
            OrderName: order.OrderName.Value,
            ShippingAddress: shippingAddress,
            BillingAddress: billingAddress,
            Payment: payment,
            Status: order.Status,
            OrderItems: order.OrderItems
                .Select(oi => new OrderItemDto(
                    oi.OrderId.Value,
                    oi.ProductId.Value,
                    oi.Quantity,
                    oi.Price))
                .ToList()
        );

        return orderDto;
    }
}