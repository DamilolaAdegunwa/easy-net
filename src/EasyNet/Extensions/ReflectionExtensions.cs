using System;
using System.Reflection;

namespace EasyNet.Extensions
{
    /// <summary>
    /// Extension methods in an <see cref="object" />.
    /// </summary>
    public static class ReflectionExtensions
    {
        /// <summary>
        /// Gets the value of the private field.
        /// </summary>
        /// <typeparam name="T">The type of the field.</typeparam>
        /// <param name="instance">The instance which the property belong to.</param>
        /// <param name="fieldName">The field name.</param>
        /// <returns>The value of the private field.</returns>
        /// <exception cref="InvalidOperationException">Throws exception if there is no private field by name.</exception>
        public static T GetPrivateField<T>(this object instance, string fieldName)
        {
            Check.NotNullOrEmpty(fieldName, nameof(fieldName));

            var type = instance.GetType();

            var fieldInfo = GetPrivateFieldInfo(type, fieldName);

            if (fieldInfo == null) throw new InvalidOperationException($"Cannot found private field {fieldName} in object {type.FullName}.");

            return (T)fieldInfo.GetValue(instance);
        }


        private static FieldInfo GetPrivateFieldInfo(Type type, string fieldName)
        {
            var fieldInfo = type.GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);

            if (fieldInfo == null)
            {
                if (type.BaseType != null && type.BaseType != typeof(object))
                {
                    return GetPrivateFieldInfo(type.BaseType, fieldName);
                }
                else
                {
                    return null;
                }
            }

            return fieldInfo;
        }

        /// <summary>
        /// Gets the value of the private property.
        /// </summary>
        /// <typeparam name="T">The type of the property.</typeparam>
        /// <param name="instance">The instance which the property belong to.</param>
        /// <param name="propertyName">The property name.</param>
        /// <returns>The value of the private property.</returns>
        /// <exception cref="InvalidOperationException">Throws exception if there is no private property by name.</exception>
        public static T GetPrivateProperty<T>(this object instance, string propertyName)
        {
            Check.NotNullOrEmpty(propertyName, nameof(propertyName));

            var type = instance.GetType();

            var propertyInfo = type.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.NonPublic);

            if (propertyInfo == null) throw new InvalidOperationException($"Cannot found private property {propertyName} in object {type.FullName}.");

            return (T)propertyInfo.GetValue(instance, null);
        }
    }
}
