using System;
using System.IO;
using System.Linq;
using CalculatorApp;
using CalculatorApp.Models;
using Xunit;

namespace CalculatorApp.IntegrationTests;

public sealed class CalculatorServiceIntegrationTests : IDisposable
{
    private readonly string _filePath;
    private readonly XmlHistoryRepository _repository;
    private readonly CalculatorService _service;

    public CalculatorServiceIntegrationTests()
    {
        _filePath = Path.Combine(Path.GetTempPath(), $"history_integration_{Guid.NewGuid():N}.xml");
        _repository = new XmlHistoryRepository(_filePath);
        _service = new CalculatorService(new Calculator(), _repository);
    }

    [Fact]
    public void Calculate_Add_AppendsHistoryToXml()
    {
        var result = _service.Calculate("Add", 3, 4);

        Assert.Equal(7, result);
        var history = _service.GetHistory();
        Assert.Single(history);
        Assert.Equal("Add", history[0].Operation);
    }

    [Fact]
    public void ClearHistory_EmptiesXmlFile()
    {
        _service.Calculate("Add", 1, 2);
        _service.ClearHistory();

        var history = _service.GetHistory();

        Assert.Empty(history);
    }

    [Fact]
    public void Calculate_MoreThan10Entries_StoresOnlyLast10()
    {
        for (var i = 1; i <= 11; i++)
        {
            _service.Calculate("Add", i, i);
        }

        var history = _service.GetHistory();

        Assert.Equal(10, history.Count);
        Assert.Equal(4, history[0].Result);
        Assert.Equal(22, history[^1].Result);
    }

    public void Dispose()
    {
        if (File.Exists(_filePath))
        {
            File.Delete(_filePath);
        }
    }
}
