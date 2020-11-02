using System;
using EasyNet.DependencyInjection;
using EasyNet.EntityFrameworkCore;
using EasyNet.Identity.EntityFrameworkCore.Domain;
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

            identitySetupAction(new IdentityBuilder(typeof(TUser), builder.Services));

            return builder;
        }
    }
}
