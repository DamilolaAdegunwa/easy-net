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

            // Act && Assert
        }
    }

    public class TestClass
    {
        public short Short { get; set; }

        public short? ShortOrNull { get; set; }
    }
}
