using System;
using System.Collections.Generic;
using System.Text;
using EasyNet.Extensions;
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

            #region short

            type.GetProperty("Short")?.SetValue<short>(test, 1);
            Assert.Equal(1, test.Short);

            type.GetProperty("Short")?.SetValue<short>(test, null);
            Assert.Equal(0, test.Short);

            type.GetProperty("ShortOrNull")?.SetValue<short?>(test, 2);
            Assert.Equal(2, test.ShortOrNull.Value);

            type.GetProperty("ShortOrNull")?.SetValue<short?>(test, null);
            Assert.Null(test.ShortOrNull);

            #endregion

            #region int

            type.GetProperty("Int")?.SetValue<int>(test, 1);
            Assert.Equal(1, test.Int);

            type.GetProperty("Int")?.SetValue<int>(test, null);
            Assert.Equal(0, test.Int);

            type.GetProperty("IntOrNull")?.SetValue<int?>(test, 2);
            Assert.Equal(2, test.IntOrNull.Value);

            type.GetProperty("IntOrNull")?.SetValue<int?>(test, null);
            Assert.Null(test.IntOrNull);

            #endregion

            #region long

            type.GetProperty("Long")?.SetValue<long>(test, 1);
            Assert.Equal(1, test.Long);

            type.GetProperty("Long")?.SetValue<long>(test, null);
            Assert.Equal(0, test.Long);

            type.GetProperty("LongOrNull")?.SetValue<long?>(test, 2);
            Assert.Equal(2, test.LongOrNull.Value);

            type.GetProperty("LongOrNull")?.SetValue<long?>(test, null);
            Assert.Null(test.LongOrNull);

            #endregion

            #region DateTime

            var now = DateTime.Now;

            type.GetProperty("DateTime")?.SetValue<DateTime>(test, now);
            Assert.Equal(now, test.DateTime);

            type.GetProperty("DateTime")?.SetValue<DateTime>(test, null);
            Assert.Equal(default(DateTime), test.DateTime);

            type.GetProperty("DateTimeOrNull")?.SetValue<DateTime?>(test, now);
            Assert.Equal(now, test.DateTimeOrNull.Value);

            type.GetProperty("DateTimeOrNull")?.SetValue<DateTime?>(test, null);
            Assert.Null(test.DateTimeOrNull);

            #endregion

            #region float

            type.GetProperty("Float")?.SetValue<float>(test, 1);
            Assert.Equal(1, test.Float);

            type.GetProperty("Float")?.SetValue<float>(test, null);
            Assert.Equal(0, test.Float);

            type.GetProperty("FloatOrNull")?.SetValue<float?>(test, 2);
            Assert.Equal(2, test.FloatOrNull.Value);

            type.GetProperty("FloatOrNull")?.SetValue<float?>(test, null);
            Assert.Null(test.FloatOrNull);

            #endregion

            #region double

            type.GetProperty("Double")?.SetValue<double>(test, 1);
            Assert.Equal(1, test.Double);

            type.GetProperty("Double")?.SetValue<double>(test, null);
            Assert.Equal(0, test.Double);

            type.GetProperty("DoubleOrNull")?.SetValue<double?>(test, 2);
            Assert.Equal(2, test.DoubleOrNull.Value);

            type.GetProperty("DoubleOrNull")?.SetValue<double?>(test, null);
            Assert.Null(test.DoubleOrNull);

            #endregion

            #region decimal

            type.GetProperty("Decimal")?.SetValue<decimal>(test, 1);
            Assert.Equal(1, test.Decimal);

            type.GetProperty("Decimal")?.SetValue<decimal>(test, null);
            Assert.Equal(0, test.Decimal);

            type.GetProperty("DecimalOrNull")?.SetValue<decimal?>(test, 2);
            Assert.Equal(2, test.DecimalOrNull.Value);

            type.GetProperty("DecimalOrNull")?.SetValue<decimal?>(test, null);
            Assert.Null(test.DecimalOrNull);

            #endregion

            #region byte

            type.GetProperty("Byte")?.SetValue<byte>(test, 1);
            Assert.Equal(1, test.Byte);

            type.GetProperty("Byte")?.SetValue<byte>(test, null);
            Assert.Equal(default(byte), test.Byte);

            type.GetProperty("ByteOrNull")?.SetValue<byte?>(test, 2);
            Assert.Equal(2, test.ByteOrNull.Value);

            type.GetProperty("ByteOrNull")?.SetValue<byte?>(test, null);
            Assert.Null(test.ByteOrNull);

            #endregion

            #region char

            type.GetProperty("Char")?.SetValue<char>(test, '1');
            Assert.Equal('1', test.Char);

            type.GetProperty("Char")?.SetValue<char>(test, null);
            Assert.Equal(default(char), test.Char);

            type.GetProperty("CharOrNull")?.SetValue<char?>(test, '2');
            Assert.Equal('2', test.CharOrNull.Value);

            type.GetProperty("CharOrNull")?.SetValue<char?>(test, null);
            Assert.Null(test.CharOrNull);

            #endregion

            #region Guid

            type.GetProperty("Guid")?.SetValue<Guid>(test, "E82EFFB7-9577-4856-7FF8-F7839BF6D140");
            Assert.Equal(Guid.Parse("E82EFFB7-9577-4856-7FF8-F7839BF6D140"), test.Guid);

            type.GetProperty("Guid")?.SetValue<Guid>(test, null);
            Assert.Equal(default(Guid), test.Guid);

            type.GetProperty("GuidOrNull")?.SetValue<Guid?>(test, "960888E9-E921-3BFA-75ED-5BC6D954F328");
            Assert.Equal(Guid.Parse("960888E9-E921-3BFA-75ED-5BC6D954F328"), test.GuidOrNull.Value);

            type.GetProperty("GuidOrNull")?.SetValue<Guid?>(test, null);
            Assert.Null(test.GuidOrNull);

            #endregion
        }
    }

    public class TestClass
    {
        public short Short { get; set; }

        public short? ShortOrNull { get; set; }

        public int Int { get; set; }

        public int? IntOrNull { get; set; }

        public long Long { get; set; }

        public long? LongOrNull { get; set; }

        public DateTime DateTime { get; set; }

        public DateTime? DateTimeOrNull { get; set; }

        public float Float { get; set; }

        public float? FloatOrNull { get; set; }

        public double Double { get; set; }

        public double? DoubleOrNull { get; set; }

        public decimal Decimal { get; set; }

        public decimal? DecimalOrNull { get; set; }

        public byte Byte { get; set; }

        public byte? ByteOrNull { get; set; }

        public char Char { get; set; }

        public char? CharOrNull { get; set; }

        public Guid Guid { get; set; }

        public Guid? GuidOrNull { get; set; }
    }
}