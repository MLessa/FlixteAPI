using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace Database.Common
{
    public class ConnectionString
    {
        public static string Get(string connectionStringKey)
        {
            string connectionStringValue = ConfigurationManager.AppSettings[connectionStringKey].ToString();
            if (!string.IsNullOrEmpty(connectionStringValue))
                connectionStringValue = FastSolution.Common.Util.SecurityManager.Decrypt(connectionStringValue, connectionStringKey);

            return connectionStringValue;
        }
    }
}
