﻿using System;
using System.Transactions;
using EasyNet.CommonTests;
using EasyNet.DependencyInjection;
using EasyNet.Domain.Uow;
using EasyNet.Runtime.Session;
using EasyNet.Tests.Session;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace EasyNet.Tests
{
	public class EasyNetBuilderExtensionsTest : DependencyInjectionTest
	{
		[Fact]
		public void TestConfigureUnitOfWorkDefaultOptions()
		{
			// Arrange
			var services = new ServiceCollection();
			services.AddSingleton(CommonTest.GetHostingEnvironment());

			// Act
			services
				.AddEasyNet()
				.ConfigureUnitOfWorkDefaultOptions(options =>
				{
					options.IsTransactional = false;
					options.Scope = TransactionScopeOption.Suppress;
					options.Timeout = TimeSpan.Zero;
					options.IsolationLevel = IsolationLevel.RepeatableRead;
				});

			var serviceProvider = services.BuildServiceProvider();
			var defaultOptions = serviceProvider.GetRequiredService<IOptions<UnitOfWorkDefaultOptions>>().Value;

			// Assert
			Assert.Equal(false, defaultOptions.IsTransactional);
			Assert.Equal(TransactionScopeOption.Suppress, defaultOptions.Scope);
			Assert.Equal(TimeSpan.Zero, defaultOptions.Timeout);
			Assert.Equal(IsolationLevel.RepeatableRead, defaultOptions.IsolationLevel);
		}

		[Fact]
		public void TestAddSession()
		{
			// Arrange
			var services = new ServiceCollection();
			services.AddSingleton(CommonTest.GetHostingEnvironment());

			// Act
			services
				.AddEasyNet()
				.AddSession<TestSession>();

			var serviceProvider = services.BuildServiceProvider();
			var session = serviceProvider.GetRequiredService<IEasyNetSession>();

			// Assert
			AssertSpecifiedServiceTypeAndImplementationType<IEasyNetSession, TestSession>(services, ServiceLifetime.Scoped);
			Assert.Equal("1", session.UserId);
			Assert.Equal("Test", session.UserName);
			Assert.Equal("Admin", session.Role);
		}
	}
}
