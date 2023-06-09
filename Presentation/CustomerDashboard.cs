﻿using System.Drawing;
using Console = Colorful.Console;
using Newtonsoft.Json;
using Project.Presentation;

public static class CustomerDashboard
{

    
    private static CustomerAccount customer { get; set; }    
    private static List<Reservation> upcomingReservations { get; set; }
  
    private static Dictionary<int, string> filePaths = new Dictionary<int, string>()
    {
        { 2, @"..\..\..\DataSources\Oldreservations\oldReservationsWithin1YearAgo.json" },
        { 3, @"..\..\..\DataSources\Oldreservations\oldReservationsWithin1To2YearsAgo.json" },
        { 4, @"..\..\..\DataSources\Oldreservations\oldReservationsWithin2To3YearsAgo.json" },
        { 5, @"..\..\..\DataSources\Oldreservations\oldReservationsWithin1To2YearsAgo.json" },
        { 6, @"..\..\..\DataSources\Oldreservations\oldReservationsWithin2To3YearsAgo.json" },
        { 7, @"..\..\..\DataSources\Oldreservations\oldReservationsWithin1To2YearsAgo.json" },
    };


    public static void DisplayDashboard()
    {
        GetLoggedInCustomer();
        while(customer != null)
        {
            Console.Clear();
            WriteLogo();
            GetReservationsForCustomer();
            WriteToConsole(1, "Make new reservation");
            WriteToConsole(2, "Cancel a reservation");
            WriteToConsole(3, "View upcoming reservations");
            WriteToConsole(4, "View all reservations");
            WriteToConsole(5, "Make order");
            WriteToConsole(6, "Edit account");
            WriteToConsole(7, "Log out");
            string ? input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    Reservations.Reservationstart();
                    break;
                case "2":
                    CancelReservation.CancelNow();
                    break;
                case "3":
                    Console.Clear();
                    WriteLogo(@"
  _   _                           _                                                  _   _                 
 | | | |_ __   ___ ___  _ __ ___ (_)_ __   __ _   _ __ ___  ___  ___ _ ____   ____ _| |_(_) ___  _ __  ___ 
 | | | | '_ \ / __/ _ \| '_ ` _ \| | '_ \ / _` | | '__/ _ \/ __|/ _ \ '__\ \ / / _` | __| |/ _ \| '_ \/ __|
 | |_| | |_) | (_| (_) | | | | | | | | | | (_| | | | |  __/\__ \  __/ |   \ V / (_| | |_| | (_) | | | \__ \
  \___/| .__/ \___\___/|_| |_| |_|_|_| |_|\__, | |_|  \___||___/\___|_|    \_/ \__,_|\__|_|\___/|_| |_|___/
       |_|                                |___/                                                            
");
                    DisplayReservations(upcomingReservations);
                    Helper.ContinueDisplay();
                    break;
                case "4":
                    List<Reservation> oldReservations = GetAllOldReservations();
                    Console.Clear();
                    WriteLogo(@"
     _    _ _                                      _   _                 
    / \  | | |  _ __ ___  ___  ___ _ ____   ____ _| |_(_) ___  _ __  ___ 
   / _ \ | | | | '__/ _ \/ __|/ _ \ '__\ \ / / _` | __| |/ _ \| '_ \/ __|
  / ___ \| | | | | |  __/\__ \  __/ |   \ V / (_| | |_| | (_) | | | \__ \
 /_/   \_\_|_| |_|  \___||___/\___|_|    \_/ \__,_|\__|_|\___/|_| |_|___/
");
                    if(oldReservations.Count == 0 && upcomingReservations.Count == 0)
                    {
                        Console.WriteLine("No reservations!");
                        Helper.ContinueDisplay();
                        break;
                    }
                    if (oldReservations.Count > 0)
                    {
                        Console.WriteLine("Past reservations: ");
                        DisplayReservations(oldReservations);
                    }
                    if(upcomingReservations.Count > 0)
                    {
                        Console.WriteLine("Upcoming reservations: ");
                        DisplayReservations(upcomingReservations);
                    }
                    Helper.ContinueDisplay();
                    break;
                case "5":
                    FoodMenu.Start(false);
                    break;
                case "6":
                    CustomerProfileEditor.DisplayDashboard();
                    break;
                case "7":
                    LogUserOut();
                    break;
                default:
                    Console.WriteLine("Error! Please choose a valid option!", Color.Green);
                    Console.WriteLine();
                    Console.WriteLine("Press any key to return...");
                    Console.ReadKey();
                    break;
            }
        }
    }

        public static void WriteToConsole(int prefix, string message)
        {
            Console.Write("[");
            Console.Write(prefix, Color.Red);
            Console.WriteLine("] " + message);
        }
        public static void WriteLogo(string logoInput = "")
        {
            string logo = logoInput.Count() > 0 ? logoInput :  @"
  ____             __ _ _      
 |  _ \ _ __ ___  / _(_) | ___ 
 | |_) | '__/ _ \| |_| | |/ _ \
 |  __/| | | (_) |  _| | |  __/
 |_|   |_|  \___/|_| |_|_|\___|                                                                
";

            Console.WriteLine(logo, Color.Wheat);
        }
        public static void ClearScreen()
        {
            Console.Clear();
            WriteLogo();
        }
        public static void GetLoggedInCustomer()
        {
            List<CustomerAccount> accounts = CustomerAccess.LoadAll();
            CustomerAccount loggedInCustomer = accounts.FirstOrDefault(x => x.IsLoggedIn == true);
            if (loggedInCustomer != null)
            {
                customer = loggedInCustomer;
                Console.WriteLine("Logged in as: " + customer.UserNameSetter, Color.Green);
            }

            else {
                Console.WriteLine("Something went wrong! You will be redirected back to the log in page.");
                Console.ReadKey();
                AccountManager.LogIn();
            }
        }
        public static void GetReservationsForCustomer()
        {
            List<Reservation> allreservations = SaveReservations.LoadAll();
            upcomingReservations = allreservations.Where(r => r.CustomerId == customer.ID).ToList();
        }
        public static void DisplayReservations(List<Reservation> reservations)
        {
            if(reservations.Count == 0) {
                Console.WriteLine("No upcoming reservations!");
                Helper.ContinueDisplay();
                CustomerDashboard.DisplayDashboard();
            }
            else
            {
                foreach (Reservation reservation in reservations)
                {
                    AdminReservationsView.DisplayReservation(reservation);
                }
            }   

        }
        public static void LogUserOut()
        {
            List<CustomerAccount> accounts = CustomerAccess.LoadAll();
            CustomerAccount loggedInCustomer = accounts.FirstOrDefault(x => x.IsLoggedIn == true);
            if (loggedInCustomer != null)
            {
                loggedInCustomer.IsLoggedIn = false;
                CustomerAccess.WriteAll(accounts);
                MainMenu.NewStart(false);
            }   
            else
            {
                Console.WriteLine("Something went wrong! You will be redirected back to the log in page.");
                Console.ReadKey();
                AccountManager.LogIn();
            }
        }

        private static List<Reservation> GetAllOldReservations()
        {
            var allReservations = new List<Reservation>();

            foreach (var filePath in filePaths.Values)
            {
                string fullPath = Path.Combine(Environment.CurrentDirectory, filePath);
                string jsonString = File.ReadAllText(fullPath);
                List<Reservation> reservations = JsonConvert.DeserializeObject<List<Reservation>>(jsonString) ?? new List<Reservation>();
                allReservations.AddRange(reservations.Where(r => r.CustomerId == customer.ID));
        }

            return allReservations;
        }


}
