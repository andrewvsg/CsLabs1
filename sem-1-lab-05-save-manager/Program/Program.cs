using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

public class SaveEntry
{
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("file")]
    public string FileName { get; set; }

    [JsonProperty("updatedAt")]
    public DateTime UpdatedAt { get; set; }
}

public class Position
{
    [JsonProperty("x")]
    public double X { get; set; }

    [JsonProperty("y")]
    public double Y { get; set; }
}

public class SaveData
{
    [JsonProperty("character")]
    public string Character { get; set; }

    [JsonProperty("map")]
    public string Map { get; set; }

    [JsonProperty("position")]
    public Position Position { get; set; }

    [JsonProperty("health")]
    public int Health { get; set; }

    [JsonProperty("inventory")]
    public List<string> Inventory { get; set; }
}

public class Program
{
    public static List<SaveEntry> LoadIndex(string indexPath)
    {
        if (!File.Exists(indexPath))
        {
            Console.WriteLine("Файл index.json не найден");
            return null;
        }

        try
        {
            var json = File.ReadAllText(indexPath);
            var entries = JsonConvert.DeserializeObject<List<SaveEntry>>(json);

            if (entries == null)
            {
                Console.WriteLine("Ошибка в index.json");
                return null;
            }

            foreach (var entry in entries)
            {
                if (entry == null || string.IsNullOrWhiteSpace(entry.Name) || string.IsNullOrWhiteSpace(entry.FileName))
                {
                    Console.WriteLine("Ошибка в index.json");
                    return null;
                }
            }

            return entries;
        }
        catch (Exception)
        {
            Console.WriteLine("Не удалось прочитать index.json");
            return null;
        }
    }

    public static SaveData LoadSave(string savesDirectory, SaveEntry entry)
    {
        var path = Path.Combine(savesDirectory, entry.FileName);

        if (!File.Exists(path))
        {
            Console.WriteLine("Файл сохранения не найден");
            return null;
        }

        try
        {
            var json = File.ReadAllText(path);
            var save = JsonConvert.DeserializeObject<SaveData>(json);

            if (save == null || save.Character == null || save.Map == null ||
                save.Position == null || save.Inventory == null)
            {
                Console.WriteLine("Ошибка в данных сохранения");
                return null;
            }

            return save;
        }
        catch (Exception)
        {
            Console.WriteLine("Не удалось прочитать сохранение");
            return null;
        }
    }

    public static void PrintSave(SaveEntry entry, SaveData save)
    {
        Console.WriteLine("Название: " + entry.Name);
        Console.WriteLine("Персонаж: " + save.Character);
        Console.WriteLine("Карта: " + save.Map);
        Console.WriteLine("X: " + save.Position.X);
        Console.WriteLine("Y: " + save.Position.Y);
        Console.WriteLine("Здоровье: " + save.Health);
        Console.WriteLine("Инвентарь: " + string.Join(", ", save.Inventory));
    }

    public static SaveEntry FindByName(List<SaveEntry> entries, string name)
    {
        foreach (var entry in entries)
        {
            if (string.Equals(entry.Name, name, StringComparison.OrdinalIgnoreCase))
            {
                return entry;
            }
        }

        return null;
    }

    public static bool SaveGame(string savesDirectory, SaveEntry entry, SaveData save)
    {
        try
        {
            Directory.CreateDirectory(savesDirectory);
            var path = Path.Combine(savesDirectory, entry.FileName);
            var json = JsonConvert.SerializeObject(save, Formatting.Indented);
            File.WriteAllText(path, json);
            Console.WriteLine("Сохранение изменено");
            return true;
        }
        catch (Exception)
        {
            Console.WriteLine("Ошибка при записи сохранения");
            return false;
        }
    }

    public static bool SaveIndex(string indexPath, List<SaveEntry> entries)
    {
        try
        {
            var settings = new JsonSerializerSettings();
            settings.DateFormatString = "yyyy-MM-ddTHH:mm:ss";
            var json = JsonConvert.SerializeObject(entries, Formatting.Indented, settings);
            File.WriteAllText(indexPath, json);
            return true;
        }
        catch (Exception)
        {
            Console.WriteLine("Не удалось сохранить index.json");
            return false;
        }
    }

    public static void CreateBackup(string savesDirectory, string backupsDirectory, SaveEntry entry)
    {
        var source = Path.Combine(savesDirectory, entry.FileName);
        var backup = Path.Combine(backupsDirectory, entry.FileName);

        if (!File.Exists(source))
        {
            Console.WriteLine("Файл сохранения не найден");
            return;
        }

        try
        {
            Directory.CreateDirectory(backupsDirectory);
            File.Copy(source, backup, true);
            Console.WriteLine("Резервная копия создана");
        }
        catch (Exception)
        {
            Console.WriteLine("Ошибка создания копии");
        }
    }

    public static void RestoreBackup(string savesDirectory, string backupsDirectory, SaveEntry entry)
    {
        var backup = Path.Combine(backupsDirectory, entry.FileName);
        var destination = Path.Combine(savesDirectory, entry.FileName);

        if (!File.Exists(backup))
        {
            Console.WriteLine("Резервная копия не найдена");
            return;
        }

        try
        {
            Directory.CreateDirectory(savesDirectory);
            File.Copy(backup, destination, true);
            Console.WriteLine("Сохранение восстановлено");
        }
        catch (Exception)
        {
            Console.WriteLine("Ошибка восстановления");
        }
    }

    public static void DeleteSave(string savesDirectory, string indexPath, List<SaveEntry> entries, SaveEntry entry)
    {
        var path = Path.Combine(savesDirectory, entry.FileName);

        try
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            else
            {
                Console.WriteLine("Файл сохранения не найден");
            }

            entries.Remove(entry);

            if (SaveIndex(indexPath, entries))
            {
                Console.WriteLine("Сохранение удалено");
            }
        }
        catch (Exception)
        {
            Console.WriteLine("Ошибка удаления");
        }
    }

    public static void Main()
    {
        var indexPath = "index.json";
        var savesDirectory = "saves";
        var backupsDirectory = "backups";

        Directory.CreateDirectory(savesDirectory);
        Directory.CreateDirectory(backupsDirectory);

        var entries = LoadIndex(indexPath);

        if (entries == null)
        {
            return;
        }

        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("1 - Список сохранений");
            Console.WriteLine("2 - Посмотреть сохранение");
            Console.WriteLine("3 - Изменить сохранение");
            Console.WriteLine("4 - Создать копию");
            Console.WriteLine("5 - Восстановить копию");
            Console.WriteLine("6 - Удалить сохранение");
            Console.WriteLine("7 - Выход");
            Console.Write("Выберите действие: ");

            var choice = Console.ReadLine();

            if (choice == "7")
            {
                return;
            }

            if (choice == "1")
            {
                foreach (var entry in entries)
                {
                    Console.WriteLine(entry.Name);
                }

                continue;
            }

            if (choice != "2" && choice != "3" && choice != "4" && choice != "5" && choice != "6")
            {
                Console.WriteLine("Нет такой команды");
                continue;
            }

            Console.Write("Введите имя сохранения: ");
            var name = Console.ReadLine();
            var selected = FindByName(entries, name);

            if (selected == null)
            {
                Console.WriteLine("Сохранение не найдено");
                continue;
            }

            if (choice == "2" || choice == "3")
            {
                var save = LoadSave(savesDirectory, selected);

                if (save == null)
                {
                    continue;
                }

                if (choice == "2")
                {
                    PrintSave(selected, save);
                    continue;
                }

                Console.Write("Новая карта: ");
                var map = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(map))
                {
                    save.Map = map;
                }

                Console.Write("Новая координата X: ");
                var xText = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(xText))
                {
                    if (double.TryParse(xText, out var x))
                    {
                        save.Position.X = x;
                    }
                    else
                    {
                        Console.WriteLine("Неверная координата X");
                    }
                }

                Console.Write("Новая координата Y: ");
                var yText = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(yText))
                {
                    if (double.TryParse(yText, out var y))
                    {
                        save.Position.Y = y;
                    }
                    else
                    {
                        Console.WriteLine("Неверная координата Y");
                    }
                }

                Console.Write("Новое здоровье: ");
                var healthText = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(healthText))
                {
                    if (int.TryParse(healthText, out var health))
                    {
                        save.Health = health;
                    }
                    else
                    {
                        Console.WriteLine("Неверное здоровье");
                    }
                }

                Console.Write("Новый инвентарь через запятую: ");
                var items = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(items))
                {
                    var inventory = new List<string>();

                    foreach (var item in items.Split(','))
                    {
                        if (!string.IsNullOrWhiteSpace(item))
                        {
                            inventory.Add(item.Trim());
                        }
                    }

                    save.Inventory = inventory;
                }

                if (SaveGame(savesDirectory, selected, save))
                {
                    selected.UpdatedAt = DateTime.Now;
                    SaveIndex(indexPath, entries);
                }
            }
            else if (choice == "4")
            {
                CreateBackup(savesDirectory, backupsDirectory, selected);
            }
            else if (choice == "5")
            {
                RestoreBackup(savesDirectory, backupsDirectory, selected);
            }
            else if (choice == "6")
            {
                DeleteSave(savesDirectory, indexPath, entries, selected);
            }
        }
    }
}
