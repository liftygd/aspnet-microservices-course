using Basket.API.Data;
using Basket.API.Models;
using BuildingBlocks.CQRS;
using Discount.Grpc;
using FluentValidation;

namespace Basket.API.Basket.StoreBasket
{
    public record StoreBasketCommand(ShoppingCart Cart) : ICommand<StoreBasketResult>;
    public record StoreBasketResult(string UserName);

    public class StoreBasketCommandValidator : AbstractValidator<StoreBasketCommand>
    {
        public StoreBasketCommandValidator()
        {
            RuleFor(x => x.Cart).NotNull().WithMessage("Card cannot be null.");

            When(x => x.Cart != null, () =>
            {
                RuleFor(x => x.Cart.UserName).NotEmpty().WithMessage("Username cannot be empty.");
            });
        }
    }

    internal class StoreBasketCommandHandler(
        IBasketRepository repository,
        DiscountProtoService.DiscountProtoServiceClient discountProto)
        : ICommandHandler<StoreBasketCommand, StoreBasketResult>
    {
        public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
        {
            //Calculate prices with discount
            await ApplyDiscounts(command, cancellationToken);

            var cart = await repository.StoreBasket(command.Cart, cancellationToken);
            return new StoreBasketResult(cart.UserName);
        }

        private async Task ApplyDiscounts(StoreBasketCommand command, CancellationToken cancellationToken)
        {
            foreach (var item in command.Cart.Items)
            {
                var coupon = await discountProto.GetDiscountAsync(
                    new GetDiscountRequest { ProductName = item.ProductName },
                    cancellationToken: cancellationToken);

                item.Price -= coupon.Amount;
            }
        }
    }
}
