using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
#if NET461
using System.Collections.ObjectModel;
#else
using System.Collections.Immutable;
#endif

namespace EasyNet.Domain.Uow
{
    public class UnitOfWorkDefaultOptions
    {
        public UnitOfWorkDefaultOptions()
        {
            IsTransactional = true;
            Scope = TransactionScopeOption.Required;
            _filters = new List<DataFilterConfiguration>
            {
                new DataFilterConfiguration(EasyNetDataFilters.MustHaveTenant, true),
                new DataFilterConfiguration(EasyNetDataFilters.MayHaveTenant, true),
                new DataFilterConfiguration(EasyNetDataFilters.SoftDelete, true)
            };
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

        public IReadOnlyList<DataFilterConfiguration> Filters
        {
            get
            {
#if Net461
                return new ReadOnlyCollection<DataFilterConfiguration>(_filters);
#else
                return _filters.ToImmutableList();
#endif
            }
        }

        private readonly List<DataFilterConfiguration> _filters;

        public void RegisterFilter(string filterName, bool isEnabledByDefault)
        {
            if (_filters.Any(f => f.FilterName == filterName))
            {
                throw new EasyNetException("There is already a filter with name: " + filterName);
            }

            _filters.Add(new DataFilterConfiguration(filterName, isEnabledByDefault));
        }

        public void OverrideFilter(string filterName, bool isEnabledByDefault)
        {
            _filters.RemoveAll(f => f.FilterName == filterName);
            _filters.Add(new DataFilterConfiguration(filterName, isEnabledByDefault));
        }
    }
}
