using System;
using System.ComponentModel;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Shibusa.Transformations
{
    public class EnumDescriptionConverterFactory : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert) => typeToConvert.IsEnum;

        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options) =>
            (JsonConverter)Activator.CreateInstance(
                typeof(EnumConverterInner<>).MakeGenericType(new Type[] { typeToConvert }),
                BindingFlags.Instance | BindingFlags.Public,
                binder: null,
                args: null,
                culture: null);

        private class EnumConverterInner<T> : JsonConverter<T> where T : struct, Enum
        {
            public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                string propertyName = reader.GetString();

                MemberInfo[] members = typeToConvert.GetMembers(BindingFlags.Public | BindingFlags.Static);
                foreach (MemberInfo member in members)
                {
                    var attrs = member.GetCustomAttributes(typeof(DescriptionAttribute), false);
                    if (attrs.Length > 0)
                    {
                        for (int i = 0; i < attrs.Length; i++)
                        {
                            string description = ((DescriptionAttribute)attrs[i]).Description;
                            if (propertyName.Equals(description, StringComparison.OrdinalIgnoreCase))
                            {
                                return (T)Enum.Parse(typeToConvert, member.Name, true);
                            }
                        }
                    }

                    if (member.Name.Equals(propertyName, StringComparison.OrdinalIgnoreCase))
                    {
                        return (T)Enum.Parse(typeToConvert, member.Name, true);
                    }
                }

                if (!Enum.TryParse(propertyName, ignoreCase: false, out T result) &&
                        !Enum.TryParse(propertyName, ignoreCase: true, out result))
                {
                    throw new JsonException(
                        $"Unable to convert \"{propertyName}\" to Enum \"{typeToConvert}\".");
                }

                return result;
            }

            public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
            {
                Type type = value.GetType();

                var memberInfo = type.GetMember(value.ToString());
                if (memberInfo.Length > 0)
                {
                    var attrs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                    if (attrs.Length > 0)
                    {
                        var description = ((DescriptionAttribute)attrs[0]).Description;
                        writer.WriteStringValue(options.PropertyNamingPolicy?.ConvertName(description) ?? description);
                    }
                    else
                    {
                        writer.WriteStringValue(options.PropertyNamingPolicy?.ConvertName(value.ToString()) ?? value.ToString());
                    }
                }
            }
        }
    }
}
