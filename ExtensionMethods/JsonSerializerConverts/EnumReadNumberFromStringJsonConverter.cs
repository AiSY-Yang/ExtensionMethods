using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ExtensionMethods.JsonSerializerConverts
{
	/// <summary>
	/// 枚举转换器 读取字符串形式的数字转换为枚举
	/// </summary>
	public class EnumReadNumberFromStringJsonConverter : JsonConverter<ValueType>
	{
		/// <inheritdoc/>
		public override bool CanConvert(Type typeToConvert)
		{
			return typeToConvert.IsEnum || (typeToConvert.IsGenericType && typeToConvert.GenericTypeArguments.Length == 1 && typeToConvert.GenericTypeArguments[0].IsEnum);
		}
		/// <inheritdoc/>
		public override ValueType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			var s = reader.GetString();
			if (typeToConvert.IsEnum)
			{
				if (s == null) throw new JsonException("The value is null but typeToConvert is not null in position" + reader.Position);
				return int.Parse(s);
			}
			else
			{
				if (s == null)
					return null!;
				else
					return (ValueType)(Enum.Parse(typeToConvert.GenericTypeArguments[0], s));
			}
		}
		/// <inheritdoc/>
		public override void Write(Utf8JsonWriter writer, ValueType value, JsonSerializerOptions options)
		{
			if (value == null)
				writer.WriteNullValue();
			else
				writer.WriteNumberValue((int)value);
		}
	}
}
