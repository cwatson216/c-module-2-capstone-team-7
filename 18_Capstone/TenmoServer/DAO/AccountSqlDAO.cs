using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
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
            int int1 = userId;
            int int2 = transferId;
            decimal dec3 = amount;

            Account acc = GetAccount(userId);
            //Account tranAcc = GetAccount(transferId);
            //decimal newTranBalance = tranAcc.Balance + amount;
            //decimal newAccBalance = acc.Balance - amount;

            //SqlConnection conn = new SqlConnection(connectionString);
            //SqlTransaction transaction;

            if (acc.Balance >= amount)
            {
                //conn.Open();
                //transaction = conn.BeginTransaction();
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        SqlTransaction transaction;
                        conn.Open();
                        transaction = conn.BeginTransaction();

                        SqlCommand cmd = new SqlCommand($"INSERT into transfers(transfer_type_id, transfer_status_id, account_from, account_to, amount) Values (2, 2, @fromId, @toId, @ammount)", conn);
                        cmd.Parameters.AddWithValue("@fromId", userId);
                        cmd.Parameters.AddWithValue("@toId", transferId);
                        cmd.Parameters.AddWithValue("@ammount", amount);
                        cmd.ExecuteNonQuery();

                        int i = Convert.ToInt32(new SqlCommand("Select @@IDENTITY = @identity"));

                        SqlCommand cmdBalanceDec = new SqlCommand($"UPDATE accounts set balance = balance - (select amount from transfers where transfer_id = {i}) where account_id = (select account_from from transfers where transfer_id = {i})", conn);
                        cmdBalanceDec.ExecuteNonQuery();
                        SqlCommand cmdBalanceAdd = new SqlCommand($"UPDATE accounts set balance = balance + (select amount from transfers where transfer_id = {i}) where account_id = (select account_to from transfers where transfer_id = {i})", conn);
                        cmdBalanceAdd.ExecuteNonQuery();
                        transaction.Commit();
                    }
                }
                catch (SqlException)
                {
                    throw;
                }
            } else
            {
                //transaction.Rollback();
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
