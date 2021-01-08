using System;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;

namespace Database.Common.Transactions
{
	
    public enum TransactionVote 
    {
        Commit,
        Rollback
    }
    
    /// <summary>
    /// Classe wrapper para a classe DbTransaction do DAAB
	/// </summary>
	public class Transaction
	{
		private TransactionVote vote = TransactionVote.Rollback;
        private DbTransaction currentTransaction = null;
        private int transactionCount = 1;

        public Transaction(DbTransaction transaction) 
        {
            if (transaction == null) 
            {
                throw new ArgumentNullException("transaction", "Não é possível criar um contexto de transação sem um objeto DbTransaction válido");
            }

            this.currentTransaction = transaction;
            
        }

        public DbTransaction CurrentDbTransaction 
        {
            get { return currentTransaction; }
        }

        public static Transaction GetCurrent()
        {
            return GetCurrent(null);
        }

        public static Transaction GetCurrent(ITransactionHandler transactionHandler)
        {
            ITransactionHandler handler = null;
            if (transactionHandler == null) 
            {
                handler = TransactionHandlerFactory.CreateDefault();
            }

            return handler.Current;

        }

        public static bool IsContextCreated() 
        {
            return IsContextCreated(null);
        }

        public bool IsRootTransaction 
        {
            get 
            {
                return (Count == 1);
            }
        }

        public bool Terminated 
        {
            get 
            {
                return (IsRootTransaction && (Vote == TransactionVote.Commit));
            }
        }

        public static bool IsContextCreated(ITransactionHandler transactionHandler) 
        {
            ITransactionHandler handler = null;
            if (transactionHandler == null) 
            {
                handler = TransactionHandlerFactory.CreateDefault();
            }

            return handler.IsContextCreated;
        }


        public int Count 
        {
            get { return transactionCount; }
            set { transactionCount = value; }
        }

        public TransactionVote Vote 
        {
            get { return vote; }
            set { vote = value; }
        }
        
	}
}
