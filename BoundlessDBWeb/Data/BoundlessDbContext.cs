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
        public string ErrorMessage { set; get; }
        public BoundlessDbContext(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        public Transaction GetTransaction(int id)
        {
            Transaction transaction;
            MySqlConnection conn = GetConnection();
            try
            {
                conn.Open();
                MySqlCommand query = new MySqlCommand(
                    "SELECT transactions.TransactionID, transactions.DATE, items.ItemName, transactions.Coins, transactions.UnitaryValue, transactions.Quantity, locations.NAME " +
                    "FROM transactions INNER JOIN items ON items.ItemId = transactions.ItemId INNER JOIN locations ON locations.LocationID = transactions.LocationId " +
                    $"WHERE transactions.TransactionID = {id}", conn);
                var reader = query.ExecuteReader();
                reader.Read();
                transaction = new Transaction()
                {
                    TransactionId = Convert.ToInt32(reader["TransactionId"]),
                    Date = (DateTime)reader["Date"],
                    ItemName = reader["ItemName"].ToString(),
                    Coins = Convert.ToDouble(reader["Coins"]),
                    UnitaryValue = Convert.ToDouble(reader["UnitaryValue"]),
                    Quantity = Convert.ToInt32(reader["Quantity"]),
                    LocationName = reader["Name"].ToString()
                };
            } 
            catch(Exception ex)
            {
                transaction = null;
                ErrorMessage = ex.ToString();
            }
            return transaction;
        }

        public List<Transaction> GetTransactions()
        {
            List<Transaction> transactions = new List<Transaction>();
            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();
                MySqlCommand query = new MySqlCommand(
                    "SELECT transactions.TransactionID, transactions.DATE, items.ItemName, transactions.Coins, transactions.UnitaryValue, transactions.Quantity, locations.NAME " +
                    "FROM transactions INNER JOIN items ON items.ItemId = transactions.ItemId INNER JOIN locations ON locations.LocationID = transactions.LocationId " +
                    "ORDER BY transactions.DATE DESC", connection);

                using (var reader = query.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        transactions.Add(new Transaction()
                        {
                            TransactionId = Convert.ToInt32(reader["TransactionId"]),
                            Date = (DateTime)reader["Date"],
                            ItemName = reader["ItemName"].ToString(),
                            Coins = Convert.ToDouble(reader["Coins"]),
                            UnitaryValue = Convert.ToDouble(reader["UnitaryValue"]),
                            Quantity = Convert.ToInt32(reader["Quantity"]),
                            LocationName = reader["Name"].ToString()
                        });
                    }
                }
            }
            
            return transactions;
        }

        public bool SaveTransaction(Transaction transaction)
        {
            bool isConnected = true;
            MySqlConnection connection = GetConnection();
            try
            {
                Console.WriteLine("Connecting to database");
                connection.Open();
                double coins;
                if (transaction.Coins == 0 && transaction.UnitaryValue != 0 && transaction.Quantity !=0)
                {
                    coins = transaction.UnitaryValue * transaction.Quantity;
                }
                else
                {
                    coins = transaction.Coins;
                }
                int quantity;
                if (transaction.Quantity == 0 && transaction.UnitaryValue != 0 && transaction.Coins != 0)
                {
                    quantity = (int)Math.Floor(transaction.Coins / transaction.UnitaryValue);
                }
                else
                {
                    quantity = transaction.Quantity;
                }
                MySqlCommand query = new MySqlCommand($"SELECT items.ItemId FROM items WHERE items.ItemName = '{transaction.ItemName}'", connection);
                var reader = query.ExecuteReader();
                reader.Read();
                int itemId = Convert.ToInt32(reader["ItemId"]);
                query = new MySqlCommand($"SELECT locations.LocationId FROM locations WHERE locations.NAME = '{transaction.LocationName}'", connection);
                reader.Close();
                reader = query.ExecuteReader();
                reader.Read();
                int locationId = Convert.ToInt32(reader["LocationId"]);
                reader.Close();

                string sqlcmd = $"INSERT INTO transactions(ItemId, Date, Coins, UnitaryValue, Quantity, LocationId) VALUES(" +
                    $"{itemId}," +
                    $"'{transaction.Date.Year}-{transaction.Date.Month}-{transaction.Date.Day}'," +
                    $"{coins}," +
                    $"{transaction.UnitaryValue}," +
                    $"{quantity}," +
                    $"{locationId})";
                MySqlCommand comm = connection.CreateCommand();
                comm.CommandText = sqlcmd;
                comm.ExecuteNonQuery();
            }
            catch(Exception ex)
            {
                ErrorMessage = ex.ToString();
                isConnected = false;
            }
            connection.Close();
            return isConnected;
        }

        public bool DeleteTransaction(int id)
        {
            MySqlConnection conn = GetConnection();
            try
            {
                conn.Open();
                string sqlcmd = $"DELETE FROM transactions WHERE transactions.TransactionId = {id}";
                MySqlCommand comm = conn.CreateCommand();
                comm.CommandText = sqlcmd;
                comm.ExecuteNonQuery();
                conn.Close();
                return true;
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.ToString();
                return false;
            }

        }

        public List<String> GetItemNames()
        {
            List<string> itemNames = new List<string>();
            MySqlConnection connection = GetConnection();

            try
            {
                Console.WriteLine("Attempting to access database");
                connection.Open();
                MySqlCommand query = new MySqlCommand(
                    "SELECT items.ItemName FROM items ORDER BY items.ItemName", connection);
                MySqlDataReader reader = query.ExecuteReader();
                while (reader.Read())
                {
                    itemNames.Add(reader["ItemName"].ToString());
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            connection.Close();
            return itemNames;
        }

        public List<string> GetLocationNames()
        {
            List<string> locationNames = new List<string>();
            MySqlConnection connection = GetConnection();

            try
            {
                Console.WriteLine("Attempting to access database");
                connection.Open();
                MySqlCommand query = new MySqlCommand(
                    "SELECT locations.Name FROM locations ORDER BY locations.Name", connection);
                MySqlDataReader reader = query.ExecuteReader();
                while (reader.Read())
                {
                    locationNames.Add(reader["Name"].ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            connection.Close();
            return locationNames;
        }

        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }

    }
}
