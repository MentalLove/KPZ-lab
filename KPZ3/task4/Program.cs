using System;
using System.IO;
using System.Text.RegularExpressions;

namespace ProxyExample
{
    // 1. Клас SmartTextReader: читає текстовий файл у двовимірний масив символів
    public class SmartTextReader
    {
        private string _filePath;

        public SmartTextReader(string filePath)
        {
            _filePath = filePath;
        }

        public char[][] Read()
        {
            string[] lines = File.ReadAllLines(_filePath);
            char[][] result = new char[lines.Length][];
            for (int i = 0; i < lines.Length; i++)
            {
                result[i] = lines[i].ToCharArray();
            }
            return result;
        }
    }

    // 2. Проксі SmartTextChecker з логуванням
    public class SmartTextChecker
    {
        private SmartTextReader _reader;

        public SmartTextChecker(string filePath)
        {
            _reader = new SmartTextReader(filePath);
        }

        public char[][] Read()
        {
            Console.WriteLine("Opening file...");
            char[][] content;
            try
            {
                content = _reader.Read();
                Console.WriteLine("File read successfully.");
                int totalLines = content.Length;
                int totalChars = 0;
                foreach (var line in content)
                    totalChars += line.Length;
                Console.WriteLine($"Total lines: {totalLines}");
                Console.WriteLine($"Total characters: {totalChars}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error reading file: " + ex.Message);
                throw;
            }
            Console.WriteLine("Closing file.");
            return content;
        }
    }

    // 3. Проксі SmartTextReaderLocker з обмеженням доступу
    public class SmartTextReaderLocker
    {
        private SmartTextReader _reader;
        private Regex _accessPattern;

        public SmartTextReaderLocker(string filePath, string regexPattern)
        {
            _accessPattern = new Regex(regexPattern);
            _reader = new SmartTextReader(filePath);
        }

        public char[][] Read()
        {
            if (_accessPattern.IsMatch(_readerFileName))
            {
                Console.WriteLine("Access denied!");
                return null;
            }
            return _reader.Read();
        }

        private string _readerFileName => Path.GetFileName(_readerFilePath);

        private string _readerFilePath => GetPrivateField<string>(_reader, "_filePath");

        // Метод для доступу до приватного поля _filePath через рефлексію
        private T GetPrivateField<T>(object obj, string fieldName)
        {
            var field = obj.GetType().GetField(fieldName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (field == null)
                throw new Exception($"Field {fieldName} not found");
            return (T)field.GetValue(obj);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Припустимо, є файли:
            // "allowed.txt" - доступний файл
            // "secret_data.txt" - заблокований файл

            // 1. Перевірка SmartTextReader
            Console.WriteLine("=== SmartTextReader ===");
            var reader = new SmartTextReader("allowed.txt");
            var data = reader.Read();
            PrintContent(data);

            // 2. Перевірка SmartTextChecker (з логуванням)
            Console.WriteLine("\n=== SmartTextChecker ===");
            var checker = new SmartTextChecker("allowed.txt");
            var checkedData = checker.Read();
            PrintContent(checkedData);

            // 3. Перевірка SmartTextReaderLocker (обмеження доступу)
            Console.WriteLine("\n=== SmartTextReaderLocker ===");
            string regex = @"secret_.*"; // файли, які починаються з "secret_"

            var lockerAllowed = new SmartTextReaderLocker("allowed.txt", regex);
            var allowedContent = lockerAllowed.Read();
            if (allowedContent != null)
                PrintContent(allowedContent);

            var lockerDenied = new SmartTextReaderLocker("secret_data.txt", regex);
            var deniedContent = lockerDenied.Read();
            if (deniedContent != null)
                PrintContent(deniedContent);
        }

        static void PrintContent(char[][] content)
        {
            if (content == null)
                return;
            foreach (var line in content)
            {
                Console.WriteLine(new string(line));
            }
        }
    }
}
