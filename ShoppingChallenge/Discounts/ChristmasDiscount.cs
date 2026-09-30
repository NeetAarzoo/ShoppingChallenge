using CodingChallenge.Shopping.Models;
namespace CodingChallenge.Shopping.Discounts;

public sealed class ChristmasDiscount : IDiscount
{
	public decimal? GetRate(CartItem item, CheckoutContext context)
	{
		if (item.Product.Category != ProductCategory.Christmas || context.CheckoutDate.Month != 12)
		{
			return null;
		}

		return context.CheckoutDate.Day switch
		{
			< 15 => 0.20m,
			<= 25 => 0.60m,
			> 25 => 0.90m
		};
	}
}