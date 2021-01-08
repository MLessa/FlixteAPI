using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using System.Security.Cryptography;
using System.Collections;
using System.Data.Common;
using System.Data;
using Dapper;
using MySql.Data.MySqlClient;
using System.Reflection;
using System.Linq;

namespace Database.Common
{
    public abstract class DBDapperComponent
    {
        protected string DefaultConnectionStringKey { get; set; }

        public DBDapperComponent(string DefaultConnectionStringKey = "CN_FASTSOLUTION")
        {
            this.DefaultConnectionStringKey = DefaultConnectionStringKey;
        }

        protected T QuerySingle<T>(string query, Object param = null, string connectionStringName = null, int? commandTimeout = 90)
        {
            connectionStringName = connectionStringName ?? DefaultConnectionStringKey;
            using (MySqlConnection conn = GetConnectionByKey(connectionStringName))
            {
                conn.Open();
                var result = conn.Query<T>(query, param, commandTimeout: commandTimeout).FirstOrDefault();
                conn.Close();
                return result;
            }
        }

        protected bool Execute(string query, Object param = null, string connectionStringName = null, int? commandTimeout = 90, bool enableException = true)
        {
            connectionStringName = connectionStringName ?? DefaultConnectionStringKey;
            try
            {
                using (MySqlConnection conn = GetConnectionByKey(connectionStringName))
                {
                    conn.Open();
                    var result = conn.Execute(query, param, commandTimeout: commandTimeout);
                    conn.Close();
                    return true;
                }
            }
            catch (Exception e)
            {
                if (enableException)
                    throw e;
                return false;
            }
        }

        protected List<T> QueryList<T>(string query, Object param = null, string connectionStringName = null, int? commandTimeout = 90)
        {
            connectionStringName = connectionStringName ?? DefaultConnectionStringKey;
            using (MySqlConnection conn = GetConnectionByKey(connectionStringName))
            {
                conn.Open();
                var result = conn.Query<T>(query, param, commandTimeout: commandTimeout).ToList();
                conn.Close();
                return result;
            }
        }

        protected List<object> QueryMultipleResults(string query, IList<Type> types, object param = null, string connectionStringName = null)
        {
            connectionStringName = connectionStringName ?? DefaultConnectionStringKey;
            try
            {
                using (MySqlConnection conn = GetConnectionByKey(connectionStringName))
                {
                    List<object> result = new List<object>();
                    conn.Open();
                    var multipleResults = conn.QueryMultiple(query, param);

                    foreach (Type type in types)
                    {
                        MethodInfo method = multipleResults.GetType().GetMethods()
                            .Where(m => m.Name == "Read" && m.IsGenericMethodDefinition).FirstOrDefault()
                            .MakeGenericMethod(new Type[] { type });
                        var queryResult = method.Invoke(multipleResults, new object[] { true });
                        result.Add(queryResult);
                    }
                    conn.Close();
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected MySqlConnection GetDefaultConnection()
        {
            return GetConnectionByKey(DefaultConnectionStringKey);
        }

        protected MySqlConnection GetConnectionByKey(string connectionKey)
        {
            string connectionString = ConnectionString.Get(DefaultConnectionStringKey);
            return new MySqlConnection(connectionString);
        }
    }
}
