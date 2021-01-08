using System;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Database.Common.Transactions;
using System.Data.Common;
using System.IO;

namespace Database.Common
{
    /// <summary>
    /// Classe wrapper para a classe Database do DAAB, tornando-a "transaction aware"
    /// </summary>
    public class Database
    {
        private Microsoft.Practices.EnterpriseLibrary.Data.Database databaseInstance;
        private ITransactionHandler handler;

        public Database(Microsoft.Practices.EnterpriseLibrary.Data.Database database)
            : this(database, null)
        {
        }

        public Database(Microsoft.Practices.EnterpriseLibrary.Data.Database database, ITransactionHandler handler)
        {
            if (database == null)
            {
                throw new ArgumentNullException("database", "Parâmetro não pode ser nulo");
            }

            if (handler == null)
            {
                TransactionHandler = TransactionHandlerFactory.CreateDefault();
            }

            DatabaseInstance = database;
        }

        private Microsoft.Practices.EnterpriseLibrary.Data.Database DatabaseInstance
        {
            get { return databaseInstance; }
            set { databaseInstance = value; }
        }

        private ITransactionHandler TransactionHandler
        {
            get { return handler; }
            set { handler = value; }
        }

        private bool IsTransactionCreated
        {
            get
            {
                if (TransactionHandler.IsContextCreated)
                {

                    if (TransactionHandler.Current.Terminated)
                    {
                        TransactionHandler.Current.Vote = TransactionVote.Rollback;
                        throw new TransactionException("Não é possível realizar operação. O método complete() já foi invocado na transação raiz");
                    }
                    else
                    {
                        return true;

                    }

                }

                return false;

            }
        }    

        public DataSet ExecuteDataSet(DbCommand command)
        {
            DataSet datst = null;
            if ((databaseInstance.ConnectionStringWithoutCredentials.ToLower().IndexOf("mysql") != -1)|| (System.Configuration.ConfigurationManager.AppSettings["provider"]=="mysql"))
            {
                command.CommandText = command.CommandText.Replace("@", "?");
                command.CommandText = command.CommandText.Replace("with", " ");
                command.CommandText = command.CommandText.Replace("(nolock)", " ");
                command.CommandText = command.CommandText.Replace("nolock", " ");
            }
            if (IsTransactionCreated)
            {
                                  
                    datst = DatabaseInstance.ExecuteDataSet(command, TransactionHandler.Current.CurrentDbTransaction);                    
                
            }
            else
            {
                try
                {
                    datst = DatabaseInstance.ExecuteDataSet(command);
                }                
                catch(Exception ex) {
                    try
                    {

                        if (System.Configuration.ConfigurationManager.AppSettings["enabledAutoReconect"]==null && command.Connection != null && (command.Connection.State == ConnectionState.Closed || ex.Message.ToLower().IndexOf("foi forçado o cancelamento de uma conexão existente")!=-1))
                            command.Connection.Open();
                    }
                    catch { }
                    datst = DatabaseInstance.ExecuteDataSet(command);
                }
                
            }
            return datst;
        }
        public DataSet ExecuteDataSet(CommandType commandType, string commandText)
        {
            DataSet datst = null;
            if ((databaseInstance.ConnectionStringWithoutCredentials.ToLower().IndexOf("mysql") != -1)|| (System.Configuration.ConfigurationManager.AppSettings["provider"]=="mysql"))
            {
                commandText = commandText.Replace("@", "?");
                commandText = commandText.Replace("with", " ");
                commandText = commandText.Replace("(nolock)", " ");
                commandText = commandText.Replace("nolock", " ");
            }
            if (IsTransactionCreated)
            {
                
                    datst = DatabaseInstance.ExecuteDataSet(TransactionHandler.Current.CurrentDbTransaction, commandType, commandText);
                
            }
            else
            {
                
                    datst = DatabaseInstance.ExecuteDataSet(commandType, commandText);
                
            }
            return datst;
        }

        public DataSet ExecuteDataSet(string storedProcedureName, params object[] parameterValues)
        {
            DataSet datst = null;
            if (IsTransactionCreated)
            {
                
                    datst = DatabaseInstance.ExecuteDataSet(TransactionHandler.Current.CurrentDbTransaction, storedProcedureName, parameterValues);
                
            }
            else
            {                
                    datst = DatabaseInstance.ExecuteDataSet(storedProcedureName, parameterValues);                
            }
            return datst;
        }

        public int ExecuteNonQuery(DbCommand command)
        {
            int intResult = int.MinValue;
            if ((databaseInstance.ConnectionStringWithoutCredentials.ToLower().IndexOf("mysql") != -1)|| (System.Configuration.ConfigurationManager.AppSettings["provider"]=="mysql"))
            {
                command.CommandText = command.CommandText.Replace("@", "?");
                command.CommandText = command.CommandText.Replace("with", " ");
                command.CommandText = command.CommandText.Replace("(nolock)", " ");
                command.CommandText = command.CommandText.Replace("nolock", " ");
            }
            if (IsTransactionCreated)
            {

                intResult = DatabaseInstance.ExecuteNonQuery(command, TransactionHandler.Current.CurrentDbTransaction);
                #region LogDB
                try
                {
                    string pathLog = GetPathLogDB();
                    if (!string.IsNullOrEmpty(pathLog))
                    {
                        System.IO.FileStream file = System.IO.File.Open(pathLog + DateTime.Now.ToString("yyyyMMdd") + ".sql", FileMode.Append);
                        StreamWriter writer = new StreamWriter(file);
                        string commandText = GetCommandTextCompleteFromDBCommand(command);
                        writer.WriteLine(commandText.IndexOf(";") == -1 ? commandText + ";" : commandText);
                        writer.Flush();
                        file.Close();
                    }
                }
                catch { }
                #endregion
            }
            else
            {
                try
                {
                    intResult = DatabaseInstance.ExecuteNonQuery(command);
                    #region LogDB
                    try
                    {
                        string pathLog = GetPathLogDB();
                        if (!string.IsNullOrEmpty(pathLog))
                        {
                            System.IO.FileStream file = System.IO.File.Open(pathLog + DateTime.Now.ToString("yyyyMMdd") + ".sql", FileMode.Append);
                            StreamWriter writer = new StreamWriter(file);
                            string commandText = GetCommandTextCompleteFromDBCommand(command);
                            writer.WriteLine(commandText.IndexOf(";") == -1 ? commandText + ";" : commandText);
                            writer.Flush();
                            file.Close();
                        }
                    }
                    catch { }
                    #endregion
                }
                catch (Exception ex)
                {
                    try
                    {
                        if (System.Configuration.ConfigurationManager.AppSettings["enabledAutoReconect"] == null && command.Connection != null && (command.Connection.State == ConnectionState.Closed || ex.Message.ToLower().IndexOf("foi forçado o cancelamento de uma conexão existente") != -1))
                            command.Connection.Open();
                    }
                    catch { }
                    intResult = DatabaseInstance.ExecuteNonQuery(command);
                }

            }
            return intResult;
        }


        public int ExecuteNonQuery(CommandType commandType, string commandText)
        {
            int intResult = int.MinValue;
            if ((databaseInstance.ConnectionStringWithoutCredentials.ToLower().IndexOf("mysql") != -1)|| (System.Configuration.ConfigurationManager.AppSettings["provider"]=="mysql"))
            {
                commandText = commandText.Replace("@", "?");
                commandText = commandText.Replace("with", " ");
                commandText = commandText.Replace("(nolock)", " ");
                commandText = commandText.Replace("nolock", " ");
            }
            if (IsTransactionCreated)
            {

                intResult = DatabaseInstance.ExecuteNonQuery(TransactionHandler.Current.CurrentDbTransaction, commandType, commandText);
                #region LogDB
                try
                {
                    string pathLog = GetPathLogDB();
                    if (!string.IsNullOrEmpty(pathLog))
                    {
                        System.IO.FileStream file = System.IO.File.Open(pathLog + DateTime.Now.ToString("yyyyMMdd") + ".sql", FileMode.Append);
                        StreamWriter writer = new StreamWriter(file);
                        writer.WriteLine(commandText.IndexOf(";") == -1 ? commandText + ";" : commandText);
                        writer.Flush();
                        file.Close();
                    }
                }
                catch { }
                #endregion
            }
            else
            {

                intResult = DatabaseInstance.ExecuteNonQuery(commandType, commandText);
                #region LogDB
                try
                {
                    string pathLog = GetPathLogDB();
                    if (!string.IsNullOrEmpty(pathLog))
                    {
                        System.IO.FileStream file = System.IO.File.Open(pathLog + DateTime.Now.ToString("yyyyMMdd") + ".sql", FileMode.Append);
                        StreamWriter writer = new StreamWriter(file);
                        writer.WriteLine(commandText.IndexOf(";") == -1 ? commandText + ";" : commandText);
                        writer.Flush();
                        file.Close();
                    }
                }
                catch { }
                #endregion
            }
            return intResult;
        }

        public int ExecuteNonQuery(string storedProcedureName, params object[] parameterValues)
        {
            int intResult = int.MinValue;
            if (IsTransactionCreated)
            {
                
                    intResult = DatabaseInstance.ExecuteNonQuery(TransactionHandler.Current.CurrentDbTransaction, storedProcedureName, parameterValues);
                
            }
            else
            {
                
                    intResult = DatabaseInstance.ExecuteNonQuery(storedProcedureName, parameterValues);
                
            }
            return intResult;
        }

        public IDataReader ExecuteReader(DbCommand command)
        {
            IDataReader datrdr = null;
            if ((databaseInstance.ConnectionStringWithoutCredentials.ToLower().IndexOf("mysql") != -1)|| (System.Configuration.ConfigurationManager.AppSettings["provider"]=="mysql"))
            {
                command.CommandText = command.CommandText.Replace("@", "?");
                command.CommandText = command.CommandText.Replace("with", " ");
                command.CommandText = command.CommandText.Replace("(nolock)", " ");
                command.CommandText = command.CommandText.Replace("nolock", " ");
            }
            if (IsTransactionCreated)
            {
                
                    datrdr = DatabaseInstance.ExecuteReader(command, TransactionHandler.Current.CurrentDbTransaction);
                
            }
            else
            {
                try{
                    datrdr = DatabaseInstance.ExecuteReader(command);
                }
                catch (Exception ex)
                {
                    try{
                    if (System.Configuration.ConfigurationManager.AppSettings["enabledAutoReconect"]==null && command.Connection != null && (command.Connection.State == ConnectionState.Closed || ex.Message.ToLower().IndexOf("foi forçado o cancelamento de uma conexão existente")!=-1))
                        command.Connection.Open();
                    }
                    catch { }
                    datrdr = DatabaseInstance.ExecuteReader(command);
                }
                
            }
            return datrdr;
        }

        public IDataReader ExecuteReader(CommandType commandType, string commandText)
        {
            IDataReader datrdr = null;
            if ((databaseInstance.ConnectionStringWithoutCredentials.ToLower().IndexOf("mysql") != -1)|| (System.Configuration.ConfigurationManager.AppSettings["provider"]=="mysql"))
            {
                commandText = commandText.Replace("@", "?");
                commandText = commandText.Replace("with", " ");
                commandText = commandText.Replace("(nolock)", " ");
                commandText = commandText.Replace("nolock", " ");
            }
            if (IsTransactionCreated)
            {
                
                    datrdr = DatabaseInstance.ExecuteReader(TransactionHandler.Current.CurrentDbTransaction, commandType, commandText);
                
            }
            else
            {
                
                    datrdr = DatabaseInstance.ExecuteReader(commandType, commandText);
                
            }
            return datrdr;
        }

        public IDataReader ExecuteReader(string storedProcedureName, params object[] parameterValues)
        {
            IDataReader datrdr = null;
            if (IsTransactionCreated)
            {
                
                    datrdr = DatabaseInstance.ExecuteReader(TransactionHandler.Current.CurrentDbTransaction, storedProcedureName, parameterValues);
                
            }
            else
            {
                
                    datrdr = DatabaseInstance.ExecuteReader(storedProcedureName, parameterValues);
                
            }
            return datrdr;
        }

        public object ExecuteScalar(DbCommand command)
        {
            object objResult = null;
            if ((databaseInstance.ConnectionStringWithoutCredentials.ToLower().IndexOf("mysql") != -1)|| (System.Configuration.ConfigurationManager.AppSettings["provider"]=="mysql"))
            {
                command.CommandText = command.CommandText.Replace("@", "?");
                command.CommandText = command.CommandText.Replace("with", " ");
                command.CommandText = command.CommandText.Replace("(nolock)", " ");
                command.CommandText = command.CommandText.Replace("nolock", " ");
            }
            if (IsTransactionCreated)
            {
                
                    objResult = DatabaseInstance.ExecuteScalar(command, TransactionHandler.Current.CurrentDbTransaction);
                    #region LogDB
                    try
                    {
                        string pathLog = GetPathLogDB();
                        if (!string.IsNullOrEmpty(pathLog))
                        {
                            System.IO.FileStream file = System.IO.File.Open(pathLog + DateTime.Now.ToString("yyyyMMdd") + ".sql", FileMode.Append);
                            StreamWriter writer = new StreamWriter(file);
                            string commandText = GetCommandTextCompleteFromDBCommand(command);
                            writer.WriteLine(commandText.IndexOf(";") == -1 ? commandText + ";" : commandText);
                            writer.Flush();
                            file.Close();
                        }
                    }
                    catch { }
                    #endregion
            }
            else
            {
                try{
                    objResult = DatabaseInstance.ExecuteScalar(command);
                    #region LogDB
                    try
                    {
                        string pathLog = GetPathLogDB();
                        if (!string.IsNullOrEmpty(pathLog))
                        {
                            System.IO.FileStream file = System.IO.File.Open(pathLog + DateTime.Now.ToString("yyyyMMdd") + ".sql", FileMode.Append);
                            StreamWriter writer = new StreamWriter(file);
                            string commandText = GetCommandTextCompleteFromDBCommand(command);
                            writer.WriteLine(commandText.IndexOf(";") == -1 ? commandText + ";" : commandText);
                            writer.Flush();
                            file.Close();
                        }
                    }
                    catch { }
                    #endregion
                }
                catch (Exception ex)
                {
                    try{
                        if (command.Connection != null && command.Connection.State == ConnectionState.Closed && ex.Message.ToLower().IndexOf("foi forçado o cancelamento de uma conexão existente") != -1)
                            command.Connection.Open();
                    }
                    catch { }
                    objResult = DatabaseInstance.ExecuteScalar(command);
                }
                
            }
            return objResult;
        }

        public object ExecuteScalar(CommandType commandType, string commandText)
        {
            object objResult = null;
            if ((databaseInstance.ConnectionStringWithoutCredentials.ToLower().IndexOf("mysql") != -1)|| (System.Configuration.ConfigurationManager.AppSettings["provider"]=="mysql"))
            {
                commandText = commandText.Replace("@", "?");
                commandText = commandText.Replace("with", " ");
                commandText = commandText.Replace("(nolock)", " ");
                commandText = commandText.Replace("nolock", " ");
            }
            if (IsTransactionCreated)
            {
                
                    objResult = DatabaseInstance.ExecuteScalar(TransactionHandler.Current.CurrentDbTransaction, commandType, commandText);
                    #region LogDB
                    try
                    {
                        string pathLog = GetPathLogDB();
                        if (!string.IsNullOrEmpty(pathLog))
                        {
                            System.IO.FileStream file = System.IO.File.Open(pathLog + DateTime.Now.ToString("yyyyMMdd") + ".sql", FileMode.Append);
                            StreamWriter writer = new StreamWriter(file);
                            writer.WriteLine(commandText.IndexOf(";") == -1 ? commandText + ";" : commandText);
                            writer.Flush();
                            file.Close();
                        }
                    }
                    catch { }
                    #endregion
            }
            else
            {
                
                    objResult = DatabaseInstance.ExecuteScalar(commandType, commandText);
                    #region LogDB
                    try
                    {
                        string pathLog = GetPathLogDB();
                        if (!string.IsNullOrEmpty(pathLog))
                        {
                            System.IO.FileStream file = System.IO.File.Open(pathLog + DateTime.Now.ToString("yyyyMMdd") + ".sql", FileMode.Append);
                            StreamWriter writer = new StreamWriter(file);
                            writer.WriteLine(commandText.IndexOf(";") == -1 ? commandText + ";" : commandText);
                            writer.Flush();
                            file.Close();
                        }
                    }
                    catch { }
                    #endregion
            }
            return objResult;
        }
        
        public object ExecuteScalar(string storedProcedureName, params object[] parameterValues)
        {
            object objResult = null;
            if (IsTransactionCreated)
            {
                
                    objResult = DatabaseInstance.ExecuteScalar(TransactionHandler.Current.CurrentDbTransaction, storedProcedureName, parameterValues);
                
            }
            else
            {
                
                    objResult = DatabaseInstance.ExecuteScalar(storedProcedureName, parameterValues);
                
            }
            return objResult;
        }

        public DbDataAdapter GetDataAdapter()
        {
            return DatabaseInstance.GetDataAdapter();
        }

        public object GetInstrumentationEventProvider()
        {
            return DatabaseInstance.GetInstrumentationEventProvider();
        }


        public object GetParameterValue(DbCommand command, string name)
        {
            if ((databaseInstance.ConnectionStringWithoutCredentials.ToLower().IndexOf("mysql") != -1)|| (System.Configuration.ConfigurationManager.AppSettings["provider"]=="mysql"))
            {
                command.CommandText = command.CommandText.Replace("@", "?");
                command.CommandText = command.CommandText.Replace("with", " ");
                command.CommandText = command.CommandText.Replace("(nolock)", " ");
                command.CommandText = command.CommandText.Replace("nolock", " ");
            }
            return DatabaseInstance.GetParameterValue(command, name);
        }

        public DbCommand GetSqlStringCommand(string query)
        {

            if ((databaseInstance.ConnectionStringWithoutCredentials.ToLower().IndexOf("mysql") != -1)|| (System.Configuration.ConfigurationManager.AppSettings["provider"]=="mysql"))
            {
                query = query.Replace("@", "?");
                query = query.Replace("with", " ");
                query = query.Replace("(nolock)", " ");
                query = query.Replace("nolock", " ");
            }
            return DatabaseInstance.GetSqlStringCommand(query);
        }


        public DbCommand GetStoredProcCommand(string storedProcedureName)
        {
            return DatabaseInstance.GetStoredProcCommand(storedProcedureName);
        }

        public DbCommand GetStoredProcCommand(string storedProcedureName, params object[] parameterValues)
        {
            return DatabaseInstance.GetStoredProcCommand(storedProcedureName, parameterValues);
        }

        public DbCommand GetStoredProcCommandWithSourceColumns(string storedProcedureName, params string[] sourceColumns)
        {
            return DatabaseInstance.GetStoredProcCommandWithSourceColumns(storedProcedureName, sourceColumns);
        }

        public void LoadDataSet(DbCommand command, DataSet dataSet, string tableName)
        {
            if ((databaseInstance.ConnectionStringWithoutCredentials.ToLower().IndexOf("mysql") != -1)|| (System.Configuration.ConfigurationManager.AppSettings["provider"]=="mysql"))
            {
                command.CommandText = command.CommandText.Replace("@", "?");
                command.CommandText = command.CommandText.Replace("with", " ");
                command.CommandText = command.CommandText.Replace("(nolock)", " ");
                command.CommandText = command.CommandText.Replace("nolock", " ");
            }
            if (IsTransactionCreated)
            {
                
                    DatabaseInstance.LoadDataSet(command, dataSet, tableName, TransactionHandler.Current.CurrentDbTransaction);
                
            }
            else
            {
                try{
                    DatabaseInstance.LoadDataSet(command, dataSet, tableName);
                }
                catch (Exception ex)
                {
                    try{
                    if (System.Configuration.ConfigurationManager.AppSettings["enabledAutoReconect"]==null && command.Connection != null && (command.Connection.State == ConnectionState.Closed || ex.Message.ToLower().IndexOf("foi forçado o cancelamento de uma conexão existente")!=-1))
                        command.Connection.Open();
                    }
                    catch { }
                    DatabaseInstance.LoadDataSet(command, dataSet, tableName);
                }
                
            }
        }

        public void LoadDataSet(DbCommand command, DataSet dataSet, string[] tableNames)
        {
            if ((databaseInstance.ConnectionStringWithoutCredentials.ToLower().IndexOf("mysql") != -1)|| (System.Configuration.ConfigurationManager.AppSettings["provider"]=="mysql"))
            {
                command.CommandText = command.CommandText.Replace("@", "?");
                command.CommandText = command.CommandText.Replace("with", " ");
                command.CommandText = command.CommandText.Replace("(nolock)", " ");
                command.CommandText = command.CommandText.Replace("nolock", " ");
            }
            if (IsTransactionCreated)
            {
                
                    DatabaseInstance.LoadDataSet(command, dataSet, tableNames, TransactionHandler.Current.CurrentDbTransaction);
                
            }
            else
            {
               try{
                    DatabaseInstance.LoadDataSet(command, dataSet, tableNames);
               }
               catch (Exception ex)
               {
                   try{
                   if (System.Configuration.ConfigurationManager.AppSettings["enabledAutoReconect"]==null && command.Connection != null && (command.Connection.State == ConnectionState.Closed || ex.Message.ToLower().IndexOf("foi forçado o cancelamento de uma conexão existente")!=-1))
                       command.Connection.Open();
                   }
                   catch { }
                   DatabaseInstance.LoadDataSet(command, dataSet, tableNames);
               }
                
            }
        }

        public void LoadDataSet(CommandType commandType, string commandText, DataSet dataSet, string[] tableNames)
        {
            if ((databaseInstance.ConnectionStringWithoutCredentials.ToLower().IndexOf("mysql") != -1)|| (System.Configuration.ConfigurationManager.AppSettings["provider"]=="mysql"))
            {
                commandText = commandText.Replace("@", "?");
                commandText = commandText.Replace("with", " ");
                commandText = commandText.Replace("(nolock)", " ");
                commandText = commandText.Replace("nolock", " ");
            }
            if (IsTransactionCreated)
            {
                
                    DatabaseInstance.LoadDataSet(TransactionHandler.Current.CurrentDbTransaction, commandType, commandText, dataSet, tableNames);
                
            }
            else
            {
               
                    DatabaseInstance.LoadDataSet(commandType, commandText, dataSet, tableNames);
                
            }
        }

        public void LoadDataSet(string storedProcedureName, DataSet dataSet, string[] tableNames, params object[] parameterValues)
        {
            if (IsTransactionCreated)
            {
               
                    DatabaseInstance.LoadDataSet(TransactionHandler.Current.CurrentDbTransaction, storedProcedureName, dataSet, tableNames, parameterValues);
                
            }
            else
            {
               
                    DatabaseInstance.LoadDataSet(storedProcedureName, dataSet, tableNames, parameterValues);
                
            }
        }

        public string ConnectionStringWithoutCredentials
        {
            get
            {
                return DatabaseInstance.ConnectionStringWithoutCredentials;
            }
        }

        public DbProviderFactory DbProviderFactory
        {
            get
            {
                return DatabaseInstance.DbProviderFactory;
            }
        }

        public void AddInParameter(DbCommand command, string name, DbType dbType)
        {
            if ((databaseInstance.ConnectionStringWithoutCredentials.ToLower().IndexOf("mysql") != -1)|| (System.Configuration.ConfigurationManager.AppSettings["provider"]=="mysql"))
                name = name.Replace("@", "?");            
            DatabaseInstance.AddInParameter(command, name, dbType);
        }
        public void AddInParameter(DbCommand command, string name, DbType dbType, object value)
        {
            if ((databaseInstance.ConnectionStringWithoutCredentials.ToLower().IndexOf("mysql") != -1)|| (System.Configuration.ConfigurationManager.AppSettings["provider"]=="mysql"))
                name = name.Replace("@", "?");            
            DatabaseInstance.AddInParameter(command, name, dbType, value);
        }

        public void AddInParameter(DbCommand command, string name, DbType dbType, string sourceColumn, DataRowVersion sourceVersion)
        {
            if ((databaseInstance.ConnectionStringWithoutCredentials.ToLower().IndexOf("mysql") != -1)|| (System.Configuration.ConfigurationManager.AppSettings["provider"]=="mysql"))
                name = name.Replace("@", "?");            
            DatabaseInstance.AddInParameter(command, name, dbType, sourceColumn, sourceVersion);
        }

        public void AddOutParameter(DbCommand command, string name, DbType dbType, int size)
        {
            if ((databaseInstance.ConnectionStringWithoutCredentials.ToLower().IndexOf("mysql") != -1)|| (System.Configuration.ConfigurationManager.AppSettings["provider"]=="mysql"))
                name = name.Replace("@", "?");            
            DatabaseInstance.AddOutParameter(command, name, dbType, size);
        }

        public void AddParameter(DbCommand command, string name, DbType dbType, ParameterDirection direction, string sourceColumn, DataRowVersion sourceVersion, object value)
        {
            if ((databaseInstance.ConnectionStringWithoutCredentials.ToLower().IndexOf("mysql") != -1)|| (System.Configuration.ConfigurationManager.AppSettings["provider"]=="mysql"))
                name = name.Replace("@", "?");            
            DatabaseInstance.AddParameter(command, name, dbType, direction, sourceColumn, sourceVersion, value);
        }

        public void AddParameter(DbCommand command, string name, DbType dbType, int size, ParameterDirection direction, bool nullable, byte precision, byte scale, string sourceColumn, DataRowVersion sourceVersion, object value)
        {
            if ((databaseInstance.ConnectionStringWithoutCredentials.ToLower().IndexOf("mysql") != -1)|| (System.Configuration.ConfigurationManager.AppSettings["provider"]=="mysql"))
                name = name.Replace("@", "?");            
            DatabaseInstance.AddParameter(command, name, dbType, size, direction, nullable, precision, scale, sourceColumn, sourceVersion, value);
        }

        public string BuildParameterName(string name)
        {
            return DatabaseInstance.BuildParameterName(name);
        }
      
        public DbConnection CreateConnection()
        {
            if (IsTransactionCreated)
            {
                return TransactionHandler.Current.CurrentDbTransaction.Connection;
            }
            else
            {
                return DatabaseInstance.CreateConnection();
            }
        }

        public void DiscoverParameters(DbCommand command)
        {
            DatabaseInstance.DiscoverParameters(command);
        }

        private string GetPathLogDB() 
        {
            string pathLogDB = "";
            if (System.Configuration.ConfigurationManager.AppSettings["LogDBExecution"] != null)
                pathLogDB = System.Configuration.ConfigurationManager.AppSettings["LogDBExecution"].ToString();
            return pathLogDB;
        }
        private string GetCommandTextCompleteFromDBCommand(DbCommand command) 
        {
            string commandText = "";
            
            commandText = command.CommandText;
            if (!commandText.ToLower().TrimStart().StartsWith("select"))
            {
                while (commandText.IndexOf("  ") != -1)
                    commandText = commandText.Replace("  ", " ");
                foreach (DbParameter parameter in command.Parameters)
                {
                    commandText = commandText.ToLower().Replace("?" + parameter.ParameterName.ToLower() + " where", ParameterValueForSQL(parameter) + " where");
                    commandText = commandText.ToLower().Replace("?" + parameter.ParameterName.ToLower() + ",", ParameterValueForSQL(parameter) + ",");
                    commandText = commandText.ToLower().Replace("?" + parameter.ParameterName.ToLower() + " ,", ParameterValueForSQL(parameter) + " ,");
                    commandText = commandText.ToLower().Replace("?" + parameter.ParameterName.ToLower() + ";", ParameterValueForSQL(parameter) + ";");
                    commandText = commandText.ToLower().Replace("?" + parameter.ParameterName.ToLower() + ")", ParameterValueForSQL(parameter) + ")");
                    commandText = commandText.ToLower().Replace("?" + parameter.ParameterName.ToLower() + " )", ParameterValueForSQL(parameter) + " )");
                }
            }
            if (commandText.IndexOf("?") != -1)
                return "";
            return commandText;
        }
        public static String ParameterValueForSQL(DbParameter sp)
        {
            String retval = "";

            switch (sp.DbType)
            {                                
                case DbType.String:
                case DbType.Xml:
                    retval = "'" + sp.Value.ToString().Replace("'", "''") + "'";
                    break;
                case DbType.Time:
                case DbType.Date:
                case DbType.DateTime:
                case DbType.DateTime2:
                case DbType.DateTimeOffset:
                    System.IFormatProvider dateFormat = new System.Globalization.CultureInfo("pt-BR", true);
                    if (sp.Value.ToString().IndexOf("/")!=-1)
                        retval = "'" + Convert.ToDateTime(sp.Value.ToString().Replace("'", ""), dateFormat).ToString("yyyy-MM-dd HH:mm:ss") + "'";
                    else
                        retval = "'" + sp.Value.ToString().Replace("'", "''") + "'";
                    break;
                case DbType.Boolean:
                case DbType.Byte:
                case DbType.Binary:
                    retval = ((bool)sp.Value) ? "1" : "0";
                    break;                    
                default:
                    retval = sp.Value.ToString().Replace("'", "''").Replace(",",".");
                    break;
            }

            return retval;
        }
    }
}