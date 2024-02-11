#pragma warning disable CS8600
#pragma warning disable CS8603
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace PortfolioV2.Core
{
    public static class EnumHelper
    {
        public static string GetDisplayName(this Enum enumValue)
        {
            return enumValue.GetType()
              .GetMember(enumValue.ToString())
              .First()
              .GetCustomAttribute<DisplayAttribute>()
              ?.GetName();
        }

        public static T GetValueFromName<T>(this string name) where T : Enum
        {
            Type type = typeof(T);

            foreach (FieldInfo field in type.GetFields())
            {
                if (Attribute.GetCustomAttribute(field, typeof(DisplayAttribute)) is DisplayAttribute attribute)
                {
                    if (attribute.Name == name)
                    {
                        return (T)field.GetValue(null);
                    }
                }

                if (field.Name == name)
                {
                    return (T)field.GetValue(null);
                }
            }

            throw new ArgumentException($"{name} not found", nameof(name));
        }
    }
}