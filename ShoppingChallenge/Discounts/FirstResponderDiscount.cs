using CodingChallenge.Shopping.Models;

namespace CodingChallenge.Shopping.Discounts;

public sealed class FirstResponderDiscount : IDiscount
{
	// Illustrative extension: applies a 10% rate to all items for verified eligibility.
	public decimal? GetRate(CartItem item, CheckoutContext context) =>
		context.IsFirstResponder ? 0.10m : null;
}