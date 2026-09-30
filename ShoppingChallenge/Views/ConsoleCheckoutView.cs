
public sealed class ConsoleCheckoutView
{
	public void ShowTotal(string description, decimal total) =>
		Console.WriteLine($"{description}: {total:C}");
}