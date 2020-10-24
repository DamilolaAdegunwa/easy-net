using System;
using EasyNet.Domain.Uow;
using Microsoft.Extensions.DependencyInjection;

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
	}
}
