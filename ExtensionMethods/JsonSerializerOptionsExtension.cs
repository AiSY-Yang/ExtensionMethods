using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

using ExtensionMethods.JsonSerializerConverts;

namespace ExtensionMethods
{
	/// <summary>
	/// Json序列化选项扩展
	/// </summary>
	public static class JsonSerializerOptionsExtension
	{
		/// <summary>
		/// 添加可空Guid转换器 如果传入空字符串则转换为null
		/// </summary>
		/// <param name="options"></param>
		public static JsonSerializerOptions AddNullableGuidJsonConverter(this JsonSerializerOptions options)
		{
			options.Converters.Add(new NullableGuidJsonConverter());
			return options;
		}
		/// <summary>
		/// 添加Type类型的转换器
		/// </summary>
		/// <param name="options"></param>
		public static JsonSerializerOptions AddTypeConverter(this JsonSerializerOptions options)
		{
			options.Converters.Add(new JsonConverterForType());
			return options;
		}
#if NET6_0_OR_GREATER
		/// <summary>
		/// 添加日期转换器 如果为null则转换为default
		/// </summary>
		/// <param name="options"></param>
		public static JsonSerializerOptions AddDateOnlyJsonConverter(this JsonSerializerOptions options)
		{
			options.Converters.Add(new DateOnlyJsonConverter());
			options.Converters.Add(new NullableDateOnlyJsonConverter());
			return options;
		}
		/// <summary>
		/// 添加时间转换器 如果为null则转换为default
		/// </summary>
		/// <param name="options"></param>
		public static JsonSerializerOptions AddTimeOnlyJsonConverter(this JsonSerializerOptions options)
		{
			options.Converters.Add(new TimeOnlyJsonConverter());
			options.Converters.Add(new NullableTimeOnlyJsonConverter());
			return options;
		}
#endif
	}
}