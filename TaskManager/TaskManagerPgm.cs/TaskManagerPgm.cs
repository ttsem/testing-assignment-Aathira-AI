using System;

public class Printer
{
    public void Print()
    {
        Console.WriteLine("Printing document...");
    }
}

public class Scanner
{
    public void Scan()
    {
        Console.WriteLine("Scanning document...");
    }
}
public class PrintScanner{

}
public class TaskManager
{
  
    public void PrintTask(int taskId, Printer printer)
    {
        Console.WriteLine($"Executing Print Task: {taskId}");
        printer.Print();
    }

    public void ScanTask(int taskId, Scanner scanner)
    {
        Console.WriteLine($"Executing Scan Task: {taskId}");
        scanner.Scan();
    }
}

public class Program
{
    public static void Main()
    {
        var printer = new Printer();
        var scanner = new Scanner();
        var printScanner=new PrintScanner();

        var scheduler = new TaskManager();

        scheduler.PrintTask(101, printer);
        scheduler.ScanTask(102, scanner);
        scheduler.PrintTask(103, printScanner);
        scheduler.ScanTask(104, printScanner);
        
    }
}
