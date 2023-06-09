﻿using Newtonsoft.Json;
using Project.Presentation;

public class Payment
{
    public static void AskPay(double amount, string reservationCode)
    {
        try
        {
            Console.WriteLine("How would you like to pay?");
            OrderFood.Say("1", "Card");
            OrderFood.Say("2", "Cash");
            OrderFood.Say("3", "Go back");
            string input = Console.ReadLine();
            Console.Clear();
            int customerID = ReservationSystem.GetCustomerIdFromReservation(reservationCode);
            if (input == "1")
            {
                Console.WriteLine("Please enter your card number:");
                string cardInput = Console.ReadLine();
                if (IsValidCard(cardInput))
                {
                    ReservationSystem.SetHasOrderdAnything(reservationCode, false);
                    ReservationSystem.SetReservationStatusToPaid(reservationCode, true);
                    WriteData(customerID, reservationCode, amount);
                    Console.WriteLine($"You have successfully paid €{amount}");
                    ClearCart(reservationCode);
                    Helper.ContinueDisplay();
                    Console.Clear();
                    OrderFood.Start(false);
                }
                else
                {
                    Console.WriteLine("Card invalid please try again...");
                    Console.ReadLine();
                    Console.Clear();
                    AskPay(amount, reservationCode);
                }
            }
            else if (input == "2")
            {
                ReservationSystem.SetHasOrderdAnything(reservationCode, false);
                ReservationSystem.SetReservationStatusToPaid(reservationCode, true);
                WriteData(customerID, reservationCode, amount);
                Console.WriteLine($"You have successfully paid €{amount}");
                ClearCart(reservationCode);
                Helper.ContinueDisplay();
                Console.Clear();
                OrderFood.Start(false);
            }
            else if (input == "3")
            {
                Console.Clear();
                OrderFood.Start(false);
            }
            else
            {
                Helper.ErrorDisplay("1, 2 or 3");

            }
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(ex.Message);
            Console.ResetColor();
            Helper.ContinueDisplay();
            Console.Clear();
            AskPay(amount, reservationCode);
        }
    }


    // Luhn Algorithm
    // valid credit card: 378282246310005
    // invalid credit card: 4111111111111112
    public static bool IsValidCard(string number)
    {
        if (string.IsNullOrEmpty(number))
        {
            return false;
        }

        int[] digits = new int[number.Length];
        for (int i = 0; i < number.Length; i++)
        {
            if (!int.TryParse(number[i].ToString(), out digits[i]))
            {
                return false;
            }
        }

        int sum = 0;
        bool isEvenIndex = false;
        for (int i = digits.Length - 1; i >= 0; i--)
        {
            if (isEvenIndex)
            {
                int doubledDigit = digits[i] * 2;
                sum += doubledDigit > 9 ? doubledDigit - 9 : doubledDigit;
            }
            else
            {
                sum += digits[i];
            }
            isEvenIndex = !isEvenIndex;
        }

        return sum % 10 == 0;
    }

    public static bool IsReservationCodeEmpty(string reservationCode)
    {
        string filePath = Path.Combine("..", "..", "..", "DataSources", "Orders.json");

        // Deserialize the existing JSON data
        List<Dictionary<string, List<Dictionary<string, int>>>> jsonData;
        string jsonString = File.ReadAllText(filePath);
        jsonData = JsonConvert.DeserializeObject<List<Dictionary<string, List<Dictionary<string, int>>>>>(jsonString);

        foreach (Dictionary<string, List<Dictionary<string, int>>> dict in jsonData)
        {
            if (dict.ContainsKey(reservationCode))
            {
                // Check if the list of dictionaries associated with the reservation code is empty
                return dict[reservationCode].Count == 0;
            }
        }

        // Reservation code not found, assuming it has no values
        return true;
    }



    public static void ClearCart(string reservationCode)
    {
        string filePath = Path.Combine("..", "..", "..", "DataSources", "Orders.json");

        List<Dictionary<string, List<Dictionary<string, int>>>> jsonData;
        string jsonString = File.ReadAllText(filePath);
        jsonData = JsonConvert.DeserializeObject<List<Dictionary<string, List<Dictionary<string, int>>>>>(jsonString);

        foreach (Dictionary<string, List<Dictionary<string, int>>> dict in jsonData)
        {
            if (dict.ContainsKey(reservationCode))
            {
                dict[reservationCode].Clear();
                break; 
            }
        }

        string updatedJsonString = JsonConvert.SerializeObject(jsonData, Formatting.Indented);
        File.WriteAllText(filePath, updatedJsonString);

    }



    public static void WriteData(int customerID, string reservationCode, double totalPrice)
    {
        string filePath = Path.Combine(Environment.CurrentDirectory, @"..\..\..\DataSources\PaidOrders.json");

        List<PaidOrder> paidOrders;

        if (File.Exists(filePath))
        {
            string jsonString = File.ReadAllText(filePath);
            paidOrders = JsonConvert.DeserializeObject<List<PaidOrder>>(jsonString);
        }
        else
        {
            paidOrders = new List<PaidOrder>(); 
        }

        var paidOrder = paidOrders.FirstOrDefault(o => o.ReservationCode == reservationCode);

        if (paidOrder != null)
        {
            paidOrder.CustomerId = customerID;
            paidOrder.PaidTime = DateTime.Now;
            paidOrder.TotalPrice = totalPrice;
        }
        else
        {
            var newPaidOrder = new PaidOrder
            {
                ReservationCode = reservationCode,
                CustomerId = customerID,
                PaidTime = DateTime.Now,
                TotalPrice = totalPrice
            };
            paidOrders.Add(newPaidOrder);
        }

        string updatedJsonString = JsonConvert.SerializeObject(paidOrders, Formatting.Indented);

        File.WriteAllText(filePath, updatedJsonString);
    }





}