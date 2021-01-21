using System;
using EasyNet.DependencyInjection;
using EasyNet.EntityFrameworkCore;
using EasyNet.Identity.EntityFrameworkCore.Domain;
using EasyNet.Identity.EntityFrameworkCore.Domain.Entities;
using EasyNet.Identity.EntityFrameworkCore.Initialization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
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
        public static IEasyNetBuilder AddIdentityCore<TUser, TDbContext>(
            this IEasyNetBuilder builder,
            Action<IdentityOptions> identitySetupAction = null,
            Action<AuthenticationOptions> authenticationSetupAction = null,
            Action<IdentityCookiesBuilder> identityCookiesSetupAction = null)
            where TUser : EasyNetUser<int>
            where TDbContext : EasyNetDbContext
        {
            return builder.AddIdentityCore<TUser, TDbContext, int>(identitySetupAction, authenticationSetupAction, identityCookiesSetupAction);
        }

        public static IEasyNetBuilder AddIdentityCore<TUser, TDbContext, TPrimaryKey>(
            this IEasyNetBuilder builder,
            Action<IdentityOptions> identitySetupAction = null,
            Action<AuthenticationOptions> authenticationSetupAction = null,
            Action<IdentityCookiesBuilder> identityCookiesSetupAction = null)
            where TUser : EasyNetUser<TPrimaryKey>
            where TDbContext : EasyNetDbContext
            where TPrimaryKey : IEquatable<TPrimaryKey>
        {
            return builder.AddIdentityCore<TUser, EasyNetRole<TPrimaryKey>, TDbContext, TPrimaryKey>(identitySetupAction, authenticationSetupAction, identityCookiesSetupAction);
        }

        public static IEasyNetBuilder AddIdentityCore<TUser, TRole, TDbContext, TPrimaryKey>(
            this IEasyNetBuilder builder,
            Action<IdentityOptions> identitySetupAction = null,
            Action<AuthenticationOptions> authenticationSetupAction = null,
            Action<IdentityCookiesBuilder> identityCookiesSetupAction = null)
            where TUser : EasyNetUser<TPrimaryKey>
            where TRole : EasyNetRole<TPrimaryKey>
            where TDbContext : EasyNetDbContext
            where TPrimaryKey : IEquatable<TPrimaryKey>
        {
            return builder.AddIdentityCore<TUser, TRole, TDbContext, TPrimaryKey, EasyNetUserClaim<TPrimaryKey>, EasyNetUserRole<TPrimaryKey>, EasyNetUserLogin<TPrimaryKey>, EasyNetUserToken<TPrimaryKey>, EasyNetRoleClaim<TPrimaryKey>>(
                identitySetupAction,
                authenticationSetupAction,
                identityCookiesSetupAction);
        }

        public static IEasyNetBuilder AddIdentityCore<TUser, TRole, TDbContext, TPrimaryKey, TUserClaim, TUserRole, TUserLogin, TUserToken, TRoleClaim>(
            this IEasyNetBuilder builder,
            Action<IdentityOptions> identitySetupAction = null,
            Action<AuthenticationOptions> authenticationSetupAction = null,
            Action<IdentityCookiesBuilder> identityCookiesSetupAction = null)
            where TUser : EasyNetUser<TPrimaryKey>
            where TRole : EasyNetRole<TPrimaryKey>
            where TDbContext : Microsoft.EntityFrameworkCore.DbContext
            where TPrimaryKey : IEquatable<TPrimaryKey>
            where TUserClaim : EasyNetUserClaim<TPrimaryKey>, new()
            where TUserRole : EasyNetUserRole<TPrimaryKey>, new()
            where TUserLogin : EasyNetUserLogin<TPrimaryKey>, new()
            where TUserToken : EasyNetUserToken<TPrimaryKey>, new()
            where TRoleClaim : EasyNetRoleClaim<TPrimaryKey>, new()
        {
            Check.NotNull(builder, nameof(builder));

            builder.Services
                .AddAuthentication(o =>
                {
                    o.DefaultScheme = IdentityConstants.ApplicationScheme;
                    o.DefaultSignInScheme = IdentityConstants.ExternalScheme;
                    authenticationSetupAction?.Invoke(o);
                })
                .AddIdentityCookies(identityCookiesSetupAction);

            builder.Services
                .AddIdentityCore<TUser>(o =>
                {
                    o.Stores.MaxLengthForKeys = 128;
                    identitySetupAction?.Invoke(o);
                });

            var identityBuilder = new IdentityBuilder(typeof(TUser), typeof(TRole), builder.Services);
            identityBuilder.AddUserManager<EasyNetUserManager<TUser, TPrimaryKey>>()
                .AddSignInManager<EasyNetSignInManager<TUser, TPrimaryKey>>()
                .AddUserStore<EasyNetUserStore<TUser, TRole, TDbContext, TPrimaryKey, TUserClaim, TUserRole, TUserLogin, TUserToken, TRoleClaim>>()
                .AddRoleStore<EasyNetRoleStore<TRole, TDbContext, TPrimaryKey, TUserRole, TRoleClaim>>();

            builder.Services.TryAddScoped<IEasyNetGeneralSignInManager, EasyNetSignInManager<TUser, TPrimaryKey>>();

            builder.Services.Configure<DefaultAdminUserOptions>(o => { });

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
