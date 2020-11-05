using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public class AccountSqlDAO : IAccountDAO
    {
        private readonly string connectionString;
        //private decimal balance;

        public AccountSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public Account GetAccount(int userId)
        {
            Account returnAccount = null;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT account_id, user_id, balance FROM accounts WHERE user_id = @userId", conn);
                    cmd.Parameters.AddWithValue("@userId", userId);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows && reader.Read())
                    {
                        returnAccount = GetAccountFromReader(reader);
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }
            return returnAccount;
        }

        public void Transfer(int userId, int transferId, decimal amount)
        {
            Account acc = GetAccount(userId);
            Account tranAcc = GetAccount(transferId);
            decimal newTranBalance = tranAcc.Balance + amount;
            decimal newAccBalance = acc.Balance - amount;

            if (acc.Balance < amount)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();

                        SqlCommand cmd = new SqlCommand($"UPDATE accounts SET balance = {newTranBalance} WHERE user_id = {transferId}");
                        cmd.ExecuteNonQuery();
                        SqlCommand cmd2 = new SqlCommand($"UPDATE accounts SET balance = {newAccBalance} WHERE user_id = {userId}");
                        cmd2.ExecuteNonQuery();
                    }
                }
                catch (SqlException)
                {
                    throw;
                }
            } else
            {
                Console.WriteLine("You don't have enough money!");
            }
        }

        public Account GetAccountFromReader(SqlDataReader reader)
        {
            Account acc = new Account
            {
                AccountId = Convert.ToInt32(reader["account_id"]),
                UserId = Convert.ToInt32(reader["user_id"]),
                Balance = Convert.ToInt32(reader["balance"])
            };

            return acc;
        }
    }
}
