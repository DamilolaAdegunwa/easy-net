using System;
using EasyNet.DependencyInjection;
using EasyNet.EntityFrameworkCore;
using EasyNet.Identity.EntityFrameworkCore.Domain;
using EasyNet.Identity.EntityFrameworkCore.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace EasyNet.Identity.EntityFrameworkCore.DependencyInjection
{
    /// <summary>
    /// Extension methods for setting up EasyNet.Identity services in an <see cref="IEasyNetBuilder" />.
    /// </summary>
    public static class EasyNetBuilderExtensions
    {
	    public static IEasyNetBuilder AddIdentityCore<TDbContext>(this IEasyNetBuilder builder, Action<IdentityOptions> setupAction)
		    where TDbContext : EasyNetDbContext
	    {
		    Check.NotNull(builder, nameof(builder));

		    return builder.AddIdentityCore<TDbContext, EasyNetUser>(setupAction);
	    }

        public static IEasyNetBuilder AddIdentityCore<TDbContext, TUser>(this IEasyNetBuilder builder, Action<IdentityOptions> setupAction) 
            where  TDbContext : EasyNetDbContext
            where TUser : class
        {
            Check.NotNull(builder, nameof(builder));

            builder.Services
                .AddIdentityCore<TUser>(setupAction)
                .AddUserManager<EasyNetUserManager<TUser>>()
                .AddSignInManager<EasyNetSignInManager<TUser>>()
                .AddEntityFrameworkStores<TDbContext>();

            return builder;
        }

        public static IEasyNetBuilder AddIdentity<TUser>(this IEasyNetBuilder builder, Action<IdentityBuilder> setupAction)
        {
            Check.NotNull(builder, nameof(builder));
            Check.NotNull(setupAction, nameof(setupAction));

            setupAction(new IdentityBuilder(typeof(TUser), builder.Services));

            return builder;
        }
    }
}
