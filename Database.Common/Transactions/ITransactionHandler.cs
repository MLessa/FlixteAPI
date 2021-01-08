using System;

namespace Database.Common.Transactions
{
	/// <summary>
	/// Summary description for IContextHandler.
	/// </summary>
	public interface ITransactionHandler
	{
        bool IsContextCreated { get; }
        Transaction Current { get; } 
        void Save(Transaction transaction); 
        void Remove();

	}
}
