using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ExtensionMethods.JsonSerializerConverts
{
#if NET6_0_OR_GREATER
	/// <summary>
	/// 日期转换器
	/// </summary>
	public class DateOnlyJsonConverter : JsonConverter<DateOnly>
	{
		/// <inheritdoc/>
		public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			var value = reader.GetString();
			if (value == null) return default;
			return DateOnly.Parse(value);
		}
		/// <inheritdoc/>
		public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
		{
			string value2 = value.ToString("yyyy-MM-dd");
			writer.WriteStringValue(value2);
		}
	}
#endif
}