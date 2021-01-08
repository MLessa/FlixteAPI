using System;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using Database.Common;
using System.Data.Common;

namespace Database.Common.Transactions
{
	/// <summary>
	/// Classe responsavelem gerenciar a transação entre as camadas
	/// </summary>
	public class TransactionScope : IDisposable
	{
        
		private ITransactionHandler transactionHandler;
        
        
		public TransactionScope() : this(null, null,null)
		{            
		}

        public TransactionScope(string connectionStringWithProvider)
            : this(connectionStringWithProvider, null,null){}

        public TransactionScope(string connectionStringWithProvider, ITransactionHandler transactionHandler)
            : this(connectionStringWithProvider, null, transactionHandler){}

        public TransactionScope(DBComponent dbComponent)
            : this(null, dbComponent, null){}

        public TransactionScope(DBComponent dbComponent, ITransactionHandler transactionHandler) 
            : this(null,dbComponent,transactionHandler){}

        private TransactionScope(string connectionStringWithProvider, DBComponent dbComponent, ITransactionHandler transactionHandler)
        {
            if (transactionHandler == null)
            {
                TransactionHandler = TransactionHandlerFactory.CreateDefault();
            }
            else
            {
                TransactionHandler = transactionHandler;
            }

            if (dbComponent != null)
            {
                BuildTransactionContext(dbComponent);
            }
            else
            {
                BuildTransactionContext(connectionStringWithProvider);
            }
        }

		private ITransactionHandler TransactionHandler 
		{
			get { return transactionHandler; }
			set { transactionHandler = value; }
		}

        public DbTransaction CurrentDbTransaction
        {
            get
            {
                DbTransaction transaction = null;

                if (transactionHandler != null && transactionHandler.Current != null)
                {
                    transaction = transactionHandler.Current.CurrentDbTransaction;
                }

                return transaction;
           }

        }

        /// <summary>
        /// Método responsavel em criar o objeto de transação
        /// </summary>
        /// <param name="connectionStringWithProvider"></param>
        protected void BuildTransactionContext(string connectionStringWithProvider)
        {
            BuildTransactionContext(null, connectionStringWithProvider);
        }

        protected void BuildTransactionContext(DBComponent dbComponent)
        {
            BuildTransactionContext(dbComponent, null);
        }
        private void BuildTransactionContext(DBComponent dbComponent, string connectionStringWithProvider) 
		{
            //caso a transação não esteja criada
			if (!TransactionHandler.IsContextCreated) 
			{

                Database database = null;

                if (dbComponent != null)
                {
                    database = dbComponent.DatabaseInformation.DB;
                }
                else
                {
                    database = DatabaseFactory.CreateDatabase(connectionStringWithProvider);
                }
         
				DbConnection connection = database.CreateConnection();
				connection.Open();
				DbTransaction dbTransaction = connection.BeginTransaction();

				Transaction transaction = new Transaction(dbTransaction);


				TransactionHandler.Save(transaction);
			} 
			else 
			{   
				IncrementTransactionCount();
			}
            
		}

		private bool IsRootTransaction 
		{
			get 
			{
				return TransactionHandler.Current.IsRootTransaction;
			}
		}

        /// <summary>
        /// Método responsavel em completar a transação
        /// </summary>
		public void Complete() 
		{
			if (!TransactionHandler.IsContextCreated)
			{
				throw new TransactionException("Falha ao tentar efetuar o commit. Não existe contexto de transação criado");
			}

			if (IsRootTransaction) 
			{
				TransactionHandler.Current.Vote = TransactionVote.Commit;
			}
		}

		protected void Terminate() 
		{

			if (!TransactionHandler.IsContextCreated) 
			{
				throw new TransactionException("Falha ao tentar efetuar o commit. Não existe contexto de transação criado");
			}

          
			if (IsRootTransaction) 
			{
				if (TransactionHandler.Current.Vote == TransactionVote.Commit) 
				{
					CommitTransaction();
				} 
				else 
				{
					RollbackTransaction();
				}
			} 
			else 
			{
				DecrementTransactionCount();
			}

		}

		protected void CommitTransaction()  
		{
			IDbTransaction dbTransaction = TransactionHandler.Current.CurrentDbTransaction;
			IDbConnection dbConnection = dbTransaction.Connection;
			try 
			{
				dbTransaction.Commit();
			} 
			finally 
			{
				if ( (dbConnection != null) && (dbConnection.State != ConnectionState.Closed) ) 
				{
					dbConnection.Close();
					dbConnection.Dispose();
				}

				dbTransaction.Dispose();
				TransactionHandler.Remove();
			}
		}

		protected void RollbackTransaction()  
		{
			IDbTransaction dbTransaction = TransactionHandler.Current.CurrentDbTransaction;
			IDbConnection dbConnection = dbTransaction.Connection;
			try 
			{
				dbTransaction.Rollback();
			} 
			finally 
			{
				if ( (dbConnection != null) && (dbConnection.State != ConnectionState.Closed) ) 
				{
					dbConnection.Close();
					dbConnection.Dispose();
				}

				dbTransaction.Dispose();
				TransactionHandler.Remove();
			}
		}

		protected void IncrementTransactionCount() 
		{
			Transaction transaction = TransactionHandler.Current;
			transaction.Count++;
			TransactionHandler.Save(transaction);
		}

		protected void DecrementTransactionCount() 
		{
			Transaction transaction = TransactionHandler.Current;
			transaction.Count--;
			TransactionHandler.Save(transaction);
		}

		public void Dispose()
		{
			Terminate();
		}


	}
}
