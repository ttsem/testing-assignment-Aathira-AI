# Project Structure

```
project-root/
├── src/
│   ├── calculator.py or calculator/__init__.py
│   ├── calculator/
│   │   ├── models.py
│   │   ├── interfaces.py
│   │   ├── repository.py
│   │   └── service.py
├── tests/
│   ├── unit/
│   │   ├── test_calculator.py
│   │   └── test_history_repository.py
│   └── integration/
│       ├── test_calculator_service.py
│       └── test_xml_history_repository.py
├── README.md
└── test-inventory.md
```