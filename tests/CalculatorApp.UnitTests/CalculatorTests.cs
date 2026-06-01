using CalculatorApp;
using Xunit;

namespace CalculatorApp.UnitTests;

public sealed class CalculatorTests
{
    private readonly Calculator _calculator = new();

    [Fact]
    public void Add_ReturnsSum()
    {
        var result = _calculator.Add(10, 5);

        Assert.Equal(15, result);
    }

    [Fact]
    public void Subtract_ReturnsDifference()
    {
        var result = _calculator.Subtract(10, 5);

        Assert.Equal(5, result);
    }

    [Fact]
    public void Multiply_ReturnsProduct()
    {
        var result = _calculator.Multiply(10, 5);

        Assert.Equal(50, result);
    }

    [Fact]
    public void Divide_ReturnsQuotient()
    {
        var result = _calculator.Divide(10, 5);

        Assert.Equal(2, result);
    }

    [Fact]
    public void Divide_ByZero_ThrowsDivideByZeroException()
    {
        Assert.Throws<DivideByZeroException>(() => _calculator.Divide(10, 0));
    }
}
