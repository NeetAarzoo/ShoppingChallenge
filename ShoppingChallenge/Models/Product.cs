namespace CodingChallenge.Shopping.Models;

public sealed class Product(string name, ProductCategory category, decimal unitPrice)
{
	public string Name { get; } = name;
	public ProductCategory Category { get; } = category;
	public decimal UnitPrice { get; } = unitPrice;
}