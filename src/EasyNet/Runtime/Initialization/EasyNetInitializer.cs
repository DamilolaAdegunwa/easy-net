using System;
using EasyNet.Domain.Uow;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace EasyNet.Runtime.Initialization
{
    public class EasyNetInitializer : IEasyNetInitializer
    {
        protected readonly IServiceProvider ApplicationServices;
        protected readonly EasyNetInitializerOptions Options;

        public EasyNetInitializer(IServiceProvider serviceProvider, IOptions<EasyNetInitializerOptions> options)
        {
            Check.NotNull(serviceProvider, nameof(serviceProvider));
            Check.NotNull(options, nameof(options));

            ApplicationServices = serviceProvider;
            Options = options.Value;
        }

        /// <inheritdoc/>
        public virtual void Init()
        {
            foreach (var jobType in Options.JobTypes)
            {
                using (var scope = ApplicationServices.CreateScope())
                {
                    using (var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWorkManager>().Begin())
                    {
                        if (scope.ServiceProvider.GetRequiredService(jobType) is IEasyNetInitializationJob job)
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