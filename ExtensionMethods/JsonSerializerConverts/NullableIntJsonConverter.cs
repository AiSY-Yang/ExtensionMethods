using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ExtensionMethods.JsonSerializerConverts
{
#if NET5_0_OR_GREATER
	/// <summary>
	/// 可空Guid转换器 当为空字符串的时候转换为null
	/// </summary>
	public class NullableIntJsonConverter : JsonConverter<int?>
	{
		/// <inheritdoc/>
		public override int? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			return reader.TryGetInt32(out int value) ? value : null;
		}
		/// <inheritdoc/>
		public override void Write(Utf8JsonWriter writer, int? value, JsonSerializerOptions options)
		{
			if (value == null)
				writer.WriteNullValue();
			else
				writer.WriteNumberValue(value.Value);
		}
	}
#endif
}
