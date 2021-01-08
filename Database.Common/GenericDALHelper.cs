using System;
using System.Collections.Generic;
using System.Text;

namespace Database.Common
{
    public class GenericDALHelper : DBComponent
    {      
        /// <summary>
        /// Construtor privado para garantir o singleton.
        /// </summary>
        private GenericDALHelper()
        {

        }

        /// <summary>
        /// M�todo singleton que obtem a instancia do DAL gen�rico.
        /// </summary>
        /// <param name="connectionStringKey">Chave da string de conex�o.</param>
        /// <returns>DAL gen�rico.</returns>
        public static GenericDALHelper Create(string connectionStringKey)
        {
            GenericDALHelper genericDALHelper = new GenericDALHelper();

            genericDALHelper.ConnectionStringKey = connectionStringKey;
            genericDALHelper.CreateDatabaseInfo(string.Empty);

            return genericDALHelper;

        }


        public static GenericDALHelper Create(string connectionStringKey, string tableName)
        {
            GenericDALHelper genericDALHelper = new GenericDALHelper();

            genericDALHelper.ConnectionStringKey = connectionStringKey;
            genericDALHelper.CreateDatabaseInfo(tableName);

            return genericDALHelper;

        }
    }
}
