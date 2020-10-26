using System;
using EasyNet.Domain.Uow;
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
	}
}
