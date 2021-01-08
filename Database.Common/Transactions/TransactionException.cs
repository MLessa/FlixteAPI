using System;

namespace Database.Common.Transactions
{
	/// <summary>
	/// Exce��o disparada no gerenciamento de transa��es com TransactionScope
	/// </summary>
	public class TransactionException : ApplicationException
	{
		public TransactionException() : base()
		{
		}

        public TransactionException(string mensagem) : base(mensagem)
        {
        }

        public TransactionException(string mensagem, Exception causa) : base(mensagem, causa)
        {
        }

	}
}
