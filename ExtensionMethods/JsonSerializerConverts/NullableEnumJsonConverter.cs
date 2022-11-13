using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ExtensionMethods.JsonSerializerConverts
{
	/// <summary>
	/// 可空枚举转换器 当为空字符串的时候转换为null
	/// </summary>
	public class NullableEnumJsonConverter : JsonConverter<ValueType?>
	{
		/// <inheritdoc/>
		public override bool CanConvert(Type typeToConvert)
		{
			return typeToConvert.IsEnum || (typeToConvert.IsGenericType && typeToConvert.GenericTypeArguments.Length == 1 && typeToConvert.GenericTypeArguments[0].IsEnum);
		}
		/// <inheritdoc/>
		public override ValueType? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			switch (reader.TokenType)
			{
				case JsonTokenType.None:
					break;
				case JsonTokenType.StartObject:
					break;
				case JsonTokenType.EndObject:
					break;
				case JsonTokenType.StartArray:
					break;
				case JsonTokenType.EndArray:
					break;
				case JsonTokenType.PropertyName:
					break;
				case JsonTokenType.Comment:
					break;
				case JsonTokenType.String:
					if (reader.GetString() == "")
						return null;
					else
						throw new JsonException($"{reader.GetString()} cant't convert to {typeToConvert}");
				case JsonTokenType.Number:
					return Enum.ToObject(typeToConvert.GenericTypeArguments[0], reader.GetInt32()) as ValueType;
				case JsonTokenType.True:
					break;
				case JsonTokenType.False:
					break;
				case JsonTokenType.Null:
					break;
				default:
					break;
			}
			throw new JsonException($"Type {reader.TokenType} cant't convert to {typeToConvert}");
		}
		/// <inheritdoc/>
		public override void Write(Utf8JsonWriter writer, ValueType? value, JsonSerializerOptions options)
		{
			if (value == null)
				writer.WriteNullValue();
			else
				writer.WriteNumberValue((int)value);
		}
	}
}
