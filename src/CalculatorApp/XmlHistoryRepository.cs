using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using CalculatorApp.Interfaces;
using CalculatorApp.Models;

namespace CalculatorApp;

public sealed class XmlHistoryRepository : IHistoryRepository
{
    private const int MaxEntries = 10;
    private readonly string _filePath;

    public XmlHistoryRepository(string filePath)
    {
        _filePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
    }

    public IReadOnlyList<CalculationRecord> Load()
    {
        if (!File.Exists(_filePath))
        {
            return Array.Empty<CalculationRecord>();
        }

        var document = XDocument.Load(_filePath);
        var root = document.Root;
        if (root is null)
        {
            return Array.Empty<CalculationRecord>();
        }

        return root.Elements("Entry")
            .Select(ReadEntry)
            .Where(record => record is not null)
            .Cast<CalculationRecord>()
            .ToList();
    }

    public void Save(IReadOnlyList<CalculationRecord> records)
    {
        var trimmedRecords = records?.TakeLast(MaxEntries).ToList() ?? new List<CalculationRecord>();
        var root = new XElement("CalculationHistory",
            trimmedRecords.Select(CreateEntry)
        );

        var document = new XDocument(new XDeclaration("1.0", "utf-8", "yes"), root);
        var directoryName = Path.GetDirectoryName(_filePath);

        if (!string.IsNullOrWhiteSpace(directoryName))
        {
            Directory.CreateDirectory(directoryName);
        }

        document.Save(_filePath);
    }

    private static CalculationRecord? ReadEntry(XElement entry)
    {
        try
        {
            var operation = entry.Element("Operation")?.Value ?? string.Empty;
            var operand1 = double.Parse(entry.Element("Operand1")?.Value ?? "0");
            var operand2 = double.Parse(entry.Element("Operand2")?.Value ?? "0");
            var result = double.Parse(entry.Element("Result")?.Value ?? "0");
            var timestampText = entry.Element("Timestamp")?.Value ?? string.Empty;
            var timestamp = DateTime.Parse(timestampText, null, System.Globalization.DateTimeStyles.AdjustToUniversal | System.Globalization.DateTimeStyles.AssumeUniversal);

            return new CalculationRecord(operation, operand1, operand2, result, timestamp);
        }
        catch
        {
            return null;
        }
    }

    private static XElement CreateEntry(CalculationRecord record)
    {
        return new XElement("Entry",
            new XElement("Operation", record.Operation),
            new XElement("Operand1", record.Operand1),
            new XElement("Operand2", record.Operand2),
            new XElement("Result", record.Result),
            new XElement("Timestamp", record.Timestamp.ToUniversalTime().ToString("o"))
        );
    }
}
