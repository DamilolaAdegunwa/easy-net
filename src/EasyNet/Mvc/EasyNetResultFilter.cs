
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
#if !Net50 && !Net31
using Newtonsoft.Json;
#else
using System.Text.Json;
#endif

namespace EasyNet.Mvc
{
    /// <summary>
    /// A filter implementation which delegates to the controller for return result.
    /// </summary>
    public class EasyNetResultFilter : IAsyncResultFilter
    {
        private readonly IEasyNetResultWrapper _resultWrapper;

        public EasyNetResultFilter(IEasyNetResultWrapper resultWrapper = null)
        {
            _resultWrapper = resultWrapper;
        }

        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            if (_resultWrapper != null)
            {
                if (context.Result is ObjectResult objectResult)
                {
                    objectResult.Value = _resultWrapper.WrapResult(objectResult.Value);
                }
#if Net50 || Net31
                else if (context.Result is JsonResult jsonResult)
                {
                    jsonResult.Value = _resultWrapper.WrapResult(jsonResult.Value);
                }
#endif
            }

            await next();
        }
    }
}
