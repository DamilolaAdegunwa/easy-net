using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace EasyNet.CommonTests
{
	public class DependencyInjectionTest
	{
		protected void AssertSpecifiedServiceTypeAndImplementationType<TServiceType, TImplementationType>(IServiceCollection services, ServiceLifetime lifetime, int count = 1)
		{
			var descriptors = services.Where(p => p.ServiceType == typeof(TServiceType)).ToList();

			Assert.Equal(count, descriptors.Count);
			Assert.Contains(descriptors, p => p.ImplementationType == typeof(TImplementationType) && p.Lifetime == lifetime);
		}
	}
}
