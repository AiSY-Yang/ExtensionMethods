using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ExtensionMethods.JsonSerializerConverts
{
#if NET6_0_OR_GREATER
	/// <summary>
	/// 可空日期转换器 当为空字符串的时候转换为null
	/// </summary>
	public class NullableDateOnlyJsonConverter : JsonConverter<DateOnly?>
	{
		/// <inheritdoc/>
		public override DateOnly? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			var value = reader.GetString();
			if (string.IsNullOrEmpty(value))
				return null;
			else
				return DateOnly.Parse(value);
		}
		/// <inheritdoc/>
		public override void Write(Utf8JsonWriter writer, DateOnly? value, JsonSerializerOptions options)
		{
			string? value2 = value?.ToString("O");
			if (string.IsNullOrEmpty(value2))
				writer.WriteNullValue();
			else
				writer.WriteStringValue(value2);
		}
	}
#endif
}
