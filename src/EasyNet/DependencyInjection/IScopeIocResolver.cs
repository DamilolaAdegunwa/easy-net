using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyNet.DependencyInjection
{
    public interface IScopeIocResolver : IDisposable
    {
        T GetService<T>(bool required = true);

        object GetService(Type serviceType, bool required = true);

        IScopeIocResolver CreateScope();
    }
}
