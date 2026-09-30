using CodingChallenge.Shopping.Models;

namespace CodingChallenge.Shopping.Discounts;

public sealed class SeniorHoursDiscount : IDiscount
{
	public decimal? GetRate(CartItem item, CheckoutContext context)
	{
		var checkoutTime = context.CheckoutDate.TimeOfDay;
		
		// 7:00 AM is included; 9:00 AM is excluded.
		return item.Product.Category == ProductCategory.Food
			   && checkoutTime >= new TimeSpan(7, 0, 0)
			   && checkoutTime < new TimeSpan(9, 0, 0)
			? 0.10m
			: null;
	}
}