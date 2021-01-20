using System;
using EasyNet.Domain.Uow;
using EasyNet.Mvc;
using EasyNet.Runtime.Initialization;
using EasyNet.Runtime.Session;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace EasyNet.DependencyInjection
{
    /// <summary>
    /// Extensions for configuring EasyNet using an <see cref="IMvcBuilder"/>.
    /// </summary>
    public static class EasyNetBuilderExtensions
    {
        /// <summary>
        /// Configure <see cref="UnitOfWorkDefaultOptions"/>
        /// </summary>
        /// <param name="builder">The <see cref="IEasyNetBuilder"/>.</param>
        /// <param name="setupAction">An <see cref="Action{UnitOfWorkDefaultOptions}"/> to configure the provided <see cref="UnitOfWorkDefaultOptions"/>.</param>
        /// <returns>An <see cref="IEasyNetBuilder"/> that can be used to further configure the EasyNet services.</returns>
        public static IEasyNetBuilder ConfigureUnitOfWorkDefaultOptions(this IEasyNetBuilder builder, Action<UnitOfWorkDefaultOptions> setupAction)
        {
            Check.NotNull(builder, nameof(builder));
            Check.NotNull(setupAction, nameof(setupAction));

            builder.Services.Configure(setupAction);

            return builder;
        }

        /// <summary>
        /// Add a job to be executed when EasyNet initialization.
        /// </summary>
        /// <param name="builder">The <see cref="IEasyNetBuilder"/>.</param>
        /// <param name="jobTypes">The job which need to be executed when EasyNet initialization.</param>
        /// <returns>An <see cref="IEasyNetBuilder"/> that can be used to further configure the EasyNet services.</returns>
        public static IEasyNetBuilder AddInitializationJob(this IEasyNetBuilder builder, params Type[] jobTypes)
        {
            Check.NotNull(builder, nameof(builder));
            Check.NotNullOrEmpty(jobTypes, nameof(jobTypes));

            foreach (var type in jobTypes)
            {
                if (!typeof(IEasyNetInitializationJob).IsAssignableFrom(type))
                    throw new EasyNetException($"Type {type.AssemblyQualifiedName} does not inherit {typeof(IEasyNetInitializationJob).AssemblyQualifiedName}.");

                builder.Services.TryAddTransient(type);

                builder.Services.Configure<EasyNetInitializerOptions>(options =>
                {
                    options.JobTypes.Add(type);
                });
            }

            return builder;
        }

        /// <summary>
        /// Add a new <see cref="IEasyNetSession"/> implementation.
        /// </summary>
        /// <typeparam name="TSession"></typeparam>
        /// <param name="builder">The <see cref="IEasyNetBuilder"/>.</param>
        /// <returns>An <see cref="IEasyNetBuilder"/> that can be used to further configure the EasyNet services.</returns>
        public static IEasyNetBuilder AddSession<TSession>(this IEasyNetBuilder builder)
            where TSession : IEasyNetSession
        {
            Check.NotNull(builder, nameof(builder));

            builder.Services.Replace(new ServiceDescriptor(typeof(IEasyNetSession), typeof(TSession), ServiceLifetime.Scoped));

            return builder;
        }

        /// <summary>
        /// Add a new <see cref="IIocResolver"/> implementation.
        /// </summary>
        /// <typeparam name="TIocResolver"></typeparam>
        /// <param name="builder">The <see cref="IEasyNetBuilder"/>.</param>
        /// <returns>An <see cref="IEasyNetBuilder"/> that can be used to further configure the EasyNet services.</returns>
        public static IEasyNetBuilder AddIocResolver<TIocResolver>(this IEasyNetBuilder builder)
            where TIocResolver : IIocResolver
        {
            Check.NotNull(builder, nameof(builder));

            builder.Services.Replace(new ServiceDescriptor(typeof(IIocResolver), typeof(TIocResolver), ServiceLifetime.Scoped));

            return builder;
        }

        /// <summary>
        /// Add a new <see cref="IEasyNetExceptionHandler"/> implementation.
        /// </summary>
        /// <typeparam name="TExceptionHandler"></typeparam>
        /// <param name="builder">The <see cref="IEasyNetBuilder"/>.</param>
        /// <returns>An <see cref="IEasyNetBuilder"/> that can be used to further configure the EasyNet services.</returns>
        public static IEasyNetBuilder AddExceptionHandler<TExceptionHandler>(this IEasyNetBuilder builder)
            where TExceptionHandler : IEasyNetExceptionHandler
        {
            Check.NotNull(builder, nameof(builder));

            builder.Services.Replace(new ServiceDescriptor(typeof(IEasyNetExceptionHandler), typeof(TExceptionHandler), ServiceLifetime.Transient));

            return builder;
        }
    }
}
