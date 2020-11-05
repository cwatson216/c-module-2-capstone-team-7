﻿using MenuFramework;
using System;
using System.Collections.Generic;
using System.Text;
using TenmoClient.Data;
using TenmoServer.Controllers;
using TenmoServer.DAO;
using TenmoServer.Models;

namespace TenmoClient.Views
{
    public class MainMenu : ConsoleMenu
    {
        public MainMenu()
        {
            AddOption("View your current balance", ViewBalance)
                .AddOption("View your past transfers", ViewTransfers)
                .AddOption("View your pending requests", ViewRequests)
                .AddOption("Send TE bucks", SendTEBucks)
                .AddOption("Request TE bucks", RequestTEBucks)
                .AddOption("Log in as different user", Logout)
                .AddOption("Exit", Exit);
        }

        protected override void OnBeforeShow()
        {
            Console.WriteLine($"TE Account Menu for User: {UserService.GetUserName()}");
        }

        private MenuOptionResult ViewBalance()
        {
            AuthService authService = new AuthService();
            decimal d = authService.GetBalance();
            Console.WriteLine($"Your current account balance is: ${d}");
            return MenuOptionResult.WaitAfterMenuSelection;
        }

        private MenuOptionResult ViewTransfers()
        {
            Console.WriteLine("Not yet implemented!");
            return MenuOptionResult.WaitAfterMenuSelection;
        }

        private MenuOptionResult ViewRequests()
        {
            Console.WriteLine("Not yet implemented!");
            return MenuOptionResult.WaitAfterMenuSelection;
        }

        private MenuOptionResult SendTEBucks()
        {
            AuthService authService = new AuthService();
            List<API_User> list = authService.ReturnUsers();

            Console.WriteLine("________________________________");
            Console.WriteLine("Users ID\t   Name");

            foreach (API_User l in list)
            {
                Console.WriteLine($"{l.UserId}\t      {l.Username}");
            }
            Console.WriteLine("________________________________");
            Console.WriteLine();
            Console.Write("Enter ID of user you are sending to (0 to cancel): ");
            int id = Convert.ToInt32(Console.ReadLine());
            Console.Write("Enter amount: ");
            decimal amount = Convert.ToDecimal(Console.ReadLine());

            return MenuOptionResult.WaitAfterMenuSelection;
        }

        private MenuOptionResult RequestTEBucks()
        {
            Console.WriteLine("Not yet implemented!");
            return MenuOptionResult.WaitAfterMenuSelection;
        }

        private MenuOptionResult Logout()
        {
            UserService.SetLogin(new API_User()); //wipe out previous login info
            return MenuOptionResult.CloseMenuAfterSelection;
        }

    }
}
