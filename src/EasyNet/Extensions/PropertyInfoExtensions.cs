using System;
using System.Reflection;

namespace EasyNet.Extensions
{
    public static class PropertyInfoExtensions
    {
        /// <summary>
        /// Sets the value of the property of the object with specified type.
        /// </summary>
        public static void SetValue(this PropertyInfo propertyInfo, object obj, object value, Type valueType)
        {
            Check.NotNull(propertyInfo, nameof(propertyInfo));
            Check.NotNull(obj, nameof(obj));

            if (value == null)
            {
                propertyInfo.SetValue(obj, null);
                return;
            }

            if (valueType == typeof(string))
            {
                propertyInfo.SetValue(obj, value.ToString());
            }
            else if (valueType == typeof(short))
            {
                propertyInfo.SetValue(obj, Convert.ToInt16(value));
            }
            else if (valueType == typeof(int))
            {
                propertyInfo.SetValue(obj, Convert.ToInt32(value));
            }
            else if (valueType == typeof(long))
            {
                propertyInfo.SetValue(obj, Convert.ToInt64(value));
            }
            else if (valueType == typeof(DateTime))
            {
                propertyInfo.SetValue(obj, Convert.ToDateTime(value));
            }
            else if (valueType == typeof(float))
            {
                propertyInfo.SetValue(obj, Convert.ToSingle(value));
            }
            else if (valueType == typeof(double))
            {
                propertyInfo.SetValue(obj, Convert.ToDouble(value));
            }
            else if (valueType == typeof(decimal))
            {
                propertyInfo.SetValue(obj, Convert.ToDecimal(value));
            }
            else if (valueType == typeof(byte))
            {
                propertyInfo.SetValue(obj, Convert.ToByte(value));
            }
            else if (valueType == typeof(Guid))
            {
                propertyInfo.SetValue(obj, Guid.Parse(value.ToString()));
            }
            else
            {
                throw new InvalidOperationException($"Not support {valueType.AssemblyQualifiedName}.");
            }
        }
    }
}
