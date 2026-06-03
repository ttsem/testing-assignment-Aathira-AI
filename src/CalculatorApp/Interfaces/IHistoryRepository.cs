using System.Collections.Generic;
using CalculatorApp.Models;

namespace CalculatorApp.Interfaces;

public interface IHistoryRepository
{
    IReadOnlyList<CalculationRecord> Load();
    void Save(IReadOnlyList<CalculationRecord> records);
}
