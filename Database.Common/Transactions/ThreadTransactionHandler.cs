using System;
using System.Runtime.Remoting.Messaging;

namespace Database.Common.Transactions
{
	/// <summary>
    /// Classe responsavel em adicionar e remover o objeto Transaction no contexto da thread corrente
	/// </summary>
	public class ThreadTransactionHandler : ITransactionHandler
	{
		
        private const string SLOT_NAME = "POWER_TRANSACTION";
        
        public ThreadTransactionHandler()
		{
		}

        public bool IsContextCreated 
        {
            get 
            {
                return (CallContext.GetData(SLOT_NAME) != null) ? true : false;
            }
        }

        public Transaction Current 
        {
            get 
            {
                if (!IsContextCreated) 
                {
                    throw new TransactionException("Contexto inexistente");  
                } 
                else 
                {
                    return (Transaction) CallContext.GetData(SLOT_NAME);
                }
            }
        }

        public void Save(Transaction transaction) 
        {
            CallContext.SetData(SLOT_NAME, transaction);
        }

        public void Remove() 
        {
            CallContext.FreeNamedDataSlot(SLOT_NAME);
        }

	}
}
