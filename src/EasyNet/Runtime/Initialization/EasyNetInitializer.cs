using System;
using EasyNet.DependencyInjection;
using EasyNet.Domain.Uow;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace EasyNet.Runtime.Initialization
{
    public class EasyNetInitializer : IEasyNetInitializer
    {
        protected readonly IIocResolver IocResolver;
        protected readonly EasyNetInitializerOptions Options;

        public EasyNetInitializer(IIocResolver serviceProvider, IOptions<EasyNetInitializerOptions> options)
        {
            Check.NotNull(serviceProvider, nameof(serviceProvider));
            Check.NotNull(options, nameof(options));

            IocResolver = serviceProvider;
            Options = options.Value;
        }

        /// <inheritdoc/>
        public virtual void Init()
        {
            foreach (var jobType in Options.JobTypes)
            {
                using (var scope = IocResolver.CreateScope())
                {
                    using (var uow = scope.GetService<IUnitOfWorkManager>().Begin())
                    {
                        if (scope.GetService(jobType) is IEasyNetInitializationJob job)
                        {
                            job.Start();
                        }
                        else
                        {
                            throw new EasyNetException($"Type {jobType.AssemblyQualifiedName} does not inherit {typeof(IEasyNetInitializationJob).AssemblyQualifiedName}.");
                        }

                        uow.Complete();
                    }
                }
            }
        }
    }
}