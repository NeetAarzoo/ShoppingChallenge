namespace CodingChallenge.Shopping.Models;

public sealed class CartItem(Product product, decimal quantity = 1m, 
decimal? weight = null)
{
	public Product Product { get; } = product;
	public decimal Quantity { get; } = quantity;
	public decimal? Weight { get; } = weight;

	// A supplied weight takes precedence because weighted products are priced by weight.
	public decimal Subtotal => Product.UnitPrice * (Weight ?? Quantity);
}