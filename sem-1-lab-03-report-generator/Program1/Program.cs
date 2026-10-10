
using System;
using System.IO;

class Program
{
    static void Main()
    {
        var lines = File.ReadAllLines("event_server.log");
        Console.WriteLine(BuildReport(lines));
    }

    public static string BuildReport(string[] lines)
    {
        var date = "";
        var winner = "";
        var points = "";
        var item = "";
        var loser = "";
        var reward = "";
        var warnings = 0;
        var errors = 0;
        var started = false;

        foreach (var line in lines)
        {
            if (line.Contains("Событие началось:"))
            {
                started = true;
                var time = DateTime.Parse(line.Substring(0, 10));
                date = time.ToString("dd.MM.yyyy");
            }

            if (!started)
            {
                continue;
            }

            if (line.Contains("[Warning]"))
            {
                warnings = warnings + 1;
            }

            if (line.Contains("[Error]"))
            {
                errors = errors + 1;
            }

            if (line.Contains("объявлены победителями события"))
            {
                var start = line.IndexOf("] ") + 2;
                var end = line.IndexOf(" объявлены победителями");
                winner = line.Substring(start, end - start);
            }

            if (line.Contains("[Reward]") && line.Contains(" очков события"))
            {
                if (line.Contains(" получили ") && !line.Contains("утешительную"))
                {
                    var start = line.IndexOf(" получили ") + 10;
                    var end = line.IndexOf(" очков события");
                    points = line.Substring(start, end - start);
                }
            }

            if (line.Contains("получили ивентовый предмет:"))
            {
                var start = line.IndexOf("предмет:") + 8;
                item = line.Substring(start).Trim();
            }

            if (line.Contains("защитников из "))
            {
                var start = line.IndexOf("защитников из ") + 14;
                loser = line.Substring(start);
            }

            if (line.Contains("утешительную награду:"))
            {
                var start = line.IndexOf("награду:") + 8;
                reward = line.Substring(start).Trim();
            }

            if (line.Contains("закрыто") && line.Contains("[Server]"))
            {
                break;
            }
        }

        var report = "# Итоги события: Восстание Ледяного Пламени\n\n";
        report = report + "Дата: " + date + "\n";
        report = report + "Победитель: " + winner + "\n";
        report = report + "Очки победителя: " + points + "\n";
        report = report + "Ивентовый предмет: " + item + "\n";
        report = report + "Утешительная награда " + loser + ": " + reward + "\n";
        report = report + "Предупреждений во время события: " + warnings + "\n";
        report = report + "Ошибок во время события: " + errors;

        return report;
    }
}
