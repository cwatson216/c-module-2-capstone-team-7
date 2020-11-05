using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace TenmoServer.Models
{
    public class Account
    {
        public int UserId { get; set; }
        public int AccountId { get; set; }
        public decimal Balance { get; set; }
    }

    public class API_Account_Transfer
    {
        public int UserId { get; set; }
        public int TransferId { get; set; }
        public decimal Amount { get; set; }
    }
}
