using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace EasyNet.Tests.Extensions
{
    public class PropertyInfoExtensionsTests
    {
        [Fact]
        public void TestSetValue()
        {
            // Arrange
            var test = new TestClass();
            var type = test.GetType();

            // Act && Assert
            type.GetProperty("Short")?.SetValue(test, 1);
            Assert.Equal(1, test.Short);
        }
    }

    public class TestClass
    {
        public short Short { get; set; }

        public short? ShortOrNull { get; set; }
    }
}
