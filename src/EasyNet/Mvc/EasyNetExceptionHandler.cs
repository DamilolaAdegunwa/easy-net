using System;
using System.Threading.Tasks;

namespace EasyNet.Mvc
{
    public class EasyNetExceptionHandler : IEasyNetExceptionHandler
    {
        public object WrapException(Exception ex)
        {
            if (ex is EasyNetFriendlyException friendlyException)
            {
                return new
                {
                    Code = friendlyException.Code,
                    Error = ex.Message
                };
            }
            else
            {
                return new
                {
                    Code = 0,
                    Error = ex.Message
                };
            }
        }

        public Task Handle(Exception ex)
        {
            // Do nothing
            return Task.CompletedTask;
        }
    }
}
