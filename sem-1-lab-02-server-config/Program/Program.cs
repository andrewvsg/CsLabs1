namespace Program;
    
using System;

public class Program
{
    //запрашиваем данные
    static void Main()
    {
        bool isPublic;
        bool isPassword;
        
        Console.WriteLine("Введите кол-во игроков");
        int maxPlayers = int.Parse(Console.ReadLine());

        Console.WriteLine("Введите кол-во оперативной памяти целым числом");
        int ram = int.Parse(Console.ReadLine());

        Console.WriteLine("Сервер публичный? (y/n)");
        char buffer1 = char.Parse(Console.ReadLine());
        if(buffer1 == 'y')
        {
            isPublic = true;
        }
        else
        {
            isPublic = false;
        }

        Console.WriteLine("Сервер защищён паролем? (y/n)");
        char buffer2 = char.Parse(Console.ReadLine());
        if(buffer2 == 'y')
        {
            isPassword = true;
        }
        else
        {
            isPassword = false;
        }

        var res = CheckConfiguration(maxPlayers,ram,isPublic,isPassword);
        Console.WriteLine(res);

    }

    public static string CheckConfiguration(int maxPlayers, int ram, bool isPublic, bool isPassword)
    {
        if (maxPlayers <= 0)
        {
            return "Запуск невозможен: количество игроков должно быть больше нуля.";
        }

        if (ram < 2)
        {
            return "Запуск невозможен: серверу недостаточно оперативной памяти.";
        }

        if (isPublic && isPassword)
        {
            return "Запуск возможен с предупреждением: публичный сервер защищён паролем.";
        }

        if (maxPlayers > 100 && ram < 8)
        {
            return "Запуск возможен с предупреждением: для такого количества игроков рекомендуется больше оперативной памяти.";
        }

        return "Сервер готов к запуску.";
    }


    
}


