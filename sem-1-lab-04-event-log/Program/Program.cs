using System;
using System.IO;
using System.Collections.Generic;

public class LogEntry
{
    public DateTime TimeStamp;
    public string Level;
    public string Category;
    public string Message;
}

public class Program
{
    
    public static void Main()
    {
        var lines = File.ReadAllLines("event_server.long");
        var entries = ParseLog(lines);

        Console.WriteLine("Всего записей: " + entries.Length);
        Console.WriteLine("Info: "+ CountByLevel(entries, "Info"));
        Console.WriteLine("Warning: " + CountByLevel(entries, "Warning"));
        Console.WriteLine("Error: " + CountByLevel(entries, "Error"));
        Console.WriteLine("Fatal: " + CountByLevel(entries, "Fatal"));

        Console.WriteLine(GetServerStatus(entries)+ "\n");

        Console.WriteLine("Введите дату (год-месяц-день): ");
        var date = DateTime.Parse(Console.ReadLine() ?? "");
        PrintEntries(FilterByDate(entries, date));

        Console.WriteLine("Введите уровень: ");
        var level = Console.ReadLine() ?? "";
        PrintEntries(FilterByLevel(entries, level));


        Console.WriteLine("Введите категорию: ");
        var category = Console.ReadLine() ?? "";
        PrintEntries(FilterByCategory(entries, level));

        Console.WriteLine("Введите текст для поиска: ");
        var text = Console.ReadLine() ?? "";
        PrintEntries(Search(entries, text));




        
    }


    public static LogEntry[] ParseLog(string[] lines)
    {
        var entries = new LogEntry[lines.Length];

        for (var i = 0; i < lines.Length; i = i + 1)
        {
            var line = lines[i];
            var entry = new LogEntry();
            entry.TimeStamp = DateTime.Parse(line.Substring(0, 23));

            var fOpen = line.IndexOf('[');
            var fClose = line.IndexOf(']', fOpen);
            var sOpen = line.IndexOf('[', fClose);
            var sClose = line.IndexOf(']', sOpen);

            entry.Level = line.Substring(fOpen + 1, fClose - fOpen - 1);
            entry.Category = line.Substring(sOpen + 1, sClose - sOpen - 1);
            entry.Message = line.Substring(sClose + 2);
            entries[i] = entry;




        }
        return entries;
    }


    public static LogEntry[] FilterByDate(LogEntry[] entries, DateTime date)
    {
        var res = new List<LogEntry>();
        for (var i = 0; i < entries.Length; i = i + 1)
        {
            if(entries[i].TimeStamp.Date == date.Date)
            {
                res.Add(entries[i]);
            }
        }
        return res.ToArray();
        

    }


    public static LogEntry[] FilterByLevel(LogEntry[] entries, string level)
    {
        var res = new List<LogEntry>();
        for (var i = 0; i < entries.Length; i = i +1)
        {
            if(entries[i].Level == level)
            {
                res.Add(entries[i]);
            }
        }

        return res.ToArray();
    }

    public static LogEntry[] FilterByCategory(LogEntry[] entries, string category)
    {
        var res = new List<LogEntry>();

        for(var i = 0; i < entries.Length; i = i + 1)
        {
            if(entries[i].Category == category)
            {

                res.Add(entries[i]);
                
            }
        }

        return res.ToArray();
    }


    public static LogEntry[] Search(LogEntry[] entries, string text)
    {
        var res = new List<LogEntry>();

        for (var i = 0; i < entries.Length; i = i + 1)
        {
            if(entries[i].Message.Contains(text, StringComparison.OrdinalIgnoreCase))
            {
                res.Add(entries[i]);
            }
        }


        return res.ToArray();
    }


    public static int CountByLevel(LogEntry[] entries, string level)
    {
        var count = 0;

        for(var i = 0; i < entries.Length; i = i + 1)
        {
            if(entries[i].Level == level)
            {
                count++;

                
            }
        }

        return count;

        
    }


    public static string GetServerStatus(LogEntry[] entries)
    {
        var err = false;


        for (var i = 0; i < entries.Length; i++)
        {
            if(entries[i].Level== "Fatal" && entries[i].Category == "Server")
            {
                return "КРИТИЧЕСКАЯ ОШИБКА: сервер остановлен";
            }

            if(entries[i].Level == "Error" || entries[i].Level == "Fatal")
            {
                err = true;
            }
            
        }

        if (err)
        {
            return "Есть ошибки: требуется проверка";
        }

        return "Сервер работает штатно";
    }


    public static void PrintEntries(LogEntry[] entries)
    {
        Console.WriteLine("Найдено Записей: " + entries.Length);

        for (var i = 0; i < entries.Length; i++)
        {
            Console.WriteLine(entries[i].TimeStamp + " [" + entries[i].Level +"][" + entries[i].Category + "] " + entries[i].Message);

        }

    }


}