using MenuFramework;
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
            Data.Transfer transfer = new Data.Transfer();
            AuthService authService = new AuthService();
            transfer.UserId = UserService.GetUserId();
            List<Data.Transfer> list = authService.ReturnTransfers();

            Console.WriteLine("____________________________________________");
            Console.WriteLine("Transfer");
            Console.WriteLine("ID\tFrom/To\t\t\tAmount");
            Console.WriteLine("____________________________________________");
            string currentUser = UserService.GetUserName();
            foreach (Data.Transfer l in list)
            {
                string displayName;
                
                if ( currentUser == l.ToName)
                {
                    displayName = $"From:\t{l.FromName}";
                }
                else
                {
                    displayName = $"To:  \t{l.ToName}";
                }

                Console.WriteLine($"{l.TransferId}\t{displayName} \t\t${l.Amount}");
            }
            Console.WriteLine("____________________________________________");
            Console.WriteLine();
            Console.Write("Please enter transfer ID to view details (0 to cancel): ");
            int id = Convert.ToInt32(Console.ReadLine());
            foreach (Data.Transfer l in list)
            {
                if(l.TransferId == id)
                {
                    Console.WriteLine();
                    Console.WriteLine($"Id: { l.TransferId}");
                    Console.WriteLine($"From: {l.FromName}");
                    Console.WriteLine($"To: {l.ToName}");
                    string type;
                    if (currentUser == l.ToName)
                    {
                        type = "Receive";
                    }
                    else
                    {
                        type = "Send";
                    }
                    Console.WriteLine($"Type: {type}");
                    Console.WriteLine($"Status: Approved");
                    Console.WriteLine($"Amount: ${l.Amount}");                     
                }
            }
            return MenuOptionResult.WaitAfterMenuSelection;
        }

        private MenuOptionResult ViewRequests()
        {
            Console.WriteLine("Not yet implemented!");
            return MenuOptionResult.WaitAfterMenuSelection;
        }

        private MenuOptionResult SendTEBucks()
        {
            Data.Transfer transfer = new Data.Transfer();
            AuthService authService = new AuthService();
            List<API_User> list = authService.ReturnUsers();

            Console.WriteLine("________________________________");
            Console.WriteLine("Users ID\t  Name");

            foreach (API_User l in list)
            {
                Console.WriteLine($"{l.UserId}\t      {l.Username}");
            }
            Console.WriteLine("________________________________");
            Console.WriteLine();

            Console.Write("Enter ID of user you are sending to (0 to cancel): ");
            transfer.TransferId = Convert.ToInt32(Console.ReadLine());

            if (transfer.TransferId == 0)
            {
                Console.WriteLine("The transaction was canceled!");
                return MenuOptionResult.WaitAfterMenuSelection;
            }

            if (transfer.TransferId > list.Count)
            {
                Console.WriteLine("This user does not exist!");
                return MenuOptionResult.WaitAfterMenuSelection;
                
            }

            Console.Write("Enter amount: ");
            transfer.Amount = Convert.ToDecimal(Console.ReadLine());
            transfer.UserId = UserService.GetUserId();

            authService.Transfer(transfer);

            if (transfer.Amount > authService.GetBalance())
            {
                Console.WriteLine("You don't have enough money in your account!");
            }
            else
            {
                Console.WriteLine("Success! Transaction was completed!");
            }

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
