using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using WebApi.Models;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Components.Db
{
    public static class MySqlDataHelper 
    {
        private static DbConnection CreateConnection(string connectionStr = null)
        {
            if (string.IsNullOrEmpty(connectionStr))
            {
                connectionStr = HelperConfig.Current.InternalDb;
            }

            var connnection = new MySqlConnection(connectionStr);

            return connnection;
        }
    }
}