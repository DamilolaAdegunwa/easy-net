using System;
using System.Reflection;
using EasyNet.DependencyInjection;
using EasyNet.Domain.Repositories;
using EasyNet.Domain.Uow;
using EasyNet.EntityFrameworkCore.Domain.Repositories;
using EasyNet.EntityFrameworkCore.Domain.Uow;
using EasyNet.EntityFrameworkCore.Initialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace EasyNet.EntityFrameworkCore.DependencyInjection
{
    /// <summary>
    /// Extensions for configuring EasyNet using an <see cref="IMvcBuilder"/>.
    /// </summary>
    public static class EasyNetBuilderExtensions
    {
        /// <summary>
        /// Add specified services to let the system support EntityFrameworkCore.
        /// </summary>
        /// <typeparam name="TDbContext">The context associated with the application.</typeparam>
        /// <param name="builder">The <see cref="IEasyNetBuilder"/>.</param>
        /// <param name="setupAction">An <see cref="Action{DbContextOptionsBuilder}"/> to configure the provided <see cref="DbContextOptionsBuilder"/>.</param>
        /// <returns></returns>
        public static IEasyNetBuilder AddEfCore<TDbContext>(this IEasyNetBuilder builder, Action<DbContextOptionsBuilder> setupAction)
            where TDbContext : EasyNetDbContext
        {
            Check.NotNull(builder, nameof(builder));
            Check.NotNull(setupAction, nameof(setupAction));

            builder.Services
                .AddDbContext<TDbContext>(setupAction)
                .Replace(new ServiceDescriptor(typeof(IUnitOfWork), typeof(EfCoreUnitOfWork), ServiceLifetime.Transient))
                .AddScoped<IDbContextProvider<TDbContext>, UnitOfWorkDbContextProvider<TDbContext>>();

            RegisterRepositories<TDbContext>(builder.Services);

            return builder;
        }

        /// <summary>
        /// Executes database migration command when EasyNet initialization.
        /// </summary>
        /// <typeparam name="TDbContext">The context associated with the application.</typeparam>
        /// <param name="builder">The <see cref="IEasyNetBuilder"/>.</param>
        /// <returns></returns>
        public static IEasyNetBuilder AddDatabaseMigrationJob<TDbContext>(this IEasyNetBuilder builder)
            where TDbContext : EasyNetDbContext
        {
            Check.NotNull(builder, nameof(builder));

            builder.AddInitializationJob(typeof(DatabaseMigrationJob<TDbContext>));

            return builder;
        }

        /// <summary>
        /// Add all repositories service to the <see cref="IServiceCollection"/>.
        /// </summary>
        /// <typeparam name="TDbContext">The context associated with the application.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        private static void RegisterRepositories<TDbContext>(IServiceCollection services) where TDbContext : EasyNetDbContext
        {
            var dbContextType = typeof(TDbContext);
            var properties = dbContextType.GetProperties(BindingFlags.Instance | BindingFlags.Public);

            foreach (var property in properties)
            {
                // Try to get DbSet<> type collection
                if (property.PropertyType.IsGenericType &&
                    string.Equals(property.PropertyType.Name, typeof(DbSet<>).Name, StringComparison.CurrentCultureIgnoreCase))
                {
                    // Try to get entity type
                    if (property.PropertyType.GenericTypeArguments.Length == 1)
                    {
                        var entityType = property.PropertyType.GenericTypeArguments[0];

                        // Try to get id property
                        var idProperty = entityType.GetProperty("Id");
                        if (idProperty != null)
                        {
                            // Add short service IRepository<TEntity> if the id property type is int.
                            if (idProperty.PropertyType == typeof(int))
                            {
                                services.TryAddTransient(
                                    typeof(IRepository<>).MakeGenericType(entityType),
                                    typeof(EfCoreRepositoryBase<,>).MakeGenericType(dbContextType, entityType));
                            }

                            // Add service IRepository<TEntity,TPrimaryKey>
                            services.TryAddTransient(
                                typeof(IRepository<,>).MakeGenericType(entityType, idProperty.PropertyType),
                                typeof(EfCoreRepositoryBase<,,>).MakeGenericType(dbContextType, entityType, idProperty.PropertyType));
                        }
                    }
                }
            }
        }
    }
}
