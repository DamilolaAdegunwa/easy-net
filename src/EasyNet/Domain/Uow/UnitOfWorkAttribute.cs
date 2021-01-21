using System;
using System.Transactions;

namespace EasyNet.Domain.Uow
{
    public class UnitOfWorkAttribute : Attribute
    {
        public UnitOfWorkAttribute(bool isTransactional)
        {
            IsTransactional = isTransactional;
        }

        public UnitOfWorkAttribute(TimeSpan timeout)
        {
            Timeout = timeout;
        }

        public UnitOfWorkAttribute(TransactionScopeOption scope)
        {
            Scope = scope;
        }

        public UnitOfWorkAttribute(IsolationLevel isolationLevel)
        {
            IsolationLevel = isolationLevel;
        }

        public UnitOfWorkAttribute(bool isTransactional, TimeSpan timeout)
        {
            IsTransactional = isTransactional;
            Timeout = timeout;
        }

        public UnitOfWorkAttribute(bool isTransactional, TransactionScopeOption scope)
        {
            IsTransactional = isTransactional;
            Scope = scope;
        }

        public UnitOfWorkAttribute(bool isTransactional, IsolationLevel isolationLevel)
        {
            IsTransactional = isTransactional;
            IsolationLevel = isolationLevel;
        }

        public UnitOfWorkAttribute(TimeSpan timeout, TransactionScopeOption scope)
        {
            Timeout = timeout;
            Scope = scope;
        }

        public UnitOfWorkAttribute(TimeSpan timeout, IsolationLevel isolationLevel)
        {
            Timeout = timeout;
            IsolationLevel = isolationLevel;
        }

        public UnitOfWorkAttribute(TransactionScopeOption scope, IsolationLevel isolationLevel)
        {
            Scope = scope;
            IsolationLevel = isolationLevel;
        }

        public UnitOfWorkAttribute(bool isTransactional, TimeSpan timeout, TransactionScopeOption scope)
        {
            IsTransactional = isTransactional;
            Timeout = timeout;
            Scope = scope;
        }

        public UnitOfWorkAttribute(bool isTransactional, TimeSpan timeout, IsolationLevel isolationLevel)
        {
            IsTransactional = isTransactional;
            Timeout = timeout;
            IsolationLevel = isolationLevel;
        }

        public UnitOfWorkAttribute(bool isTransactional, TransactionScopeOption scope, IsolationLevel isolationLevel)
        {
            IsTransactional = isTransactional;
            Scope = scope;
            IsolationLevel = isolationLevel;
        }

        public UnitOfWorkAttribute(TimeSpan timeout, TransactionScopeOption scope, IsolationLevel isolationLevel)
        {
            Timeout = timeout;
            Scope = scope;
            IsolationLevel = isolationLevel;
        }

        public UnitOfWorkAttribute(bool isTransactional, TimeSpan timeout, TransactionScopeOption scope, IsolationLevel isolationLevel)
        {
            IsTransactional = isTransactional;
            Timeout = timeout;
            Scope = scope;
            IsolationLevel = isolationLevel;
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
