using CodingChallenge.Shopping.Models;
using CodingChallenge.Shopping.Services;
namespace CodingChallenge.Shopping.Controllers;

public sealed class CheckoutController(CheckoutCalculator calculator, ConsoleCheckoutView view)
{
	public void Checkout(string description, IEnumerable<CartItem> cart, CheckoutContext context)
	{
		var total = calculator.Calculate(cart, context);
		view.ShowTotal(description, total);
	}
}