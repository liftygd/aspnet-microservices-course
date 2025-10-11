namespace Basket.API.Dtos;

public class BasketCheckoutDto
{
    public string UserName { get; set; } = null!;
    public Guid CustomerId { get; set; } = Guid.Empty;
    public decimal TotalPrice { get; set; } = 0;

    // Shipping and BillingAddress
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string EmailAddress { get; set; } = null!;
    public string AddressLine { get; set; } = null!;
    public string Country { get; set; } = null!;
    public string State { get; set; } = null!;
    public string ZipCode { get; set; } = null!;

    // Payment
    public string CardName { get; set; } = null!;
    public string CardNumber { get; set; } = null!;
    public string Expiration { get; set; } = null!;
    public string CVV { get; set; } = null!;
    public int PaymentMethod { get; set; } = 0;
    
    //Items
    public List<BasketCheckoutItemDto> Items { get; set; } = new();
}

public class BasketCheckoutItemDto
{
    public Guid OrderId { get; set; } = Guid.Empty;
    public Guid ProductId { get; set; } = Guid.Empty;
    public int Quantity { get; set; } = 0;
    public decimal Price { get; set; } = 0;
}