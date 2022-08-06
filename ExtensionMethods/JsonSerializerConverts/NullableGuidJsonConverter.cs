using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ExtensionMethods.JsonSerializerConverts
{
	/// <summary>
	/// 可空Guid转换器 当为空字符串的时候转换为null
	/// </summary>
	public class NullableGuidJsonConverter : JsonConverter<Guid?>
	{
		/// <inheritdoc cref="JsonConverter{T}.Read(ref Utf8JsonReader, Type, JsonSerializerOptions)"/>
		public override Guid? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			var value = reader.GetString();
			if (string.IsNullOrEmpty(value))
				return null;
			else
				return Guid.Parse(value);
		}
		/// <inheritdoc cref="JsonConverter{T}.Write(Utf8JsonWriter, T, JsonSerializerOptions)"/>
		public override void Write(Utf8JsonWriter writer, Guid? value, JsonSerializerOptions options)
		{
			if (value == null)
				writer.WriteNullValue();
			else
				writer.WriteStringValue(value.Value);
		}
	}
}
