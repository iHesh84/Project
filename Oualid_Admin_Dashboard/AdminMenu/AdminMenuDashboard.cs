using System.Drawing;
using Console = Colorful.Console;
using Newtonsoft.Json;

static class AdminDashboardMenuDashboard
{
    public static Item[] menu = new Item[0];

    //This shows the menu. You can call back to this method to show the menu again
    //after another presentation method is completed.
    //You could edit this to show different menus depending on the user's role
    static public void DisplayMenuDashboard()
    {
        for (; ; )
        {
            Console.Clear();
            WriteLogo();
            WriteToConsole(1, "View Menu");
            WriteToConsole(2, "Edit Menu");
            WriteToConsole(3, "Back to Admin Dashboard");
            int input = Convert.ToInt32(Console.ReadLine());
            if (input == 1)
            {
                AdminMenuView.DisplayMenuDisplayOptions();
            }
            else if (input == 2)
            {
                AdminMenuEditor.DisplayMenuEditOptions();
            }
            else if (input == 3)
            {
                AdminDashboard.DisplayDashboard();
            }
            else
            {
                Console.WriteLine("Error! Please choose a valid option!", Color.Red);
            }
        }
    }
    public static void WriteToConsole(int prefix, string message)
    {
        Console.Write("[");
        Console.Write(prefix, Color.Red);
        Console.WriteLine("] " + message);
    }

    public static void WriteLogo()
    {
        string logo = @"
  __  __                  
 |  \/  | ___ _ __  _   _ 
 | |\/| |/ _ \ '_ \| | | |
 | |  | |  __/ | | | |_| |
 |_|  |_|\___|_| |_|\__,_|
                          
";

        Console.WriteLine(logo, Color.Wheat);
    }
}