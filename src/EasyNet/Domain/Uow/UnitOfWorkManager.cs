using System.Transactions;
using EasyNet.DependencyInjection;
using Microsoft.Extensions.Options;

namespace EasyNet.Domain.Uow
{
	/// <summary>
	/// Unit of work manager.
	/// </summary>
    internal class UnitOfWorkManager : IUnitOfWorkManager
    {
        private readonly ICurrentUnitOfWorkProvider _currentUnitOfWorkProvider;
        private readonly UnitOfWorkDefaultOptions _defaultOptions;
        private readonly IIocResolver _iocResolver;

        public UnitOfWorkManager(
	        IIocResolver iocResolver,
            ICurrentUnitOfWorkProvider currentUnitOfWorkProvider,
            IOptions<UnitOfWorkDefaultOptions> defaultOptions)
        {
            _currentUnitOfWorkProvider = currentUnitOfWorkProvider;
            _defaultOptions = defaultOptions.Value;
            _iocResolver = iocResolver;
        }

        /// <inheritdoc/>
        public IActiveUnitOfWork Current => _currentUnitOfWorkProvider.Current;

        /// <inheritdoc/>
        public IUnitOfWorkCompleteHandle Begin()
        {
            return Begin(new UnitOfWorkOptions());
        }

        /// <inheritdoc/>
        public IUnitOfWorkCompleteHandle Begin(TransactionScopeOption scope)
        {
            return Begin(new UnitOfWorkOptions { Scope = scope });
        }

        /// <inheritdoc/>
        public IUnitOfWorkCompleteHandle Begin(UnitOfWorkOptions options)
        {
            options.FillDefaultsForNonProvidedOptions(_defaultOptions);

            var outerUow = _currentUnitOfWorkProvider.Current;

            if (options.Scope == TransactionScopeOption.Required && outerUow != null)
            {
                return outerUow.Options?.Scope == TransactionScopeOption.Suppress
                    ? new InnerSuppressUnitOfWorkCompleteHandle(outerUow)
                    : new InnerUnitOfWorkCompleteHandle();
            }

            var uow = _iocResolver.GetService<IUnitOfWork>();

            uow.Completed += (sender, args) =>
            {
                _currentUnitOfWorkProvider.Current = null;
            };

            uow.Failed += (sender, args) =>
            {
                _currentUnitOfWorkProvider.Current = null;
            };

            uow.Disposed += (sender, args) =>
            {
                // No action
            };

            uow.Begin(options);

            _currentUnitOfWorkProvider.Current = uow;

            return uow;
        }
    }
}
