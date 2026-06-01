using System;
using System.IO;
using System.Linq;
using CalculatorApp;
using CalculatorApp.Models;
using Xunit;

namespace CalculatorApp.UnitTests;

public sealed class XmlHistoryRepositoryTests : IDisposable
{
    private readonly string _filePath;

    public XmlHistoryRepositoryTests()
    {
        _filePath = Path.Combine(Path.GetTempPath(), $"history_{Guid.NewGuid():N}.xml");
    }

    [Fact]
    public void Load_MissingFile_ReturnsEmptyList()
    {
        var repository = new XmlHistoryRepository(_filePath);

        var history = repository.Load();

        Assert.Empty(history);
    }

    [Fact]
    public void Save_AndLoad_Roundtrip()
    {
        var repository = new XmlHistoryRepository(_filePath);
        var record = new CalculationRecord("Add", 1, 2, 3, DateTime.UtcNow);

        repository.Save(new[] { record });
        var history = repository.Load();

        Assert.Single(history);
        Assert.Equal(record.Operation, history[0].Operation);
        Assert.Equal(record.Operand1, history[0].Operand1);
        Assert.Equal(record.Operand2, history[0].Operand2);
        Assert.Equal(record.Result, history[0].Result);
    }

    [Fact]
    public void Save_TrimsToLast10Entries()
    {
        var repository = new XmlHistoryRepository(_filePath);
        var records = Enumerable.Range(1, 12)
            .Select(i => new CalculationRecord("Add", i, i + 1, i + i + 1, DateTime.UtcNow))
            .ToList();

        repository.Save(records);
        var history = repository.Load();

        Assert.Equal(10, history.Count);
        Assert.Equal(3, history.First().Operand1);
        Assert.Equal(12, history.Last().Operand1);
    }

    public void Dispose()
    {
        if (File.Exists(_filePath))
        {
            File.Delete(_filePath);
        }
    }
}
