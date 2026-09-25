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
        //вывод результата

        var res = CheckConfiguration(maxPlayers,ram,isPublic,isPassword);
        Console.WriteLine(res);

    }

    public static string CheckConfiguration(int maxPlayers, int ram, bool isPublic, bool isPassword)
    {
        var troubles = "";


        if (maxPlayers <= 0)
        {
            troubles += "Запуск невозможен: количество игроков должно быть больше нуля.\n";
        }

        if (ram < 2)
        {
            troubles += "Запуск невозможен: серверу недостаточно оперативной памяти.\n";
        }

        if (isPublic && isPassword)
        {
            troubles += "Запуск возможен с предупреждением: публичный сервер защищён паролем.\n";
        }0
        

        if (maxPlayers > 100 && ram < 8)
        {
            troubles += "Запуск возможен с предупреждением: для такого количества игроков рекомендуется больше оперативной памяти.\n";
        }

        if(troubles == "")
        {
            return "Сервер готов к запуску.";
            
        }
        return troubles;

    }


    
}


