using System.Security.Claims;
using System.Threading;
using EasyNet.DependencyInjection;
using EasyNet.Runtime.Security;
using EasyNet.Runtime.Session;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace EasyNet.Tests.Session
{
	public class ClaimsEasyNetSessionTests
	{
		[Fact]
		public void TestEmptySession()
		{
			// Arrange
			var services = new ServiceCollection();

			// Act
			services.AddEasyNet();
			var serviceProvider = services.BuildServiceProvider();
			var session = serviceProvider.GetService<IEasyNetSession>();

			// Assert
			Assert.True(string.IsNullOrEmpty(session.UserId));
            Assert.True(string.IsNullOrEmpty(session.TenantId));
			Assert.True(string.IsNullOrEmpty(session.UserName));
			Assert.True(string.IsNullOrEmpty(session.Role));
		}

		[Fact]
		public void TestSession()
		{
			// Arrange
			var services = new ServiceCollection();

			var principal = new ClaimsPrincipal();
			var identity = new ClaimsIdentity();
			identity.AddClaim(new Claim(EasyNetClaimTypes.UserId, "1"));
            identity.AddClaim(new Claim(EasyNetClaimTypes.TenantId, "2"));
			identity.AddClaim(new Claim(EasyNetClaimTypes.UserName, "Test"));
			identity.AddClaim(new Claim(EasyNetClaimTypes.Role, "Admin"));
			principal.AddIdentity(identity);
			
			// Act
			services.AddEasyNet();
			var serviceProvider = services.BuildServiceProvider();
			Thread.CurrentPrincipal = principal;
			var session = serviceProvider.GetService<IEasyNetSession>();

			// Assert
			Assert.Equal("1", session.UserId);
            Assert.Equal("2", session.TenantId);
			Assert.Equal("Test", session.UserName);
			Assert.Equal("Admin", session.Role);
		}
	}
}
