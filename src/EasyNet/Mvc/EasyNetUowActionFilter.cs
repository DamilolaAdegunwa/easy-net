using System.Reflection;
using System.Threading.Tasks;
using EasyNet.Domain.Uow;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;

namespace EasyNet.Mvc
{
    /// <summary>
    /// A filter implementation which delegates to the controller for starting a unit of work.
    /// </summary>
    public class EasyNetUowActionFilter : IAsyncActionFilter
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly EasyNetOptions _options;

        public EasyNetUowActionFilter(IUnitOfWorkManager unitOfWorkManager, IOptions<EasyNetOptions> options)
        {
            _unitOfWorkManager = unitOfWorkManager;
            _options = options.Value;
        }

        /// <inheritdoc />
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            Check.NotNull(context, nameof(context));
            Check.NotNull(next, nameof(next));

            if (!context.ActionDescriptor.IsControllerAction())
            {
                await next();
                return;
            }

            if (!_options.UnitOfWorkAutoStart)
            {
                await next();
                return;
            }

            // Try to get UnitOfWorkAttribute from attribute of method.
            var actionMethodInfo = context.ActionDescriptor.GetMethodInfo();
            var uowAttr = actionMethodInfo.GetCustomAttribute(typeof(UnitOfWorkAttribute));

            // Set unit of work options
            var unitOfWorkOptions = uowAttr == null ? new UnitOfWorkOptions() : UnitOfWorkOptions.Create((UnitOfWorkAttribute)uowAttr);

            using (var uow = _unitOfWorkManager.Begin(unitOfWorkOptions))
            {
                var result = await next();
                if (result.Exception == null || result.ExceptionHandled)
                {
                    await uow.CompleteAsync();
                }
            }
        }
    }
}
