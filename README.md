# Shopping Challenge

A small .NET console application demonstrating an extensible grocery checkout calculator. Discount policies are implemented independently through `IDiscount` and passed into `CheckoutCalculator` from the composition root in `Program.cs`.

See the \[Technical Overview\](docs/technical-overview.md) for the architecture, design concepts, checkout flow, and extension guidance.

## Requirements

*   .NET 10 SDK

## Run

From the repository root:

```sh
dotnet run --project ShoppingChallenge/ShoppingChallenge.csproj
```

## Test

```sh
dotnet test ShoppingChallenge.Tests/ShoppingChallenge.Tests.csproj
```

The xUnit tests cover discount date and time boundaries, discount stacking, and quantity- or weight-based item subtotals.

## Structure

*   `ShoppingChallenge/Models`: products, cart items, product categories, and checkout context.
*   `ShoppingChallenge/Discounts`: `IDiscount` and its Christmas, senior-hours, and first-responder implementations.
*   `ShoppingChallenge/Services`: `CheckoutCalculator`, which calculates item totals using the injected discount rules.
*   `ShoppingChallenge/Controllers`: checkout orchestration.
*   `ShoppingChallenge/Views`: console checkout output.
*   `ShoppingChallenge.Tests`: automated tests.

## Current Discount Rules

*   Christmas products in December receive 20% off on days 1–14, 60% off on days 15–25, and 90% off from day 26 onward.
*   Food items receive 10% off from 7:00 AM up to, but not including, 9:00 AM.
*   When the checkout context identifies a first responder, items receive 10% off.
*   Applicable discount rates are added together and capped at 100% per item.

To add a discount, implement `IDiscount` and add its instance to the discount array in `Program.cs`. The calculation engine does not need to change.