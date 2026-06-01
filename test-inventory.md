# Test Inventory

## Unit Test Inventory
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

## Integration Test Inventory
TestId | TestName | Input | Expected Behavior
--- | --- | --- | ---
I001 | `CalculatorService_Calculate_Add_AppendsHistory` | `operation='Add', a=3, b=4` | Returns `7` and history contains new record
I002 | `CalculatorService_Calculate_Divide_ViaRepository` | `operation='Divide', a=20, b=4` | Returns `5` and history persists to XML file
I003 | `CalculatorService_GetHistory_ReturnsSavedRecords` | existing XML history file | Returns same records from repository
I004 | `CalculatorService_ClearHistory_EmptiesRepository` | call `clear_history()` | Repository file contains empty history list
I005 | `CalculatorService_History_MaximumSizeIs10` | calculate 11 times | History load returns only last 10 records
