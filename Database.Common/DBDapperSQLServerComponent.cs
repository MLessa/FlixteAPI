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
using System.Data.SqlClient;

namespace Database.Common
{
    public abstract class DBDapperSQLServerComponent
    {
        protected string DefaultConnectionStringKey { get; set; }

        public DBDapperSQLServerComponent(string DefaultConnectionStringKey = "CN_FASTSOLUTION")
        {
            this.DefaultConnectionStringKey = DefaultConnectionStringKey;
        }

        protected T QuerySingle<T>(string query, Object param = null, string connectionStringName = null, int? commandTimeout = null)
        {
            connectionStringName = connectionStringName ?? DefaultConnectionStringKey;
            using (SqlConnection conn = GetConnectionByKey(connectionStringName))
            {
                conn.Open();
                var result = conn.Query<T>(query, param, commandTimeout: commandTimeout);
                conn.Close();
                return result.FirstOrDefault();
            }
        }

        protected bool Execute(string query, Object param = null, string connectionStringName = null, int? commandTimeout = null, bool enableException = true)
        {
            connectionStringName = connectionStringName ?? DefaultConnectionStringKey;
            try
            {
                using (SqlConnection conn = GetConnectionByKey(connectionStringName))
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

        protected List<T> QueryList<T>(string query, Object param = null, string connectionStringName = null, int? commandTimeout = null)
        {
            connectionStringName = connectionStringName ?? DefaultConnectionStringKey;
            using (SqlConnection conn = GetConnectionByKey(connectionStringName))
            {
                conn.Open();
                var result = conn.Query<T>(query, param, commandTimeout: commandTimeout).ToList();
                conn.Close();
                return result;
            }
        }

        protected List<List<object>> QueryMultipleResults(string query, IList<Type> types, object param = null, string connectionStringName = null)
        {
            connectionStringName = connectionStringName ?? DefaultConnectionStringKey;
            try
            {
                using (SqlConnection conn = GetConnectionByKey(connectionStringName))
                {
                    List<List<object>> result = new List<List<object>>();
                    conn.Open();
                    using (var multi = conn.QueryMultiple(query, param))
                    {
                        foreach (Type type in types)
                        {
                            MethodInfo method = multi.GetType().GetMethods()
                                .Where(m => m.Name == "Read" && m.IsGenericMethodDefinition).FirstOrDefault()
                                .MakeGenericMethod(new Type[] { type });
                            var queryResult = (IEnumerable<dynamic>)method.Invoke(multi, new object[] { true });
                            result.Add(queryResult.ToList());
                        }
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

        protected SqlConnection GetDefaultConnection()
        {
            return GetConnectionByKey(DefaultConnectionStringKey);
        }

        protected SqlConnection GetConnectionByKey(string connectionKey)
        {
            string connectionString = ConnectionString.Get(DefaultConnectionStringKey);
            return new SqlConnection(connectionString);
        }
    }
}
