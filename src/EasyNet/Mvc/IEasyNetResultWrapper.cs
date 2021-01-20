using System;
using System.Threading.Tasks;

namespace EasyNet.Mvc
{
    public interface IEasyNetResultWrapper
    {
        object WrapResult(object value);
    }
}
