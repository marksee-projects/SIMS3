using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SIMS3
{
    public class DBConnect
    {
        // The database Connection
        private MySqlConnection connection = new MySqlConnection("Server=127.0.0.1; Port=3306; Database=infomanagement_db; Uid=root; Pwd=;");

        // 1. Method to get the connection
        public MySqlConnection GetConnection()
        {
            return connection;
        }

        // 2. Method to open the connection
        public void openConnect()
        {
            // It's a good practice to check if it's already open before opening it!
            if (connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }
        }

        // 3. Method to close the connection (Highly recommended to have this!)
        public void closeConnect()
        {
            if (connection.State == System.Data.ConnectionState.Open)
            {
                connection.Close();
            }
        }
    }
}
