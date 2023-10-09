using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ExtensionMethods.JsonSerializerConverts
{
	/// <summary>
	/// 枚举转换器 读取字符串形式的数字转换为枚举
	/// </summary>
	public class NanToZeroJsonConverter : JsonConverter<double>
	{
		/// <inheritdoc/>
		public override double Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			if (reader.TokenType == JsonTokenType.String && reader.GetString() == "NaN")
			{
				return double.NaN;
			}

			return reader.GetDouble(); // JsonException thrown if reader.TokenType != JsonTokenType.Number
		}

		/// <inheritdoc/>
		public override void Write(Utf8JsonWriter writer, double value, JsonSerializerOptions options)
		{
			if (double.IsNaN(value))
			{
				writer.WriteNumberValue(0);
			}
			else
			{
				writer.WriteNumberValue(value);
			}
		}
	}
}
