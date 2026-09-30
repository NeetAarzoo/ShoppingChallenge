using CodingChallenge.Shopping.Discounts;
using CodingChallenge.Shopping.Models;
using CodingChallenge.Shopping.Services;
using Xunit;

namespace CodingChallenge.Shopping.Tests;

public sealed class CheckoutDiscountTests
{
	[Theory]
	[InlineData(11, 30, -1)]
	[InlineData(12, 14, 20)]
	[InlineData(12, 15, 60)]
	[InlineData(12, 25, 60)]
	[InlineData(12, 26, 90)]
	public void ChristmasDiscountUsesExpectedDateRate(int month, int day, int expectedPercent)
	{
		var item = CreateItem(ProductCategory.Christmas);
		var context = new CheckoutContext(new DateTime(2020, month, day));
		decimal? expectedRate = expectedPercent < 0 ? null : expectedPercent / 100m;

		Assert.Equal(expectedRate, new ChristmasDiscount().GetRate(item, context));
	}

	[Theory]
	[InlineData(6, 59, false)]
	[InlineData(7, 0, true)]
	[InlineData(8, 59, true)]
	[InlineData(9, 0, false)]
	public void SeniorDiscountAppliesOnlyDuringSeniorHours(int hour, int minute, bool expected)
	{
		var item = CreateItem(ProductCategory.Food);
		var context = new CheckoutContext(new DateTime(2020, 11, 30, hour, minute, 0));

		Assert.Equal(expected ? 0.10m : null, new SeniorHoursDiscount().GetRate(item, context));
	}

	[Fact]
	public void SeniorDiscountDoesNotApplyToNonFoodItems()
	{
		var item = CreateItem(ProductCategory.Other);
		var context = new CheckoutContext(new DateTime(2020, 11, 30, 7, 0, 0));

		Assert.Null(new SeniorHoursDiscount().GetRate(item, context));
	}

	[Theory]
	[InlineData(false, -1)]
	[InlineData(true, 10)]
	public void FirstResponderDiscountDependsOnCheckoutContext(bool isFirstResponder, int expectedPercent)
	{
		var item = CreateItem(ProductCategory.Food);
		var context = new CheckoutContext(new DateTime(2020, 11, 30), isFirstResponder);
		decimal? expectedRate = expectedPercent < 0 ? null : expectedPercent / 100m;

		Assert.Equal(expectedRate, new FirstResponderDiscount().GetRate(item, context));
	}

	[Fact]
	public void CalculateEngineAddsApplicableDiscountRates()
	{
		var item = CreateItem(ProductCategory.Food, unitPrice: 100m);
		var discounts = new IDiscount[]
		{
			new FixedDiscount(0.10m),
			new FixedDiscount(0.20m)
		};
		var engine = new CheckoutCalculator(discounts);

		var total = engine.Calculate(new[] { item }, new CheckoutContext(DateTime.MinValue));

		Assert.Equal(70m, total);
	}

	[Fact]
	public void CalculateEngineCapsCombinedDiscountAtOneHundredPercent()
	{
		var item = CreateItem(ProductCategory.Food, unitPrice: 100m);
		var discounts = new IDiscount[]
		{
			new FixedDiscount(0.75m),
			new FixedDiscount(0.75m)
		};
		var engine = new CheckoutCalculator(discounts);

		var total = engine.Calculate(new[] { item }, new CheckoutContext(DateTime.MinValue));

		Assert.Equal(0m, total);
	}

	[Fact]
	public void CartItemSubtotalUsesWeightWhenProvided()
	{
		var item = new CartItem(new Product("Apples", ProductCategory.Food, 4m), weight: 1.5m);

		Assert.Equal(6m, item.Subtotal);
	}

	[Fact]
	public void CartItemSubtotalUsesQuantityWhenWeightIsNotProvided()
	{
		var item = new CartItem(new Product("Apples", ProductCategory.Food, 4m), quantity: 3m);

		Assert.Equal(12m, item.Subtotal);
	}

	private static CartItem CreateItem(ProductCategory category, decimal unitPrice = 1m) =>
		new(new Product("Test item", category, unitPrice));

	private sealed class FixedDiscount(decimal rate) : IDiscount
	{
		public decimal? GetRate(CartItem item, CheckoutContext context) => rate;
	}
}