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
        /// Propriedade que define a chave da string de conexão padrão.
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
        /// Propriedade que dá acesso as informações de conexão.
        /// </summary>
        public DatabaseInfo DatabaseInformation
        {
            get
            {
                return _databaseInfo;
            }
        }

        /// <summary>
        /// Estrutura que contém informacões do database.
        /// </summary>
        public struct DatabaseInfo
        {
            public Database DB;
            public string TableName;
        }

        /// <summary>
        /// Método que obtem informações de conexão e particionamento de tabela (Implementação padrão).
        /// </summary>
        /// <param name="tablePartitionName">Nome da tabela de acesso.</param>
        /// <param name="keyPartitionValue">Chave do particionamento.</param>
        protected virtual void CreateDatabaseInfo(string tableName, string keyPartitionValue)
        {
            string tablePartitionName = tableName;

            // verifica se esta usando ou não particionamento.
            if (keyPartitionValue != null)
            {
                // usando particionamento.
                int hashCode = GetHash(keyPartitionValue);

                // modificando o nome da tabela para colocar o nome correto.
                tablePartitionName = DefineTablePartitionName(tableName, hashCode);

                // definindo o nome padrão para a connection string com particionamento.
                _connectionStringKey = DefineConnectionStringPartitionName(tableName, hashCode);
            }

            // obtem de arquivo de configuracao as connectionsString disponiveis.
            string _connectionStringValue = ConnectionString.Get(_connectionStringKey);
            
            //Hashtable hashConnections = ConfigManager.GetConnectionsConfig();

            // verificando se a connection string é valida e se existe na lista de configurações.
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
        /// Método que obtem informações de conexão e sem particionamento de tabela.
        /// </summary>
        /// <param name="tablePartition">Nome da tabela de acesso.</param>
        protected virtual void CreateDatabaseInfo(string tableName)
        {
            CreateDatabaseInfo(tableName, null);
        }

        /// <summary>
        /// Método que obtem informações de conexão e sem o nome de tabela. Precisa definir o nome da tabela futuramente.
        /// </summary>        
        protected virtual void CreateDatabaseInfo()
        {
            CreateDatabaseInfo(null);
        }

        /// <summary>
        /// Método hash do particionamento. Este método deve ser especializado para diversas formas de particionamento não padrões.
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
        /// Método responsável em informar o número de particões, caso se esteja usando particionamento de tabelas.
        /// </summary>
        /// <returns>Número de partições.</returns>
        protected virtual int DefineNumberPartitions()
        {
            throw new ApplicationException(cNumberPartitionsNotFound);
        }

        /// <summary>
        /// Implementação padrão para definição do nome da tabela de particionamento.(Este método será especializado em diversas situações)
        /// </summary>
        /// <param name="tablePartitionName">Nome da tabela do particionamento. Ex.: Cliente</param>
        /// <param name="hashCode">Código hash do particionamento.</param>
        /// <returns>Nome da tabela do particionamento. Ex: Cliente0 , Cliente1, Cliente2. Para casos especificos sobrescreva este método.</returns>
        protected virtual string DefineTablePartitionName(string tableName, int hashCode)
        {
            return tableName + hashCode.ToString();
        }

        /// <summary>
        /// Implementação padrão para definição da string de conexão padrão para particionamento de tabelas.(Este método será especializado em diversas situações)
        /// </summary>
        /// <param name="tablePartitionName">Nome da tabela do particonamento que irá sera usanda para compor a string de conexão do particionamento. (Uso Opcional)</param>
        /// <returns>Nome da connection string do particionamento.</returns>
        protected virtual string DefineConnectionStringPartitionName(string tableName, int hashCode)
        {
            return cPrefixeConnectionString + tableName + hashCode.ToString();
        }

        /// <summary>
        /// Método utilizado para adicionar parametros no command a partir de um datarow.
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
        /// Propriedade que dá acesso as informações da tabela.
        /// </summary>
        public string TableName
        {
            get { return _databaseInfo.TableName; }
            set { _databaseInfo.TableName = value; }
        }
    }
}
