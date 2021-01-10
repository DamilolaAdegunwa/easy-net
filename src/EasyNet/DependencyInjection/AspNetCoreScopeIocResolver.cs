using System;
using Microsoft.Extensions.DependencyInjection;

namespace EasyNet.DependencyInjection
{
    internal class AspNetCoreScopeIocResolver : IScopeIocResolver
    {
        protected readonly IServiceScope ServiceScope;

        protected IServiceProvider ServiceProvider => ServiceScope?.ServiceProvider;

        internal AspNetCoreScopeIocResolver(IServiceScope serviceScope)
        {
            ServiceScope = serviceScope;
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

        public IScopeIocResolver CreateScope()
        {
            return new AspNetCoreScopeIocResolver(ServiceProvider.CreateScope());
        }


        public void Dispose()
        {
            ServiceScope?.Dispose();
        }
    }
}
