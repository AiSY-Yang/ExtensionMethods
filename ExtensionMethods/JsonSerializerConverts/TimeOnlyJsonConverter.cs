using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ExtensionMethods.JsonSerializerConverts
{
#if NET6_0_OR_GREATER
	/// <summary>
	/// 日期转换器
	/// </summary>
	public class TimeOnlyJsonConverter : JsonConverter<TimeOnly>
	{
		/// <inheritdoc/>
		public override TimeOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			var value = reader.GetString();
			if (value == null) return default;
			return TimeOnly.Parse(value);
		}
		/// <inheritdoc/>
		public override void Write(Utf8JsonWriter writer, TimeOnly value, JsonSerializerOptions options)
		{
			string value2 = value.ToString("HH:mm:ss");
			writer.WriteStringValue(value2);
		}
	}
#endif
}