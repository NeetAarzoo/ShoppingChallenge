---
name: shopping-challenge
description: "Use when working on the ShoppingChallenge .NET console checkout project: understand its architecture, build/run/test commands, or add and change checkout discount rules."
---

# ShoppingChallenge Project

Use this skill when changing or explaining this repository. It is a small .NET 10 console application that demonstrates a grocery checkout calculator with independently implemented discount policies. Keep changes consistent with the existing lightweight, in-memory design.

## Build And Run

Prerequisite: .NET 10 SDK.

Run these commands from the repository root:

```sh
dotnet build ShoppingChallenge/ShoppingChallenge.csproj
dotnet run --project ShoppingChallenge/ShoppingChallenge.csproj
dotnet test ShoppingChallenge.Tests/ShoppingChallenge.Tests.csproj
```

## Project Map

- `ShoppingChallenge/Program.cs` is the composition root. It creates the discount implementations, calculator, controller, sample carts, and checkout contexts.
- `ShoppingChallenge/Models/` contains `Product`, `ProductCategory`, `CartItem`, and `CheckoutContext`.
- `ShoppingChallenge/Discounts/IDiscount.cs` defines the discount policy contract. Each implementation in this folder returns a nullable decimal rate.
- `ShoppingChallenge/Services/CheckoutCalculator.cs` calculates item totals from configured policies.
- `ShoppingChallenge/Controllers/CheckoutController.cs` coordinates calculation and presentation.
- `ShoppingChallenge/Views/ConsoleCheckoutView.cs` writes the description and currency-formatted total to the console.
- `ShoppingChallenge.Tests/CheckoutDiscountTests.cs` contains the xUnit tests for discount rules and subtotal behavior.
- `docs/technical-overview.md` gives the longer architecture rationale and checkout flow.

There is no web framework, persistence layer, or dependency-injection container. Dependencies are composed directly in `Program.cs` and passed through constructors.

## Checkout Behavior

`CheckoutController.Checkout` passes the cart and context to `CheckoutCalculator.Calculate`, then sends the returned total to `ConsoleCheckoutView`.

For each cart item, the calculator asks every configured `IDiscount` for a rate. `null` means the rule does not apply. Applicable rates are added together, capped at 1, and applied to that item's subtotal:

```text
item total = subtotal * (1 - min(sum of applicable rates, 1))
checkout total = sum of item totals
```

Rates stack additively, not sequentially. The cap is per item. The calculator uses `decimal` values and does not round item or checkout totals. It does not validate model inputs or negative discount rates.

`CartItem.Subtotal` is `Product.UnitPrice * (Weight ?? Quantity)`. Quantity defaults to `1m`; when both quantity and weight are supplied, weight takes precedence. The models do not validate prices, quantities, or weights.

## Current Discount Rules

- `ChristmasDiscount`: applies only to `Christmas` products in December. Days 1-14 receive 20%, days 15-25 receive 60%, and days 26-31 receive 90%.
- `SeniorHoursDiscount`: applies 10% to `Food` products from 7:00 AM inclusive to 9:00 AM exclusive, using `CheckoutContext.CheckoutDate.TimeOfDay`.
- `FirstResponderDiscount`: applies 10% to every item when `CheckoutContext.IsFirstResponder` is true. The sample flag represents eligibility; this project does not verify it.

## Adding Or Changing A Discount

1. Implement `IDiscount.GetRate(CartItem, CheckoutContext)`. Return a decimal rate such as `0.10m` when applicable, or `null` otherwise.
2. Register the new instance in the `IDiscount[]` in `Program.cs`.
3. Add focused tests in `ShoppingChallenge.Tests/CheckoutDiscountTests.cs` for applicability and relevant boundaries, then cover stacking or interactions when they affect the expected total.
4. Run the test command above.

The calculator should remain policy-agnostic: add eligibility rules in a discount implementation rather than branching on a concrete discount or product category in `CheckoutCalculator`.

If a rule requires checkout data not currently represented in `CheckoutContext` or the models, extend the relevant model deliberately and test the new behavior. Update `README.md` and `docs/technical-overview.md` when changing documented business rules or architecture.

## Test Coverage And Scope

The existing tests cover Christmas date boundaries, senior-hours time boundaries and category eligibility, first-responder eligibility, additive stacking, the 100% cap, and quantity- versus weight-based subtotals. Preserve these boundary cases when refactoring.

This is an educational sample, not a complete point-of-sale system. Tax, persistence, price/quantity validation, eligibility verification, and a production rounding policy are out of scope unless explicitly requested.