using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Components.Manager
{
    public static class DbEntityManager
    {
        public static async Task<object> Insert<T>(T entity)
        {
            string sql = GenInsertSql<T>(entity);
            return await MySqlHelper.ExecuteScalarAsync(HelperConfig.Current.InternalDb, sql);
        }

        private static string GenInsertSql<T>(T entity)
        {
            List<string> lstCols = new List<string>();
            List<string> lstVals = new List<string>();

            var columns = typeof(T).GetProperties();
            foreach (var column in columns)
            {
                var typeCol = column.PropertyType;
                bool isKey = column.GetCustomAttributes(typeof(KeyAttribute), true).Length > 0;
                string val = column.GetValue(entity).ToString();
                if (!isKey)
                {
                    if (typeCol == typeof(string))
                    {
                        lstVals.Add($"\"{val}\"");
                    }
                    else if (typeCol == typeof(int))
                    {
                        lstVals.Add($"{val}");
                    }

                    lstCols.Add(column.Name);
                }
            }

            return $"insert into {typeof(T).Name} ( {string.Join(",", lstCols)} ) values( {string.Join(",", lstVals)} )";
        }
    }
}
