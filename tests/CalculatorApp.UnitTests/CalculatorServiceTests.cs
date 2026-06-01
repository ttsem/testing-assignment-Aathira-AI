using System.Collections.Generic;
using CalculatorApp;
using CalculatorApp.Interfaces;
using CalculatorApp.Models;
using Xunit;

namespace CalculatorApp.UnitTests;

public sealed class CalculatorServiceTests
{
    [Fact]
    public void Calculate_InvalidOperation_ThrowsArgumentException()
    {
        var service = new CalculatorService(new Calculator(), new FakeHistoryRepository());

        Assert.Throws<ArgumentException>(() => service.Calculate("Mod", 10, 5));
    }

    [Fact]
    public void Calculate_Add_AppendsHistory()
    {
        var repository = new FakeHistoryRepository();
        var service = new CalculatorService(new Calculator(), repository);

        var result = service.Calculate("Add", 3, 4);

        Assert.Equal(7, result);
        Assert.Single(repository.Load());
        Assert.Equal("Add", repository.Load()[0].Operation);
    }

    [Fact]
    public void ClearHistory_EmptiesRepository()
    {
        var repository = new FakeHistoryRepository();
        var service = new CalculatorService(new Calculator(), repository);

        service.Calculate("Add", 1, 1);
        service.ClearHistory();

        Assert.Empty(repository.Load());
    }

    private sealed class FakeHistoryRepository : IHistoryRepository
    {
        private readonly List<CalculationRecord> _records = new();

        public IReadOnlyList<CalculationRecord> Load() => _records.AsReadOnly();

        public void Save(IReadOnlyList<CalculationRecord> records)
        {
            _records.Clear();
            _records.AddRange(records);
        }
    }
}
