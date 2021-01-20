using System;
using System.Threading.Tasks;

namespace EasyNet.Mvc
{
    public interface IEasyNetExceptionHandler
    {
        object WrapException(Exception ex);

        Task Handle(Exception ex);
    }
}
