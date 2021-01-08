using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Microsoft.Practices.EnterpriseLibrary.Data.Oracle;
using System.Data.Common;
using System.Collections;
using Microsoft.Practices.EnterpriseLibrary.Data.MySql;

namespace Database.Common
{
    public static class DatabaseFactory
    {
        public static Database CreateDatabase(string connectionStringWithProvider)
        {
            try
            {
                string prov = "";
                if (ConfigurationManager.AppSettings["provider"] != null)
                    prov = ConfigurationManager.AppSettings["provider"].ToString();
                Microsoft.Practices.EnterpriseLibrary.Data.Database database = null;
                string[] connectionStringWithProviderArray;
                string connectionString = null;
                string provider = "system.data.sqlclient"; //provider default (sql server)

                if (connectionStringWithProvider.IndexOf(",") > 0)
                {
                    // separando a connectionString  do provider.
                    connectionStringWithProviderArray = connectionStringWithProvider.Split(",".ToCharArray());

                    connectionString = connectionStringWithProviderArray[1];
                    provider = connectionStringWithProviderArray[0];
                }
                else
                {
                    connectionString = connectionStringWithProvider;
                    
                }
                if (prov.IndexOf("mysql") != -1)
                    provider = "mysql.data.mysqlclient";
                switch (provider.ToLower())
                {
                    case "system.data.sqlclient":
                        database = new SqlDatabase(connectionString);
                        break;
                    case "system.data.oracleclient":
                        database = new OracleDatabase(connectionString);
                        break;
                    case "mysql.data.mysqlclient":
                        database = new MySqlDatabase(connectionString);
                        break;
                    default:
                        database = new SqlDatabase(connectionString); //GenericDatabase
                        break;
                }
                
                //cria o database responsavel em gerenciar a transação
                return new Database(database);
            }
            catch (Exception exception)
            {
                TryLogError(exception, connectionStringWithProvider);
                throw;
            }
        }

        private static void TryLogError(Exception exception, string instanceName)
        {
            try
            {
                DefaultDataEventLogger eventLogger = EnterpriseLibraryFactory.BuildUp<DefaultDataEventLogger>();
                if (eventLogger != null)
                {
                    eventLogger.LogConfigurationError(exception, instanceName);
                }
            }
            catch { }
        }
    }
}
