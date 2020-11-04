using System;
using System.Collections.Generic;
using System.Text;
using TenmoClient.Data;

namespace TenmoClient
{
    public static class AccountService
    {
        private static API_Account acc= new API_Account();

        public static void SetAccount(API_Account account)
        {
            acc = account;
        }
        public static decimal GetBalance()
        {
            return acc.Balance;
        }
    }
}
