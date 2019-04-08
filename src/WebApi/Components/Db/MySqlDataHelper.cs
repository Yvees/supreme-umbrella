using System;
using MySql.Data.MySqlClient;

namespace WebApi.Components.Db
{
    public class MySqlDataHelper 
    {
        

        private MySqlConnection CreateConnection(string connectionStr)
        {
            MySqlConnection connnection = new MySqlConnection(connectionStr);
            connnection.Open();

            return connnection;
        }
    }
}