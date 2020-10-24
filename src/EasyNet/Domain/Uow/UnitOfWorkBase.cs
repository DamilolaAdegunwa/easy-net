using System;
using System.Threading.Tasks;
using EasyNet.Extensions;
using EasyNet.Runtime.Session;
using Microsoft.Extensions.Options;

namespace EasyNet.Domain.Uow
{
    /// <summary>
    /// Base for all Unit Of Work classes.
    /// </summary>
    public abstract class UnitOfWorkBase : IUnitOfWork
    {
	    /// <summary>
	    /// Is <see cref="Begin"/> method called before?
	    /// </summary>
        private bool _isBeginCalledBefore;

	    /// <summary>
	    /// Is <see cref="Complete"/> method called before?
	    /// </summary>
        private bool _isCompleteCalledBefore;

	    /// <summary>
	    /// Is this unit of work successfully completed.
	    /// </summary>
        private bool _succeed;

	    /// <summary>
	    /// A reference to the exception if this unit of work failed.
	    /// </summary>
        private Exception _exception;

        protected UnitOfWorkBase(IOptions<UnitOfWorkDefaultOptions> defaultOptions)
        {
            DefaultOptions = defaultOptions.Value;

            Id = Guid.NewGuid().ToString("N");

            AbpSession = NullSession.Instance;
        }

        /// <inheritdoc/>
        public string Id { get; }

        /// <inheritdoc/>
        public IUnitOfWork Outer { get; set; }

        /// <inheritdoc/>
        public event EventHandler Completed;

        /// <inheritdoc/>
        public event EventHandler<UnitOfWorkFailedEventArgs> Failed;

        /// <inheritdoc/>
        public event EventHandler Disposed;

        /// <inheritdoc/>
        public UnitOfWorkOptions Options { get; private set; }

        /// <summary>
        /// Gets default UOW options.
        /// </summary>
        protected UnitOfWorkDefaultOptions DefaultOptions { get; }

        /// <summary>
        /// Gets a value indicates that this unit of work is disposed or not.
        /// </summary>
        public bool IsDisposed { get; private set; }

        /// <summary>
        /// Reference to current <see cref="ISession"/>.
        /// </summary>
        public ISession AbpSession { protected get; set; }

        /// <inheritdoc/>
        public void Begin(UnitOfWorkOptions options)
        {
            Check.NotNull(options, nameof(options));

            PreventMultipleBegin();
            Options = options; //TODO: Do not set options like that, instead make a copy?

            BeginUow();
        }

        /// <inheritdoc/>
        public abstract void SaveChanges();

        /// <inheritdoc/>
        public abstract Task SaveChangesAsync();

        /// <inheritdoc/>
        public void Complete()
        {
            PreventMultipleComplete();
            try
            {
                CompleteUow();
                _succeed = true;
                OnCompleted();
            }
            catch (Exception ex)
            {
                _exception = ex;
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task CompleteAsync()
        {
            PreventMultipleComplete();
            try
            {
                await CompleteUowAsync();
                _succeed = true;
                OnCompleted();
            }
            catch (Exception ex)
            {
                _exception = ex;
                throw;
            }
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            if (!_isBeginCalledBefore || IsDisposed)
            {
                return;
            }

            IsDisposed = true;

            if (!_succeed)
            {
                OnFailed(_exception);
            }

            DisposeUow();
            OnDisposed();
        }

        /// <summary>
        /// Can be implemented by derived classes to start UOW.
        /// </summary>
        protected virtual void BeginUow()
        {

        }

        /// <summary>
        /// Should be implemented by derived classes to complete UOW.
        /// </summary>
        protected abstract void CompleteUow();

        /// <summary>
        /// Should be implemented by derived classes to complete UOW.
        /// </summary>
        protected abstract Task CompleteUowAsync();

        /// <summary>
        /// Should be implemented by derived classes to dispose UOW.
        /// </summary>
        protected abstract void DisposeUow();

        /// <summary>
        /// Called to trigger <see cref="Completed"/> event.
        /// </summary>
        protected virtual void OnCompleted()
        {
            Completed.InvokeSafely(this);
        }

        /// <summary>
        /// Called to trigger <see cref="Failed"/> event.
        /// </summary>
        /// <param name="exception">Exception that cause failure</param>
        protected virtual void OnFailed(Exception exception)
        {
            Failed.InvokeSafely(this, new UnitOfWorkFailedEventArgs(exception));
        }

        /// <summary>
        /// Called to trigger <see cref="Disposed"/> event.
        /// </summary>
        protected virtual void OnDisposed()
        {
            Disposed.InvokeSafely(this);
        }

        private void PreventMultipleBegin()
        {
            if (_isBeginCalledBefore)
            {
                throw new EasyNetException("This unit of work has started before. Can not call Start method more than once.");
            }

            _isBeginCalledBefore = true;
        }

        private void PreventMultipleComplete()
        {
            if (_isCompleteCalledBefore)
            {
                throw new EasyNetException("Complete is called before!");
            }

            _isCompleteCalledBefore = true;
        }

        public override string ToString()
        {
            return $"[UnitOfWork {Id}]";
        }
    }
}
