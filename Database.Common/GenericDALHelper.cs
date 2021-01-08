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
        /// Método singleton que obtem a instancia do DAL genérico.
        /// </summary>
        /// <param name="connectionStringKey">Chave da string de conexão.</param>
        /// <returns>DAL genérico.</returns>
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
