using System;

namespace Database.Common.Transactions
{
	/// <summary>
	/// Summary description for TransactionHandlerFactory.
	/// </summary>
	public class TransactionHandlerFactory
	{

        public static ITransactionHandler CreateDefault() 
        {
            return new ThreadTransactionHandler();
        }

	}
}
