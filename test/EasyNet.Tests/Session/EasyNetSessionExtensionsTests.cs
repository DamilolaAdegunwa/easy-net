using System;
using EasyNet.Extensions;
using EasyNet.Runtime.Session;
using Moq;
using Xunit;

namespace EasyNet.Tests.Session
{
	public class EasyNetSessionExtensionsTests
	{
		[Fact]
		public void TestGetIntUserId()
		{
			// Arrange
			var sessionMock1 = new Mock<IEasyNetSession>();
			sessionMock1.SetupGet(p => p.UserId).Returns("1");

			var sessionMock2 = new Mock<IEasyNetSession>();
			sessionMock2.SetupGet(p => p.UserId).Returns("");

			var sessionMock3 = new Mock<IEasyNetSession>();
			sessionMock3.SetupGet(p => p.UserId).Returns("incorrect");

			var sessionMock4 = new Mock<IEasyNetSession>();
			sessionMock4.SetupGet(p => p.UserId).Returns("2147483648");

			// Assert
			Assert.Equal(1, sessionMock1.Object.GetIntUserId());
			Assert.Throws<EasyNetException>(() => sessionMock2.Object.GetIntUserId());
			Assert.Throws<FormatException>(() => sessionMock3.Object.GetIntUserId());
			Assert.Throws<OverflowException>(() => sessionMock4.Object.GetIntUserId());
		}

		[Fact]
		public void TestGetIntUserIdOrNull()
		{
			// Arrange
			var sessionMock1 = new Mock<IEasyNetSession>();
			sessionMock1.SetupGet(p => p.UserId).Returns("1");

			var sessionMock2 = new Mock<IEasyNetSession>();
			sessionMock2.SetupGet(p => p.UserId).Returns("");

			var sessionMock3 = new Mock<IEasyNetSession>();
			sessionMock3.SetupGet(p => p.UserId).Returns("incorrect");

			var sessionMock4 = new Mock<IEasyNetSession>();
			sessionMock4.SetupGet(p => p.UserId).Returns("2147483648");

			// Assert
			Assert.Equal(1, sessionMock1.Object.GetIntUserIdOrNull());
			Assert.True(!sessionMock2.Object.GetIntUserIdOrNull().HasValue);
			Assert.Throws<FormatException>(() => sessionMock3.Object.GetIntUserIdOrNull());
			Assert.Throws<OverflowException>(() => sessionMock4.Object.GetIntUserIdOrNull());
		}

		[Fact]
		public void TestGetLongUserId()
		{
			// Arrange
			var sessionMock1 = new Mock<IEasyNetSession>();
			sessionMock1.SetupGet(p => p.UserId).Returns("1");

			var sessionMock2 = new Mock<IEasyNetSession>();
			sessionMock2.SetupGet(p => p.UserId).Returns("");

			var sessionMock3 = new Mock<IEasyNetSession>();
			sessionMock3.SetupGet(p => p.UserId).Returns("incorrect");

			var sessionMock4 = new Mock<IEasyNetSession>();
			sessionMock4.SetupGet(p => p.UserId).Returns("9223372036854775808");

			// Assert
			Assert.Equal(1, sessionMock1.Object.GetLongUserId());
			Assert.Throws<EasyNetException>(() => sessionMock2.Object.GetLongUserId());
			Assert.Throws<FormatException>(() => sessionMock3.Object.GetLongUserId());
			Assert.Throws<OverflowException>(() => sessionMock4.Object.GetLongUserId());
		}

		[Fact]
		public void TestGetLongUserIdOrNull()
		{
			// Arrange
			var sessionMock1 = new Mock<IEasyNetSession>();
			sessionMock1.SetupGet(p => p.UserId).Returns("1");

			var sessionMock2 = new Mock<IEasyNetSession>();
			sessionMock2.SetupGet(p => p.UserId).Returns("");

			var sessionMock3 = new Mock<IEasyNetSession>();
			sessionMock3.SetupGet(p => p.UserId).Returns("incorrect");

			var sessionMock4 = new Mock<IEasyNetSession>();
			sessionMock4.SetupGet(p => p.UserId).Returns("9223372036854775808");

			// Assert
			Assert.Equal(1, sessionMock1.Object.GetLongUserIdOrNull());
			Assert.True(!sessionMock2.Object.GetLongUserIdOrNull().HasValue);
			Assert.Throws<FormatException>(() => sessionMock3.Object.GetLongUserIdOrNull());
			Assert.Throws<OverflowException>(() => sessionMock4.Object.GetLongUserIdOrNull());
		}

		[Fact]
		public void TestGetGuidUserId()
		{
			// Arrange
			var sessionMock1 = new Mock<IEasyNetSession>();
			sessionMock1.SetupGet(p => p.UserId).Returns("9b85fde3-8d6b-3657-3dd9-87722875568b");

			var sessionMock2 = new Mock<IEasyNetSession>();
			sessionMock2.SetupGet(p => p.UserId).Returns("");

			var sessionMock3 = new Mock<IEasyNetSession>();
			sessionMock3.SetupGet(p => p.UserId).Returns("incorrect");

			// Assert
			Assert.Equal(Guid.Parse("9b85fde3-8d6b-3657-3dd9-87722875568b"), sessionMock1.Object.GetGuidUserId());
			Assert.Throws<EasyNetException>(() => sessionMock2.Object.GetLongUserId());
			Assert.Throws<FormatException>(() => sessionMock3.Object.GetLongUserId());
		}

		[Fact]
		public void TestGetGuidUserIdOrNull()
		{
			// Arrange
			var sessionMock1 = new Mock<IEasyNetSession>();
			sessionMock1.SetupGet(p => p.UserId).Returns("9b85fde3-8d6b-3657-3dd9-87722875568b");

			var sessionMock2 = new Mock<IEasyNetSession>();
			sessionMock2.SetupGet(p => p.UserId).Returns("");

			var sessionMock3 = new Mock<IEasyNetSession>();
			sessionMock3.SetupGet(p => p.UserId).Returns("incorrect");

			var sessionMock4 = new Mock<IEasyNetSession>();
			sessionMock4.SetupGet(p => p.UserId).Returns("9223372036854775808");

			// Assert
			Assert.Equal(Guid.Parse("9b85fde3-8d6b-3657-3dd9-87722875568b"), sessionMock1.Object.GetGuidUserIdOrNull());
			Assert.True(!sessionMock2.Object.GetLongUserIdOrNull().HasValue);
			Assert.Throws<FormatException>(() => sessionMock3.Object.GetLongUserIdOrNull());
		}
	}
}
