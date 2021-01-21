using System;
using EasyNet.DependencyInjection;
using EasyNet.EntityFrameworkCore;
using EasyNet.Identity.EntityFrameworkCore.Domain;
using EasyNet.Identity.EntityFrameworkCore.Domain.Entities;
using EasyNet.Identity.EntityFrameworkCore.Initialization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace EasyNet.Identity.EntityFrameworkCore.DependencyInjection
{
    /// <summary>
    /// Extension methods for setting up EasyNet.Identity services in an <see cref="IEasyNetBuilder" />.
    /// </summary>
    public static class EasyNetBuilderExtensions
    {
        public static IEasyNetBuilder AddIdentityCore<TDbContext, TUser>(
            this IEasyNetBuilder builder,
            Action<IdentityOptions> identitySetupAction = null,
            Action<AuthenticationOptions> authenticationSetupAction = null)
            where TDbContext : EasyNetDbContext
            where TUser : class
        {
            Check.NotNull(builder, nameof(builder));

            builder.Services
                .AddAuthentication(o =>
                {
                    o.DefaultScheme = IdentityConstants.ApplicationScheme;
                    o.DefaultSignInScheme = IdentityConstants.ExternalScheme;
                    authenticationSetupAction?.Invoke(o);
                });

            builder.Services
                .AddIdentityCore<TUser>(o =>
                {
                    o.Stores.MaxLengthForKeys = 128;
                    identitySetupAction?.Invoke(o);
                })
                .AddUserManager<EasyNetUserManager<TUser>>()
                .AddSignInManager<EasyNetSignInManager<TUser>>()
                .AddEntityFrameworkStores<TDbContext>();

            builder.Services.TryAddScoped<IEasyNetGeneralSignInManager, EasyNetSignInManager<TUser>>();

            builder.Services.Configure<DefaultAdminUserOptions>(o => { });

            return builder;
        }

        public static IEasyNetBuilder AddIdentity<TUser>(
            this IEasyNetBuilder builder,
            Action<IdentityBuilder> identitySetupAction,
            Action<AuthenticationOptions> authenticationSetupAction = null) where TUser : class
        {
            Check.NotNull(builder, nameof(builder));
            Check.NotNull(identitySetupAction, nameof(identitySetupAction));

            builder.Services
                .AddAuthentication(o =>
                {
                    o.DefaultScheme = IdentityConstants.ApplicationScheme;
                    o.DefaultSignInScheme = IdentityConstants.ExternalScheme;
                    authenticationSetupAction?.Invoke(o);
                });

            builder.Services.TryAddScoped<IEasyNetGeneralSignInManager, EasyNetSignInManager<TUser>>();

            builder.Services.Configure<DefaultAdminUserOptions>(o => { });

            identitySetupAction(new IdentityBuilder(typeof(TUser), builder.Services));

            return builder;
        }

        /// <summary>
        /// Configure <see cref="DefaultAdminUserOptions"/>
        /// </summary>
        /// <param name="builder">The <see cref="IEasyNetBuilder"/>.</param>
        /// <param name="setupAction">An <see cref="Action{DefaultAdminUserOptions}"/> to configure the provided <see cref="DefaultAdminUserOptions"/>.</param>
        /// <returns>An <see cref="IEasyNetBuilder"/> that can be used to further configure the EasyNet services.</returns>
        public static IEasyNetBuilder ConfigureDefaultAdminUserOptions(this IEasyNetBuilder builder, Action<DefaultAdminUserOptions> setupAction)
        {
            Check.NotNull(builder, nameof(builder));
            Check.NotNull(setupAction, nameof(setupAction));

            builder.Services.Configure(setupAction);

            return builder;
        }

        /// <summary>
        /// Add a default admin when EasyNet initialization.
        /// </summary>
        /// <typeparam name="TUser">The user associated with the application.</typeparam>
        /// <param name="builder">The <see cref="IEasyNetBuilder"/>.</param>
        /// <returns></returns>
        public static IEasyNetBuilder AddAdminInitializationJob<TUser>(this IEasyNetBuilder builder)
            where TUser : EasyNetUser<int>, new()
        {
            Check.NotNull(builder, nameof(builder));

            builder.AddInitializationJob(typeof(AdminInitializationJob<TUser, int>));

            return builder;
        }

        /// <summary>
        /// Add a default admin when EasyNet initialization.
        /// </summary>
        /// <typeparam name="TUser">The user associated with the application.</typeparam>
        /// <typeparam name="TPrimaryKey">The primary key of the user associated with the application.</typeparam>
        /// <param name="builder">The <see cref="IEasyNetBuilder"/>.</param>
        /// <returns></returns>
        public static IEasyNetBuilder AddAdminInitializationJob<TUser, TPrimaryKey>(this IEasyNetBuilder builder)
            where TUser : EasyNetUser<TPrimaryKey>, new()
            where TPrimaryKey : IEquatable<TPrimaryKey>
        {
            Check.NotNull(builder, nameof(builder));

            builder.AddInitializationJob(typeof(AdminInitializationJob<TUser, TPrimaryKey>));

            return builder;
        }
    }
}
