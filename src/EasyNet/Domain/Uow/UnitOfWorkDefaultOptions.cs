using System;
using System.Transactions;

namespace EasyNet.Domain.Uow
{
    public class UnitOfWorkDefaultOptions
    {
        public UnitOfWorkDefaultOptions()
        {
            IsTransactional = true;
            Scope = TransactionScopeOption.Required;
        }

        /// <summary>
        /// Scope option.
        /// </summary>
        public TransactionScopeOption? Scope { get; set; }

        /// <summary>
        /// Is this UOW transactional?
        /// Uses default value if not supplied.
        /// </summary>
        public bool? IsTransactional { get; set; }

        /// <summary>
        /// Timeout of UOW As milliseconds.
        /// Uses default value if not supplied.
        /// </summary>
        public TimeSpan? Timeout { get; set; }

        /// <summary>
        /// If this UOW is transactional, this option indicated the isolation level of the transaction.
        /// Uses default value if not supplied.
        /// </summary>
        public IsolationLevel? IsolationLevel { get; set; }
	}
}
