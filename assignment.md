### The calculator performs arithmetic operations and persists the last 10 calculations to an XML history file.
```
Code Under Test
Interfaces
// Pure calculation contract — no I/O
public interface ICalculator
{
    double Add(double a, double b);
    double Subtract(double a, double b);
    double Multiply(double a, double b);
    double Divide(double a, double b);
}

// History persistence contract — abstracts XML I/O
public interface IHistoryRepository
{
    IReadOnlyList<CalculationRecord> Load();
    void Save(IReadOnlyList<CalculationRecord> records);
}

// Orchestrator — combines calculation + history
public interface ICalculatorService
{
    double Calculate(string operation, double a, double b);
    IReadOnlyList<CalculationRecord> GetHistory();
    void ClearHistory();
}
Models
public record CalculationRecord(
    string Operation,      // "Add", "Subtract", "Multiply", "Divide"
    double Operand1,
    double Operand2,
    double Result,
    DateTime Timestamp);
Concrete Implementations
// Pure math — no dependencies
public class Calculator : ICalculator { ... }

// Reads/writes XML file — real I/O
public class XmlHistoryRepository : IHistoryRepository
{
    private readonly string _filePath;
    public XmlHistoryRepository(string filePath) => _filePath = filePath;
    // Serializes/deserializes List<CalculationRecord> to XML
    // Keeps only the last 10 entries on Save()
}

// Orchestrates calculation + history persistence
public class CalculatorService : ICalculatorService
{
    private readonly ICalculator _calculator;
    private readonly IHistoryRepository _history;
    // Calculate() → compute result → append to history → save → return result
    // GetHistory() → load from repository
    // ClearHistory() → save empty list
}
XML History Format
<?xml version="1.0" encoding="utf-8"?>
<CalculationHistory>
  <Entry>
    <Operation>Add</Operation>
    <Operand1>10</Operand1>
    <Operand2>5</Operand2>
    <Result>15</Result>
    <Timestamp>2026-04-01T09:30:00Z</Timestamp>
  </Entry>
  <!-- ... up to 10 entries, oldest dropped first -->
```

### Expectations
- Test Project Structure
- Project Dependencies
- Test Inventory

## Test Project Structure
Recommended layout for this project:

- `src/`
  - `calculator.py` or `calculator/__init__.py`
  - `calculator/models.py`
  - `calculator/interfaces.py`
  - `calculator/repository.py`
  - `calculator/service.py`
- `tests/unit/`
  - `test_calculator.py`
  - `test_history_repository.py`
- `tests/integration/`
  - `test_calculator_service.py`
  - `test_xml_history_repository.py`
- `requirements.txt`
- `requirements-dev.txt`
- `README.md`

This keeps pure logic unit tests separate from integration tests that involve history persistence and file I/O.

## Project Dependencies
Minimal dependencies for this Python project:

## Test Inventory
The tests are separated into unit tests for isolated behavior and integration tests for end-to-end service + persistence behavior.

### Unit Test Inventory
TestId | TestName | Input | Expected Behavior
--- | --- | --- | ---
U001 | `Calculator_Add_ReturnsSum` | `a=10, b=5` | Returns `15`
U002 | `Calculator_Subtract_ReturnsDifference` | `a=10, b=5` | Returns `5`
U003 | `Calculator_Multiply_ReturnsProduct` | `a=10, b=5` | Returns `50`
U004 | `Calculator_Divide_ReturnsQuotient` | `a=10, b=5` | Returns `2`
U005 | `Calculator_Divide_ByZero_Raises` | `a=10, b=0` | Raises `ZeroDivisionError`
U006 | `CalculatorService_UnsupportedOperation_Raises` | `operation='Mod', a=10, b=5` | Raises `ValueError`
U007 | `XmlHistoryRepository_Load_MissingFile_ReturnsEmptyList` | missing file path | Returns `[]`
U008 | `XmlHistoryRepository_Save_AndLoad_Roundtrip` | save one record then load | Loads record with same values
U009 | `XmlHistoryRepository_Save_TrimsToLast10` | save 12 records | Saved history contains only last 10 entries

### Integration Test Inventory
TestId | TestName | Input | Expected Behavior
--- | --- | --- | ---
I001 | `CalculatorService_Calculate_Add_AppendsHistory` | `operation='Add', a=3, b=4` | Returns `7` and history contains new record
I002 | `CalculatorService_Calculate_Divide_ViaRepository` | `operation='Divide', a=20, b=4` | Returns `5` and history persists to XML file
I003 | `CalculatorService_GetHistory_ReturnsSavedRecords` | existing XML history file | Returns same records from repository
I004 | `CalculatorService_ClearHistory_EmptiesRepository` | call `clear_history()` | Repository file contains empty history list
I005 | `CalculatorService_History_MaximumSizeIs10` | calculate 11 times | History load returns only last 10 records

## Best Decisions
- Keep unit tests isolated: test `Calculator` and `XmlHistoryRepository` independently.
- Keep integration tests focused on `CalculatorService` with `XmlHistoryRepository` using a temporary file.
- Use temporary files or fixtures for repository integration tests so tests do not depend on a fixed file path.
- Use `pytest` fixtures to create and clean up test resources.
- Treat exceptions as behavior: verify `ZeroDivisionError` and unsupported operation handling explicitly.

