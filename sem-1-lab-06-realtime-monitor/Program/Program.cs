
using System;

public class Monitor
{
    static int[] cpuHistory = new int[20];
    static int[] ramHistory = new int[20];
    static int[] tempHistory = new int[20];

    public static void Main()
    {
        var monitor = new ServerMonitor();
        monitor.DataReceived += OnDataReceived;

        Console.WriteLine("Мониторинг запущен");
        Console.WriteLine("Нажмите Enter для выхода");

        monitor.Start();
        Console.ReadLine();
        monitor.Stop();
    }

    public static void OnDataReceived(double cpu, double ram, double temperature)
    {
        var cpuValue = (int)Math.Round(cpu);
        var ramValue = (int)Math.Round(ram);
        var tempValue = (int)Math.Round(temperature);

        AddValue(cpuHistory, cpuValue);
        AddValue(ramHistory, ramValue);
        AddValue(tempHistory, tempValue);

        Console.Clear();
        Console.WriteLine("Мониторинг сервера");
        Console.WriteLine("Нажмите Enter для выхода");
        Console.WriteLine();

        Console.WriteLine("CPU: " + cpuValue + "%");
        PrintGraph(cpuHistory);
        Console.WriteLine();

        Console.WriteLine("RAM: " + ramValue + "%");
        PrintGraph(ramHistory);
        Console.WriteLine();

        Console.WriteLine("Температура: " + tempValue + " C");
        PrintGraph(tempHistory);
    }

    public static void AddValue(int[] values, int value)
    {
        for (var i = 0; i < values.Length - 1; i++)
        {
            values[i] = values[i + 1];
        }

        values[values.Length - 1] = value;
    }

    public static void PrintGraph(int[] values)
    {
        for (var level = 100; level >= 20; level -= 20)
        {
            Console.Write(level.ToString().PadLeft(3) + " | ");
            Console.WriteLine(RenderGraph(values, level));
        }
    }

    public static string RenderGraph(int[] values, int level = 50)
    {
        var graph = "";

        for (var i = 0; i < 20; i++)
        {
            if (i < values.Length && values[i] >= level)
            {
                graph += "#";
            }
            else
            {
                graph += ".";
            }
        }

        return graph;
    }
}
