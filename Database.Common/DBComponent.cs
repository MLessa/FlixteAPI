using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using System.Security.Cryptography;
using System.Collections;
using System.Data.Common;
using System.Data;

namespace Database.Common
{
    public abstract class DBComponent
    {
        public const string cPrefixeConnectionString = "CN_";
        public const string cConnectionStringNotFound = "Connection string key not found";
        public const string cNumberPartitionsNotFound = "Number partitions not defined";

        protected DatabaseInfo _databaseInfo;
        protected string _connectionStringKey = string.Empty;

        /// <summary>
        /// Propriedade que define a chave da string de conex�o padr�o.
        /// </summary>
        protected string ConnectionStringKey
        {
            get
            {
                return _connectionStringKey;
            }
            set
            {
                _connectionStringKey = value;
            }
        }

        /// <summary>
        /// Propriedade que d� acesso as informa��es de conex�o.
        /// </summary>
        public DatabaseInfo DatabaseInformation
        {
            get
            {
                return _databaseInfo;
            }
        }

        /// <summary>
        /// Estrutura que cont�m informac�es do database.
        /// </summary>
        public struct DatabaseInfo
        {
            public Database DB;
            public string TableName;
        }

        /// <summary>
        /// M�todo que obtem informa��es de conex�o e particionamento de tabela (Implementa��o padr�o).
        /// </summary>
        /// <param name="tablePartitionName">Nome da tabela de acesso.</param>
        /// <param name="keyPartitionValue">Chave do particionamento.</param>
        protected virtual void CreateDatabaseInfo(string tableName, string keyPartitionValue)
        {
            string tablePartitionName = tableName;

            // verifica se esta usando ou n�o particionamento.
            if (keyPartitionValue != null)
            {
                // usando particionamento.
                int hashCode = GetHash(keyPartitionValue);

                // modificando o nome da tabela para colocar o nome correto.
                tablePartitionName = DefineTablePartitionName(tableName, hashCode);

                // definindo o nome padr�o para a connection string com particionamento.
                _connectionStringKey = DefineConnectionStringPartitionName(tableName, hashCode);
            }

            // obtem de arquivo de configuracao as connectionsString disponiveis.
            string _connectionStringValue = ConnectionString.Get(_connectionStringKey);
            
            //Hashtable hashConnections = ConfigManager.GetConnectionsConfig();

            // verificando se a connection string � valida e se existe na lista de configura��es.
            //if ((_connectionStringKey != null && _connectionStringKey == string.Empty) || (!hashConnections.ContainsKey(_connectionStringKey)))
                //throw new ApplicationException(cConnectionStringNotFound);
            if (string.IsNullOrEmpty(_connectionStringValue))
                throw new ApplicationException(cConnectionStringNotFound);

            // criando o objeto database do DAAB.
            _databaseInfo.DB = DatabaseFactory.CreateDatabase(_connectionStringValue);

            // colocando o nome da tabela de acesso.
            _databaseInfo.TableName = tablePartitionName;
        }

        /// <summary>
        /// M�todo que obtem informa��es de conex�o e sem particionamento de tabela.
        /// </summary>
        /// <param name="tablePartition">Nome da tabela de acesso.</param>
        protected virtual void CreateDatabaseInfo(string tableName)
        {
            CreateDatabaseInfo(tableName, null);
        }

        /// <summary>
        /// M�todo que obtem informa��es de conex�o e sem o nome de tabela. Precisa definir o nome da tabela futuramente.
        /// </summary>        
        protected virtual void CreateDatabaseInfo()
        {
            CreateDatabaseInfo(null);
        }

        /// <summary>
        /// M�todo hash do particionamento. Este m�todo deve ser especializado para diversas formas de particionamento n�o padr�es.
        /// </summary>
        /// <param name="keyPartitionValue">Chave do particionamento.</param>
        /// <returns>Resultado do calculo do hash.</returns>
        protected virtual int GetHash(string keyPartitionValue)
        {
            MD5 md5Hasher = MD5.Create();
            byte[] data = md5Hasher.ComputeHash(Encoding.ASCII.GetBytes(keyPartitionValue));
            int value = data[0] * 16 + data[data.Length - 1];
            return (value % DefineNumberPartitions()) + 1;
        }

        /// <summary>
        /// M�todo respons�vel em informar o n�mero de partic�es, caso se esteja usando particionamento de tabelas.
        /// </summary>
        /// <returns>N�mero de parti��es.</returns>
        protected virtual int DefineNumberPartitions()
        {
            throw new ApplicationException(cNumberPartitionsNotFound);
        }

        /// <summary>
        /// Implementa��o padr�o para defini��o do nome da tabela de particionamento.(Este m�todo ser� especializado em diversas situa��es)
        /// </summary>
        /// <param name="tablePartitionName">Nome da tabela do particionamento. Ex.: Cliente</param>
        /// <param name="hashCode">C�digo hash do particionamento.</param>
        /// <returns>Nome da tabela do particionamento. Ex: Cliente0 , Cliente1, Cliente2. Para casos especificos sobrescreva este m�todo.</returns>
        protected virtual string DefineTablePartitionName(string tableName, int hashCode)
        {
            return tableName + hashCode.ToString();
        }

        /// <summary>
        /// Implementa��o padr�o para defini��o da string de conex�o padr�o para particionamento de tabelas.(Este m�todo ser� especializado em diversas situa��es)
        /// </summary>
        /// <param name="tablePartitionName">Nome da tabela do particonamento que ir� sera usanda para compor a string de conex�o do particionamento. (Uso Opcional)</param>
        /// <returns>Nome da connection string do particionamento.</returns>
        protected virtual string DefineConnectionStringPartitionName(string tableName, int hashCode)
        {
            return cPrefixeConnectionString + tableName + hashCode.ToString();
        }

        /// <summary>
        /// M�todo utilizado para adicionar parametros no command a partir de um datarow.
        /// </summary>
        /// <param name="dbCommand">Command</param>
        /// <param name="collectionValues">DataRow</param>
        protected void AddCommandParameters(DbCommand dbCommand, DataRow collectionValues)
        {
            if (dbCommand != null && collectionValues != null)
            {
                DataColumnCollection dataColumnCollection = collectionValues.Table.Columns;
                foreach (DataColumn dataColumn in dataColumnCollection)
                {
                    DbParameter parameter = dbCommand.CreateParameter();
                    parameter.ParameterName = dataColumn.ColumnName;
                    parameter.Value = collectionValues[dataColumn];
                    dbCommand.Parameters.Add(parameter);
                }
            }
        }

        /// <summary>
        /// Propriedade que d� acesso as informa��es da tabela.
        /// </summary>
        public string TableName
        {
            get { return _databaseInfo.TableName; }
            set { _databaseInfo.TableName = value; }
        }
    }
}
