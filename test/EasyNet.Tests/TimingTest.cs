using System;
using EasyNet.Timing;
using Xunit;

namespace EasyNet.Tests
{
	public class TimingTest
	{
		[Fact]
		public void TestNow()
		{
			// Assert

			#region Unspecified

			Clock.Provider = ClockProviders.Unspecified;
			Assert.Equal(DateTimeKind.Unspecified, Clock.Kind);
			Assert.Equal(0, (Clock.Now - DateTime.Now).Seconds);

			#endregion

			#region Local

			Clock.Provider = ClockProviders.Local;
			Assert.Equal(DateTimeKind.Local, Clock.Kind);
			Assert.Equal(0, (Clock.Now - DateTime.Now).Seconds);

			#endregion

			#region Utc

			Clock.Provider = ClockProviders.Utc;
			Assert.Equal(DateTimeKind.Utc, Clock.Kind);
			Assert.Equal(0, (Clock.Now - DateTime.UtcNow).Seconds);

			#endregion
		}
	}
}
