using System;
using System.Data.Common;
using MySql.Data.MySqlClient;

namespace WebApi.Components.Db
{
    public class MySqlDataHelper 
    {
        

        private DbConnection CreateConnection(string connectionStr)
        {
            var connnection = new MySqlConnection(connectionStr);
            connnection.Open();

            return connnection;
        }
    }
}