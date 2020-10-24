using System.Threading.Tasks;

namespace EasyNet.Domain.Uow
{
    internal class InnerSuppressUnitOfWorkCompleteHandle : InnerUnitOfWorkCompleteHandle
    {
        private readonly IUnitOfWork _parentUnitOfWork;

        public InnerSuppressUnitOfWorkCompleteHandle(IUnitOfWork parentUnitOfWork)
        {
            _parentUnitOfWork = parentUnitOfWork;
        }

        /// <inheritdoc/>
        public override void Complete()
        {
            _parentUnitOfWork.SaveChanges();
            base.Complete();
        }

        /// <inheritdoc/>
        public override async Task CompleteAsync()
        {
            await _parentUnitOfWork.SaveChangesAsync();
            await base.CompleteAsync();
        }
    }
}
