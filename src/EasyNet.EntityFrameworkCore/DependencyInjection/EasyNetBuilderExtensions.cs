using System;
using System.Reflection;
using EasyNet.DependencyInjection;
using EasyNet.Domain.Repositories;
using EasyNet.Domain.Uow;
using EasyNet.EntityFrameworkCore.Repositories;
using EasyNet.EntityFrameworkCore.Uow;
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
        /// <param name="asMainOrmTechnology">
        /// Sometimes use more than one orm technology in one system. We need to choice one of them as main technology.
        /// Use this value to control which orm is main technology.
        /// We can use <see cref="IRepository{TEntity,TPrimaryKey}"/> to get a repository service if this value is true. We also can use <see cref="IEfCoreRepository{TEntity,TPrimaryKey}"/> at the same time.
        /// </param>
        /// <returns></returns>
        public static IEasyNetBuilder AddEfCore<TDbContext>(this IEasyNetBuilder builder, Action<DbContextOptionsBuilder> setupAction, bool asMainOrmTechnology)
            where TDbContext : EasyNetDbContext
        {
            Check.NotNull(builder, nameof(builder));
            Check.NotNull(setupAction, nameof(setupAction));

            builder.Services
                .AddDbContext<TDbContext>(setupAction)
                .Replace(new ServiceDescriptor(typeof(IUnitOfWork), typeof(EfCoreUnitOfWork), ServiceLifetime.Transient))
                .AddScoped<IDbContextProvider<TDbContext>, UnitOfWorkDbContextProvider<TDbContext>>();

            RegisterRepositories<TDbContext>(builder.Services, asMainOrmTechnology);

            return builder;
        }

        /// <summary>
        /// Add all repositories service to the <see cref="IServiceCollection"/>.
        /// </summary>
        /// <typeparam name="TDbContext">The context associated with the application.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="asMainOrmTechnology">
        /// Sometimes use more than one orm technology in one system. We need to choice one of them as main technology.
        /// Use this value to control which orm is main technology.
        /// We can use <see cref="IRepository{TEntity,TPrimaryKey}"/> to get a repository service if this value is true. We also can use <see cref="IEfCoreRepository{TEntity,TPrimaryKey}"/> at the same time.
        /// </param>
        private static void RegisterRepositories<TDbContext>(IServiceCollection services, bool asMainOrmTechnology) where TDbContext : EasyNetDbContext
        {
            var dbContextType = typeof(TDbContext);
            var properties = dbContextType.GetProperties(BindingFlags.Instance | BindingFlags.Public);

            foreach (var property in properties)
            {
                // Try to get DbSet<> type collection
                if (property.PropertyType.IsGenericType &&
                    property.PropertyType.IsAbstract &&
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
                            // Add short service IEfCoreRepository<TEntity> if the id property type is int.
                            if (idProperty.PropertyType == typeof(int))
                            {
                                if (asMainOrmTechnology)
                                {
                                    services.TryAddTransient(
                                        typeof(IRepository<>).MakeGenericType(entityType),
                                        typeof(EfCoreRepositoryBase<,>).MakeGenericType(dbContextType, entityType));
                                }

                                services.TryAddTransient(
                                    typeof(IEfCoreRepository<>).MakeGenericType(entityType),
                                    typeof(EfCoreRepositoryBase<,>).MakeGenericType(dbContextType, entityType));
                            }

                            // Add service IEfCoreRepository<TEntity,TPrimaryKey>
                            if (asMainOrmTechnology)
                            {
                                services.TryAddTransient(
                                    typeof(IRepository<,>).MakeGenericType(entityType, idProperty.PropertyType),
                                    typeof(EfCoreRepositoryBase<,,>).MakeGenericType(dbContextType, entityType, idProperty.PropertyType));
                            }

                            services.TryAddTransient(
                                typeof(IEfCoreRepository<,>).MakeGenericType(entityType, idProperty.PropertyType),
                                typeof(EfCoreRepositoryBase<,,>).MakeGenericType(dbContextType, entityType, idProperty.PropertyType));
                        }
                    }
                }
            }
        }
    }
}
