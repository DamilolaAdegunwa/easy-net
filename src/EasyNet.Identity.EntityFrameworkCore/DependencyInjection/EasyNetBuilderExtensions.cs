using System;
using EasyNet.DependencyInjection;
using EasyNet.EntityFrameworkCore;
using EasyNet.Identity.EntityFrameworkCore.Domain;
using EasyNet.Identity.EntityFrameworkCore.Domain.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace EasyNet.Identity.EntityFrameworkCore.DependencyInjection
{
    /// <summary>
    /// Extension methods for setting up EasyNet.Identity services in an <see cref="IEasyNetBuilder" />.
    /// </summary>
    public static class EasyNetBuilderExtensions
    {
        public static IEasyNetBuilder AddIdentityCore<TDbContext>(
            this IEasyNetBuilder builder,
            Action<IdentityOptions> identitySetupAction = null,
            Action<AuthenticationOptions> authenticationSetupAction = null)
            where TDbContext : EasyNetDbContext
        {
            Check.NotNull(builder, nameof(builder));

            return builder.AddIdentityCore<TDbContext, EasyNetUser>(identitySetupAction, authenticationSetupAction);
        }

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

            return builder;
        }

        public static IEasyNetBuilder AddIdentity<TUser>(
            this IEasyNetBuilder builder, 
            Action<IdentityBuilder> identitySetupAction,
            Action<AuthenticationOptions> authenticationSetupAction = null)
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

            identitySetupAction(new IdentityBuilder(typeof(TUser), builder.Services));

            return builder;
        }
    }
}
