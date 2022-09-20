using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ExtensionMethods.JsonSerializerConverts
{
	/// <summary>
	/// 枚举转换器 将枚举转换为Description所定义的内容
	/// </summary>
	public class EnumDescriptionJsonConverter : JsonConverter<ValueType>
	{
		/// <inheritdoc/>
		public override bool CanConvert(Type typeToConvert)
		{
			return typeToConvert.IsEnum || typeToConvert.IsGenericType && typeToConvert.GenericTypeArguments.Length == 1 && typeToConvert.GenericTypeArguments[0].IsEnum;
		}
		/// <inheritdoc/>
		public override ValueType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			throw new NotImplementedException();
		}
		/// <inheritdoc/>
		public override void Write(Utf8JsonWriter writer, ValueType value, JsonSerializerOptions options)
		{
			if (value == null)
				writer.WriteNullValue();
			else
				writer.WriteStringValue(GetDescription((value as Enum)!));
		}
		/// <summary>
		/// 通过反射获取枚举的描述 如果不存在描述则返回字符串
		/// </summary>
		/// <param name="source"></param>
		/// <returns></returns>
		static string GetDescription(Enum source)
		{
			System.Reflection.FieldInfo fi = source.GetType().GetField(source.ToString());
			System.ComponentModel.DescriptionAttribute[] attributes = (System.ComponentModel.DescriptionAttribute[])fi.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);

			if (attributes != null && attributes.Length > 0) return attributes[0].Description;
			else return source.ToString();
		}
	}
}
