using CodingChallenge.Shopping.Discounts;
using CodingChallenge.Shopping.Models;

namespace CodingChallenge.Shopping.Services;

public sealed class CheckoutCalculator(IEnumerable<IDiscount> discounts)
{
	private readonly IDiscount[] _discounts = discounts.ToArray();

	public decimal Calculate(IEnumerable<CartItem> items, CheckoutContext context)
	{
		decimal total = 0m;

		foreach (var item in items)
		{
			var combinedRate = _discounts
				.Select(discount => discount.GetRate(item, context))
				.Where(rate => rate.HasValue)
				.Sum(rate => rate!.Value);

			// Current promotion policy: eligible rates stack additively and cannot exceed 100%.
			total += item.Subtotal * (1m - Math.Min(combinedRate, 1m));
		}

		return total;
	}
}