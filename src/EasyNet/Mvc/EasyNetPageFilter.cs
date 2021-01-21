using System;
using System.Reflection;
using System.Threading.Tasks;
using EasyNet.Domain.Uow;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;

namespace EasyNet.Mvc
{
    public class EasyNetPageFilter : IAsyncPageFilter
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly EasyNetOptions _options;

        public EasyNetPageFilter(IUnitOfWorkManager unitOfWorkManager, IOptions<EasyNetOptions> options)
        {
            _unitOfWorkManager = unitOfWorkManager;
            _options = options.Value;
        }

        public Task OnPageHandlerSelectionAsync(PageHandlerSelectedContext context)
        {
            return Task.CompletedTask;
        }

        public async Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context, PageHandlerExecutionDelegate next)
        {
            if (context.HandlerMethod == null)
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
            var uowAttr = context.HandlerMethod.MethodInfo.GetCustomAttribute(typeof(UnitOfWorkAttribute));

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
