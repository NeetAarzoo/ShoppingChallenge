namespace CodingChallenge.Shopping.Models;

public sealed class CheckoutContext(DateTime checkoutDate, bool isFirstResponder = false)
{
	public DateTime CheckoutDate { get; } = checkoutDate;
	public bool IsFirstResponder { get; } = isFirstResponder;
}