using System;
using EasyNet.Domain.Uow;
using EasyNet.Mvc;
using EasyNet.Runtime.Initialization;
using EasyNet.Runtime.Session;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace EasyNet.DependencyInjection
{
    /// <summary>
    /// Extension methods for setting up EasyNet services in an <see cref="IServiceCollection" />.
    /// </summary>
    public static class EasyNetServiceCollectionExtensions
    {
        /// <summary>
        /// Adds EasyNet services to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <returns>An <see cref="IEasyNetBuilder"/> that can be used to further configure the EasyNet services.</returns>
        public static IEasyNetBuilder AddEasyNet(this IServiceCollection services)
        {
            Check.NotNull(services, nameof(services));

            return services.AddEasyNet(o => { });
        }

        /// <summary>
        /// Adds EasyNet services to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="setupAction">An <see cref="Action{EasyNetOptions}"/> to configure the provided <see cref="EasyNetOptions"/>.</param>
        /// <returns>An <see cref="IEasyNetBuilder"/> that can be used to further configure the EasyNet services.</returns>
        public static IEasyNetBuilder AddEasyNet(this IServiceCollection services, Action<EasyNetOptions> setupAction)
        {
            Check.NotNull(services, nameof(services));

            if (setupAction != null)
            {
                services.Configure(setupAction);
            }

            // See https://github.com/aspnet/Mvc/issues/3936 to know why we added these services.
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services
                .AddTransient<IEasyNetInitializer, EasyNetInitializer>()
                .AddScoped<ICurrentUnitOfWorkProvider, AsyncLocalCurrentUnitOfWorkProvider>()
                .AddScoped<IUnitOfWorkManager, UnitOfWorkManager>()
                .AddTransient<IUnitOfWork, NullUnitOfWork>()
                .AddSingleton<IPrincipalAccessor, DefaultPrincipalAccessor>()
                .AddScoped<IEasyNetSession, ClaimsEasyNetSession>()
                .AddTransient<EasyNetUowActionFilter>()
                .Configure<MvcOptions>(mvcOptions =>
                {
                    mvcOptions.Filters.AddService<EasyNetUowActionFilter>();
                });

            return new EasyNetBuilder(services);
        }
    }
}
