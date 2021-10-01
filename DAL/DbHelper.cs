using System;
using MySql.Data.MySqlClient;

namespace DAL
{
    public class DbHelper
    {
        private static MySqlConnection connection;
        public static MySqlConnection GetConnection()
        {
            if (connection == null)
            {
                connection = new MySqlConnection
                {
                    ConnectionString = "server=localhost;user id=PF13vtca;password=tienvtca;port=3306;database=salessystem;"
                };
            }
            return connection;
        }

        public static MySqlDataReader ExecQuery(string query){
            MySqlCommand command = new MySqlCommand(query, connection);
            return command.ExecuteReader();
        }

        private DbHelper(){}
    }
}
