# CalculatorSolution

This repository contains a C# calculator library with XML-based history persistence and separate unit/integration test projects.

## Structure

- `CalculatorSolution.sln` - Solution file referencing the application and test projects
- `src/CalculatorApp/` - Class library containing the calculator, service, and XML history repository
- `tests/CalculatorApp.UnitTests/` - xUnit unit tests for isolated behavior
- `tests/CalculatorApp.IntegrationTests/` - xUnit integration tests for end-to-end service and file persistence

## Build

```bash
dotnet build CalculatorSolution.sln
```

## Test

```bash
dotnet test CalculatorSolution.sln --no-restore
```

## Features

- `Calculator` supports `Add`, `Subtract`, `Multiply`, and `Divide`
- `XmlHistoryRepository` persists the last 10 calculation records to XML
- `CalculatorService` orchestrates calculation execution, history saving, retrieval, and clearing

## Notes

- Temporary files are used in integration tests to isolate file I/O behavior
- The library is implemented with clear interfaces for easier unit testing and future extension
