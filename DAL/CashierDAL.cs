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
                    MySqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = "select * from Cashier where userName='" + cashier.UserName + "' and password='" + Md5Algorithms.CreateMD5(cashier.Password) + "';";
                    MySqlDataReader reader = cmd.ExecuteReader();
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
        public Cashier GetByID(int cashier_id){
            Cashier cashier = null;
            try{
                connection.Open();
                MySqlCommand cmd = connection.CreateCommand();
                string query = "select *from Cashier where cashierId = '"+cashier_id+"';";

                cmd.CommandText = query;
                MySqlDataReader reader = cmd.ExecuteReader();
                if(reader.Read()){
                    cashier = new Cashier();
                    cashier.CashierId = reader.GetInt32("cashierId");
                    cashier.FirstName = reader.GetString("firstName");
                    cashier.MiddleName = reader.GetString("midlleName");
                    cashier.LastName = reader.GetString("lastName");
                    cashier.Phone = reader.GetString("phone");
                    cashier.Email = reader.GetString("email");
                    cashier.Address = reader.GetString("address");
                }
                reader.Close();
            }catch(Exception ex){
                Console.WriteLine(ex);
            }finally{
                connection.Close();
            }
            return cashier;
        }
        public bool AddCashier(Cashier cashier){
            if(cashier == null){
                throw new Exception("Cashier is null!");
            }
            bool result = false;
            try{
                connection.Open();
                MySqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "lock tables Cashier write;";
                cmd.ExecuteNonQuery();
                MySqlTransaction trans = connection.BeginTransaction();
                MySqlDataReader reader = null;
                cmd.Transaction = trans;

                try{
                    // check user name is exists
                    cmd.CommandText = "select userName from Cashier where userName = '"+cashier.UserName+"';";
                    reader = cmd.ExecuteReader();
                    if(reader.Read()){
                        reader.Close();
                        throw new Exception("Tên đăng nhập đã tồn tại!");
                    }
                    reader.Close();
                    cmd.CommandText = @"insert into Cashier(userName, password, role, firstName, midlleName, lastName, phone, email, address) value
                                (ifnull(@userName, ''), ifnull(@password, ''), 2, ifnull(@first, ''), ifnull(@middle, ''), ifnull(@last, ''), ifnull(@phone, ''), ifnull(@email, ''), ifnull(@address, ''));";

                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@userName", cashier.UserName);
                    cmd.Parameters.AddWithValue("@password", Md5Algorithms.CreateMD5(cashier.Password));
                    cmd.Parameters.AddWithValue("@first", cashier.FirstName);
                    cmd.Parameters.AddWithValue("@middle", cashier.MiddleName);
                    cmd.Parameters.AddWithValue("@last", cashier.LastName);
                    cmd.Parameters.AddWithValue("@phone", cashier.Phone);
                    cmd.Parameters.AddWithValue("@email", cashier.Email);
                    cmd.Parameters.AddWithValue("@address", cashier.Address);
                    cmd.ExecuteNonQuery();
                    trans.Commit();
                    result = true;
                }catch(Exception e){
                    try{
                        trans.Rollback();
                    }catch{}
                    throw new Exception(e.Message);
                }finally{
                    cmd.CommandText = "unlock tables;";
                    cmd.ExecuteNonQuery();
                }
            }catch(Exception ex){
                throw new Exception(ex.Message);
            }finally{
                connection.Close();
            }
            return result;
        }
    }
}