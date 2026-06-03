using System;
using System.Collections.Generic;
using System.Linq;
using CalculatorApp.Interfaces;
using CalculatorApp.Models;

namespace CalculatorApp;

public sealed class CalculatorService : ICalculatorService
{
    private readonly ICalculator _calculator;
    private readonly IHistoryRepository _historyRepository;

    public CalculatorService(ICalculator calculator, IHistoryRepository historyRepository)
    {
        _calculator = calculator ?? throw new ArgumentNullException(nameof(calculator));
        _historyRepository = historyRepository ?? throw new ArgumentNullException(nameof(historyRepository));
    }

    public double Calculate(string operation, double a, double b)
    {
        if (string.IsNullOrWhiteSpace(operation))
        {
            throw new ArgumentException("Unsupported operation: value is null or whitespace.", nameof(operation));
        }

        var result = operation.ToLowerInvariant() switch
        {
            "add" => _calculator.Add(a, b),
            "subtract" => _calculator.Subtract(a, b),
            "multiply" => _calculator.Multiply(a, b),
            "divide" => _calculator.Divide(a, b),
            _ => throw new ArgumentException($"Unsupported operation: {operation}", nameof(operation)),
        };

        var history = _historyRepository.Load().ToList();
        history.Add(new CalculationRecord(operation, a, b, result, DateTime.UtcNow));
        _historyRepository.Save(history);

        return result;
    }

    public IReadOnlyList<CalculationRecord> GetHistory()
    {
        return _historyRepository.Load();
    }

    public void ClearHistory()
    {
        _historyRepository.Save(Array.Empty<CalculationRecord>());
    }
}
