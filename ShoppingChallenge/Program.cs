using CodingChallenge.Shopping.Models;
using CodingChallenge.Shopping.Controllers;
using CodingChallenge.Shopping.Discounts;
using CodingChallenge.Shopping.Services;

IDiscount[] discounts =
{
	new ChristmasDiscount(),
	new FirstResponderDiscount(),
	new SeniorHoursDiscount()
};

var calculator = new CheckoutCalculator(discounts);
var controller = new CheckoutController(calculator, new ConsoleCheckoutView());

CartItem[] holidayCart =
[
	new(new Product("Lights", ProductCategory.Christmas, 5.99m), quantity: 10),
	new(new Product("Tree", ProductCategory.Christmas, 169m)),
	new(new Product("Ornaments", ProductCategory.Christmas, 8m), quantity: 15)
];

controller.Checkout("Christmas shopping before discounts", holidayCart,
	new CheckoutContext(new DateTime(2020, 11, 30)));
controller.Checkout("Christmas shopping after discounts", holidayCart,
	new CheckoutContext(new DateTime(2020, 12, 30)));

CartItem[] foodCart =  
{
	new(new Product("Apple", ProductCategory.Food, 3.27m), weight: 0.79m),
	new(new Product("Scallop", ProductCategory.Food, 18m), weight: 1.5m),
	new(new Product("Salad", ProductCategory.Food, 6.99m)),
	new(new Product("Ground Beef", ProductCategory.Food, 7.99m), weight: 1.5m),
	new(new Product("Red Wine", ProductCategory.Food, 25.99m))
};

controller.Checkout("Food shopping", foodCart,
	new CheckoutContext(new DateTime(2020, 11, 30)));
controller.Checkout("Food shopping during senior hours", foodCart,
	new CheckoutContext(new DateTime(2020, 11, 30, 7, 11, 0)));
controller.Checkout("First responder shopping", foodCart,
	new CheckoutContext(new DateTime(2020, 11, 30), isFirstResponder: true));
