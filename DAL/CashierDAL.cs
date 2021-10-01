using System;
using Persistance;
using MySql.Data.MySqlClient;

namespace DAL
{
    public class CashierDAL
    {
        private MySqlConnection connection = DbHelper.GetConnection();
        public Cashier Login(Cashier cashier)
        {
            lock (connection)
            {
                try
                {
                    connection.Open();
                    string query = "select * from Cashier where userName='" + cashier.UserName + "' and password='" + Md5Algorithms.CreateMD5(cashier.Password) + "';";
                    MySqlDataReader reader = DbHelper.ExecQuery(query);
                    if (reader.Read())
                    {
                        cashier.CashierId = reader.GetInt32("cashierId");
                        cashier.Role = reader.GetInt32("role");
                        cashier.FirstName = reader.GetString("firstName");
                        cashier.MiddleName = reader.GetString("midlleName");
                        cashier.LastName = reader.GetString("lastName");
                        cashier.Phone = reader.GetString("phone");
                        cashier.Email = reader.GetString("email");
                        cashier.Address = reader.GetString("address");
                    }
                    else
                    {
                        cashier.Role = 0;
                    }
                    reader.Close();
                }
                catch
                {
                    cashier.Role = -1;
                }
                finally
                {
                    connection.Close();
                }
            }
            return cashier;
        }
    }
}