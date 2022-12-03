using System.ComponentModel;
using System.Reflection;

namespace Shibusa.Extensions;

/// <summary>
/// Enum extensions.
/// </summary>
public static class EnumExtensions
{
    /// <summary>
    /// Get the description of a specific enum value.
    /// </summary>
    /// <typeparam name="T">The type of enum.</typeparam>
    /// <param name="enumerationValue">The enumeration value.</param>
    /// <returns>The description of the enumeration value.</returns>
    public static string GetDescription<T>(this T enumerationValue) where T : struct, Enum
    {
        var type = enumerationValue.GetType();
        if (!type.IsEnum)
        {
            throw new ArgumentException($"{nameof(T)} must be of type Enum.");
        }
        var memberInfo = type.GetMember(enumerationValue.ToString());
        if (memberInfo.Length > 0)
        {
            var attrs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attrs.Length > 0)
            {
                return ((DescriptionAttribute)attrs[0]).Description;
            }
        }
        return enumerationValue.ToString();
    }

    /// <summary>
    /// Get an enumeration value that corresponds to the provided description.
    /// </summary>
    /// <typeparam name="T">The type of enum.</typeparam>
    /// <param name="description">The description value.</param>
    /// <returns>The enum value.</returns>
    public static T GetEnum<T>(this string text) where T : struct, Enum
    {
        var type = typeof(T);
        if (!type.IsEnum)
        {
            throw new ArgumentException($"{nameof(T)} must be of type Enum.");
        }
        MemberInfo[] members = type.GetMembers(BindingFlags.Public | BindingFlags.Static);
        foreach (MemberInfo member in members)
        {
            var attrs = member.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attrs.Length > 0)
            {
                for (int i = 0; i < attrs.Length; i++)
                {
                    string description = ((DescriptionAttribute)attrs[i]).Description;
                    if (text.Equals(description, StringComparison.OrdinalIgnoreCase))
                    {
                        return (T)Enum.Parse(type, member.Name, true);
                    }
                }
            }
            if (member.Name.Equals(text, StringComparison.OrdinalIgnoreCase))
            {
                return (T)Enum.Parse(type, member.Name, true);
            }
        }
        return default;
    }

    /// <summary>
    /// Gets all descriptions for an enum.
    /// </summary>
    /// <typeparam name="T">The type of enum.</typeparam>
    /// <returns>A collection of descriptions for the enum.</returns>
    public static IEnumerable<string> GetDescriptions<T>() where T : struct, Enum
    {
        MemberInfo[] members = typeof(T).GetMembers(BindingFlags.Public | BindingFlags.Static);
        foreach (MemberInfo member in members)
        {
            var attrs = member.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attrs.Length > 0)
            {
                for (int i = 0; i < attrs.Length; i++)
                {
                    string description = ((DescriptionAttribute)attrs[i]).Description;

                    yield return description;
                }
            }
            else
            {
                yield return member.Name;
            }
        }
    }
}
