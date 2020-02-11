using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BoundlessDBWeb.Models;

namespace BoundlessDBWeb.Data
{
    public class BoundlessDbContext
    {
        public string ConnectionString { set; get; }
        public BoundlessDbContext(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        public List<Transaction> GetTransactions()
        {
            List<Transaction> transactions = new List<Transaction>();
            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();
                MySqlCommand query = new MySqlCommand("SELECT * FROM transactions", connection);

                using (var reader = query.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        transactions.Add(new Transaction()
                        {
                            Date = (DateTime)reader["Date"],
                            ItemId = Convert.ToInt32(reader["ItemId"]),
                            Coins = Convert.ToDouble(reader["Coins"]),
                            UnitaryValue = Convert.ToDouble(reader["UnitaryValue"]),
                            Quantity = Convert.ToInt32(reader["Quantity"])
                        });
                    }
                }
            }

            return transactions;
        }
        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }

    }
}
