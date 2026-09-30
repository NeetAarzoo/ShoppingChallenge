using CodingChallenge.Shopping.Models;

namespace CodingChallenge.Shopping.Discounts;

/// <summary>
/// Defines a policy that returns a discount rate for an applicable cart item.
/// A <see langword="null"/> result means the policy does not apply.
/// </summary>
public interface IDiscount
{
	/// <summary>
	/// Gets the applicable discount rate, or <see langword="null"/> when this policy
	/// does not apply to the item and checkout context.
	/// </summary>
	decimal? GetRate(CartItem item, CheckoutContext context);
}