using System;

namespace CalculatorApp.Models;

public sealed record CalculationRecord(
    string Operation,
    double Operand1,
    double Operand2,
    double Result,
    DateTime Timestamp
);
