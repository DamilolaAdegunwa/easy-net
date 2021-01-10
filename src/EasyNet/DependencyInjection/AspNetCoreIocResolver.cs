using System;
using Microsoft.Extensions.DependencyInjection;

namespace EasyNet.DependencyInjection
{
    public class AspNetCoreIocResolver : IIocResolver
    {
        protected readonly IServiceProvider ServiceProvider;

        public AspNetCoreIocResolver(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public T GetService<T>(bool required = true)
        {
            if (required)
            {
                return ServiceProvider.GetRequiredService<T>();
            }
            else
            {
                return ServiceProvider.GetService<T>();
            }
        }

        public object GetService(Type serviceType, bool required = true)
        {
            if (required)
            {
                return ServiceProvider.GetRequiredService(serviceType);
            }
            else
            {
                return ServiceProvider.GetService(serviceType);
            }
        }
    }
}
