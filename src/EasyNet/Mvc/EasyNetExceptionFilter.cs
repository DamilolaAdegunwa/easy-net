
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
    /// A filter implementation which delegates to the controller for handle exception.
    /// </summary>
    public class EasyNetExceptionFilter : IAsyncExceptionFilter
    {
        private readonly IEasyNetExceptionHandler _exceptionHandler;

        public EasyNetExceptionFilter(IEasyNetExceptionHandler exceptionHandler)
        {
            _exceptionHandler = exceptionHandler;
        }

        public Task OnExceptionAsync(ExceptionContext context)
        {
            if (!context.ExceptionHandled)
            {
                var exceptionResult = _exceptionHandler.WrapException(context.Exception);

                var json = string.Empty;
#if Net50 || Net31
                json = JsonSerializer.Serialize(
                    exceptionResult,
                    new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    });
#else
                json = JsonConvert.SerializeObject(
                    exceptionResult,
                    Formatting.Indented,
                    new JsonSerializerSettings
                    {
                        ContractResolver =
                            new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
                    });
#endif

                context.Result = new ContentResult
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    ContentType = "application/json;charset=utf-8",
                    Content = json
                };

                _exceptionHandler.Handle(context.Exception);
            }

            context.ExceptionHandled = true;
            return Task.CompletedTask;
        }
    }
}
