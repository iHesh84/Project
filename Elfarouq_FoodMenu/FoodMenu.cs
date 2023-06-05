using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Diagnostics;
using Menu_item_creëren;
using System.Drawing;
using Console = Colorful.Console;

public static class FoodMenu
{
    public static void Start()
    {
        WriteLogo();
        Helper.Say("1", "Show food-menu");
        Helper.Say("2", "Order food");
        Helper.Say("3", "Go back");

        string input = Console.ReadLine();
        if (input == "1")
        {
            Console.Clear();
            MenuChoices.DisplayMenuOptions();
            Console.WriteLine("\nClick enter to go back");
            Console.ReadLine();
            Console.Clear();
            Start();
        }
        else if (input == "2")
        {
            Console.Clear();
            OrderFood.Start();
        }
        else if (input == "3") 
        {
            Console.Clear();
            MainMenu.LoggedInUser();
        }
        else 
        {
            Console.WriteLine("Error! Please choose a valid option!", Color.Red);
            Console.Clear();
            FoodMenu.Start(); 
        }
    }

    public static void WriteLogo()
    {
        string logo = @"  _____               _ __  __                  
 |  ___|__   ___   __| |  \/  | ___ _ __  _   _ 
 | |_ / _ \ / _ \ / _` | |\/| |/ _ \ '_ \| | | |
 |  _| (_) | (_) | (_| | |  | |  __/ | | | |_| |
 |_|  \___/ \___/ \__,_|_|  |_|\___|_| |_|\__,_|
                                                                                                
";

        Console.WriteLine(logo, Color.Wheat);
    }
}
