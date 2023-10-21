using System;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ExtensionMethods.JsonSerializerConverts
{
#if NET5_0_OR_GREATER
	/// <summary>
	/// 枚举转换器 将枚举转换为EnumMember 或者从EnumMember转换为枚举
	/// </summary>
	public class JsonEnumMemberJsonConverter : JsonConverter<ValueType>
	{
		/// <inheritdoc/>
		public override bool CanConvert(Type typeToConvert)
		{
			return typeToConvert.IsEnum || typeToConvert.IsGenericType && typeToConvert.GenericTypeArguments.Length == 1 && typeToConvert.GenericTypeArguments[0].IsEnum;
		}
		/// <inheritdoc/>
		public override ValueType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			var s = reader.GetString();
			if (typeToConvert.IsEnum)
			{
				foreach (var x in Enum.GetNames(typeToConvert))
				{
					var member = typeToConvert.GetMember(x)[0];
					EnumMemberAttribute? attributes = member.GetCustomAttribute<EnumMemberAttribute>(false);
					if (attributes is null) continue;
					if (attributes.Value == s) return (ValueType)Enum.Parse(typeToConvert, x);
				}
			}
			else
			{
				foreach (var x in Enum.GetNames(typeToConvert.GenericTypeArguments[0]))
				{
					var member = typeToConvert.GenericTypeArguments[0].GetMember(x)[0];
					EnumMemberAttribute? attributes = member.GetCustomAttribute<EnumMemberAttribute>(false);
					if (attributes is null) continue;
					if (attributes.Value == s) return (ValueType)Enum.Parse(typeToConvert.GenericTypeArguments[0], x);
				}
			}
			throw new JsonException($"The value {s} can't convert to enum in position {reader.Position}F");
		}
		/// <inheritdoc/>
		public override void Write(Utf8JsonWriter writer, ValueType value, JsonSerializerOptions options)
		{
			if (value == null)
				writer.WriteNullValue();
			else
				writer.WriteStringValue(GetEnumMember((value as Enum)!));
		}
		/// <summary>
		/// get EnumMember value 如果不存在则返回字符串
		/// </summary>
		/// <param name="source"></param>
		/// <returns></returns>
		static string GetEnumMember(Enum source)
		{
			System.Reflection.FieldInfo? fi = source.GetType().GetField(source.ToString());
			if (fi == null) return source.ToString();
			EnumMemberAttribute? attributes = fi.GetCustomAttribute<EnumMemberAttribute>(false);

			if (attributes != null) return attributes.Value ?? source.ToString();
			else return source.ToString();
		}
	}
#endif
}
