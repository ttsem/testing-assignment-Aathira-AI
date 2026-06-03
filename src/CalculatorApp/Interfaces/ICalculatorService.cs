using System.Collections.Generic;
using CalculatorApp.Models;

namespace CalculatorApp.Interfaces;

public interface ICalculatorService
{
    double Calculate(string operation, double a, double b);
    IReadOnlyList<CalculationRecord> GetHistory();
    void ClearHistory();
}
