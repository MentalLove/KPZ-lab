using System;
using System.IO;

public interface ILogger
{
    void Log(string message);
    void Warn(string message);
    void Error(string message);
}

public class ConsoleLogger : ILogger
{
    public void Log(string message)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"[LOG]: {message}");
        Console.ResetColor();
    }

    public void Warn(string message)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"[WARNING]: {message}");
        Console.ResetColor();
    }

    public void Error(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"[ERROR]: {message}");
        Console.ResetColor();
    }
}

public class FileWriter
{
    private string filePath;

    public FileWriter(string path)
    {
        filePath = path;
    }

    public void Write(string text)
    {
        File.AppendAllText(filePath, text);
    }

    public void WriteLine(string text)
    {
        File.AppendAllText(filePath, text + "\n");
    }
}

public class FileLogger : ILogger
{
    private readonly FileWriter writer;

    public FileLogger(string filePath)
    {
        writer = new FileWriter(filePath);
    }

    public void Log(string message)
    {
        writer.WriteLine($"[LOG]: {message}");
    }

    public void Warn(string message)
    {
        writer.WriteLine($"[WARNING]: {message}");
    }

    public void Error(string message)
    {
        writer.WriteLine($"[ERROR]: {message}");
    }
}

class Program
{
    static void Main()
    {
        ILogger consoleLogger = new ConsoleLogger();
        ILogger fileLogger = new FileLogger("log.txt");

        consoleLogger.Log("Application started.");
        consoleLogger.Warn("Low disk space.");
        consoleLogger.Error("Unhandled exception occurred.");

        fileLogger.Log("File logging initialized.");
        fileLogger.Warn("Disk usage above threshold.");
        fileLogger.Error("NullReferenceException in module X.");

        Console.WriteLine("\nFile logging complete. Check 'log.txt'.");
    }
}
