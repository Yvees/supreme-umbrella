using Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Components.Manager
{
    public static class DbEntityManager
    {
        public static async Task<int> Insert<T>(T entity)
        {
            string sql = GenInsertSql(entity);
            return await MySqlHelper.ExecuteNonQueryAsync(HelperConfig.Current.InternalDb, sql);
        }

        public static async Task<bool> Exist<T>(string key, object keyVal)
        {
            string sql = GenExistSql<T>(key, keyVal);

            object result = await MySqlHelper.ExecuteScalarAsync(HelperConfig.Current.InternalDb, sql);
            return result.ToString() != "0";
        }

        public static async Task<int> Update<T>(T entity, string key, object keyVal)
        {
            string sql = GenUpdateSql(entity, key, keyVal);

            return await MySqlHelper.ExecuteNonQueryAsync(HelperConfig.Current.InternalDb, sql);
        }

        public static async Task<int> Update<T>(T entity)
        {
            string sql = GenUpdateSql(entity);

            return await MySqlHelper.ExecuteNonQueryAsync(HelperConfig.Current.InternalDb, sql);
        }

        public static async Task<T> SelectOne<T>(string key, object keyVal)
        {
            string sql = GenSelectSql<T>(key, keyVal);
            using (var connection = new MySqlConnection(HelperConfig.Current.InternalDb))
            {
                var result = await connection.QueryAsync<T>(sql);
                return result.SingleOrDefault();
            }
        }

        public static async Task<IEnumerable<T>> Select<T>(string key, object keyVal)
        {
            string sql = GenSelectSql<T>(key, keyVal);
            using (var connection = new MySqlConnection(HelperConfig.Current.InternalDb))
            {
                return await connection.QueryAsync<T>(sql);
            }
        }

        #region Private Methods
        private static string GenSelectSql<T>(string key, object keyVal)
        {
            string sql = $"select * from {typeof(T).Name} where {key}={FormatValue(keyVal)}";
            Console.WriteLine(sql);
            return sql;
        }

        private static string GenInsertSql<T>(T entity)
        {
            List<string> lstCols = new List<string>();
            List<string> lstVals = new List<string>();

            var columns = typeof(T).GetProperties();
            foreach (var column in columns)
            {
                bool isKey = column.GetCustomAttributes(typeof(KeyAttribute), true).Length > 0;
                object val = column.GetValue(entity);
                if (!isKey)
                {
                    lstVals.Add(FormatValue(val));
                    lstCols.Add(column.Name);
                }
            }

            string sql = $"insert into {typeof(T).Name} ( {string.Join(",", lstCols)} ) values( {string.Join(",", lstVals)} )";
            Console.WriteLine(sql);
            return sql;
        }

        private static string GenExistSql<T>(string key, object keyVal)
        {
            string sql = $"select count(1) from {typeof(T).Name} where {key} = {FormatValue(keyVal)}";
            Console.WriteLine(sql);
            return sql;
        }

        private static string GenUpdateSql<T>(T entity, string key, object keyVal)
        {
            Dictionary<string, string> dicColVal = new Dictionary<string, string>();

            var columns = typeof(T).GetProperties();
            foreach (var column in columns)
            {
                bool isKey = column.GetCustomAttributes(typeof(KeyAttribute), true).Length > 0;

                if (column.Name != key && !isKey)
                {
                    dicColVal.Add(column.Name, FormatValue(column.GetValue(entity)));
                }
            }

            string strUpdate = string.Empty;
            foreach (var col in dicColVal.Keys)
            {
                strUpdate += $"{col}={dicColVal[col]},";
            }

            string sql = $"update {typeof(T).Name} set {strUpdate.Substring(0, strUpdate.Length - 1)} where {key} = {FormatValue(keyVal)}";
            Console.WriteLine(sql);
            return sql;
        }

        private static string GenUpdateSql<T>(T entity)
        {
            Dictionary<string, string> dicColVal = new Dictionary<string, string>();
            string key = string.Empty;
            object keyVal = null;
            var columns = typeof(T).GetProperties();
            foreach (var column in columns)
            {
                bool isKey = column.GetCustomAttributes(typeof(KeyAttribute), true).Length > 0;

                if (!isKey)
                {
                    dicColVal.Add(column.Name, FormatValue(column.GetValue(entity)));
                }
                else
                {
                    key = column.Name;
                    keyVal = column.GetValue(entity);
                }
            }

            string strUpdate = string.Empty;
            foreach (var col in dicColVal.Keys)
            {
                strUpdate += $"{col}={dicColVal[col]},";
            }

            string sql = $"update {typeof(T).Name} set {strUpdate.Substring(0, strUpdate.Length - 1)} where {key} = {FormatValue(keyVal)}";
            Console.WriteLine(sql);
            return sql;
        }

        private static string FormatValue(object val)
        {
            if (val is string)
                return $"\"{val}\"";
            else if (val is int || val is long || val is float || val is double || val is decimal)
                return $"{val}";
            else if (val is bool)
            {
                return $"{val.ToString().ToLower()}";
            }
            else
                throw new NotSupportedException("Value Type Not Supported.");
        }

        [Obsolete("Use dapper instead.")]
        private static async Task<List<ArrayList>> RetrieveReader(DbDataReader reader)
        {
            var result = new List<ArrayList>();

            if (reader.HasRows)
            {
                while (await reader.ReadAsync())
                {
                    var row = new ArrayList();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        row.Add(reader.GetValue(i));
                    }

                    result.Add(row);
                }
            }

            return result;
        }

        #endregion
    }
}
