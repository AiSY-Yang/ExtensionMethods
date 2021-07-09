using System;
#if NETCOREAPP3_0_OR_GREATER || NET5_0_OR_GREATER
using System.Text.Json;
#endif

namespace ExtensionMethods
{
	/// <summary>
	/// Object扩展
	/// </summary>
	public static class ObjectExtension
	{
		/// <summary>
		/// 输出所有属性和字段
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static string ToPropertityAndFieldString(this object obj)
		{
			Type t = obj.GetType();
			System.Reflection.PropertyInfo[] PropertyList = t.GetProperties();
			string s = "";
			foreach (System.Reflection.PropertyInfo item in PropertyList)
			{
				s += "Propertity:" + item.Name + "=" + item.GetValue(obj) + Environment.NewLine;
			}
			System.Reflection.FieldInfo[] FieldInfoList = t.GetFields();
			foreach (System.Reflection.FieldInfo item in FieldInfoList)
			{
				s += "Field:" + item.Name + "=" + item.GetValue(obj) + Environment.NewLine;
			}
			return s;
		}
		#region Json

		/// <summary>
		/// 转换为json
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="serializerSettings">序列化设置</param>
		/// <returns></returns>
		[Obsolete("System.Text.Json在简单序列化时性能可以提高10倍,推荐使用System.Text.Json")]
		public static string UseNewtonsoftToJson(this object obj, Newtonsoft.Json.JsonSerializerSettings serializerSettings = null)
		{
			if (serializerSettings is null)
			{
				return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
			}
			else
			{
				return Newtonsoft.Json.JsonConvert.SerializeObject(obj, serializerSettings);
			}
		}
		/// <summary>
		/// 使用指定的日期格式将对象转换为json
		/// </summary>
		/// <param name="_object"></param>
		/// <param name="timeFormat">日期格式字符串</param>
		/// <returns></returns>
		[Obsolete("System.Text.Json在简单序列化时性能可以提高10倍,推荐使用System.Text.Json")]
		public static string UseNewtonsoftToJsonWithCommonTimeFormat(this object _object, string timeFormat = "yyyy-MM-dd HH:mm:ss")
		{
			return Newtonsoft.Json.JsonConvert.SerializeObject(_object, new Newtonsoft.Json.Converters.IsoDateTimeConverter() { DateTimeFormat = timeFormat });
		}
#if NETCOREAPP3_0_OR_GREATER || NET5_0_OR_GREATER
		/// <summary>
		/// Json日期转换器,格式为<code>yyyy-MM-dd HH:mm:ss</code>
		/// </summary>
		private class JsonConverterDateTimeStandard : System.Text.Json.Serialization.JsonConverter<DateTime>
		{
			///<inheritdoc/>
			public override DateTime Read(
				ref System.Text.Json.Utf8JsonReader reader,
				Type typeToConvert,
				System.Text.Json.JsonSerializerOptions options) =>
					DateTime.ParseExact(reader.GetString(),
						"yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
			///<inheritdoc/>
			public override void Write(
				System.Text.Json.Utf8JsonWriter writer,
				DateTime dateTimeValue,
				System.Text.Json.JsonSerializerOptions options) =>
					writer.WriteStringValue(dateTimeValue.ToString(
						"yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture));
		}
		/// <summary>
		/// 缓存的标准序列化选项,在第一次调用的时候初始化,文字不进行编码,
		/// </summary>
		private static System.Text.Json.JsonSerializerOptions JsonSerializerStandardOptions { get; set; }
		/// <summary>
		/// 缓存的标准序列化选项,在第一次调用的时候初始化,文字不进行编码,指定时间格式
		/// </summary>
		private static System.Text.Json.JsonSerializerOptions JsonSerializerStandardOptionsWithCommonTimeFormat { get; set; }
		/// <summary>
		/// 采用System.Text.Json实现,效率更高,按照不进行编码的方式序列化对象
		/// <code>!!!.net3.1程序无法序列化字段,.net5.0默认序列化字段</code>
		/// </summary>
		/// <param name="_object"></param>
		/// <returns></returns>
		public static string ToJson(this object _object)
		{
			if (JsonSerializerStandardOptions is null)
			{
				JsonSerializerStandardOptions = new System.Text.Json.JsonSerializerOptions
				{
					WriteIndented = true,
#if NET5_0
					IncludeFields = true,
#endif
					Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All),
				};
			}
			return System.Text.Json.JsonSerializer.Serialize(_object, JsonSerializerStandardOptions);
		}
		/// <summary>
		/// 按照自定的序列化选项进行序列化
		/// </summary>
		/// <param name="_object"></param>
		/// <param name="jsonSerializerOptions"></param>
		/// <returns></returns>
		public static string ToJson(this object _object, JsonSerializerOptions jsonSerializerOptions)
		{
			return System.Text.Json.JsonSerializer.Serialize(_object, JsonSerializerStandardOptions);
		}
		/// <summary>
		/// 采用System.Text.Json实现,效率更高,默认将时间序列化为yyyy-MM-dd HH:mm:ss格式
		/// <code>!!!.net3.1程序无法序列化字段,.net5.0默认序列化字段</code>
		/// </summary>
		/// <param name="_object"></param>
		/// <returns></returns>
		public static string ToJsonWithCommonTimeFormat(this object _object)
		{
			if (JsonSerializerStandardOptionsWithCommonTimeFormat is null)
			{
				JsonSerializerStandardOptionsWithCommonTimeFormat = new System.Text.Json.JsonSerializerOptions
				{
					WriteIndented = true,

#if NET5_0_OR_GREATER
					IncludeFields = true,
#endif
					Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All),
				};
				JsonSerializerStandardOptionsWithCommonTimeFormat.Converters.Add(new JsonConverterDateTimeStandard());
			}
			return System.Text.Json.JsonSerializer.Serialize(_object, JsonSerializerStandardOptionsWithCommonTimeFormat);
		}
#endif
		#endregion
#if NET5_0_OR_GREATER
		/// <summary>
		/// 使用DataRow内的同名数据填充对象的属性
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="dataRow"></param>
		public static T FillPropertiesWithDataRow<T>(this T obj, System.Data.DataRow dataRow)
		{
			System.Reflection.PropertyInfo[] ps = obj.GetType().GetProperties();
			foreach (var item in ps)
			{
				try
				{
					item.SetValue(obj, item.GetValue(obj) switch
					{
						string => DateTime.TryParse(dataRow[item.Name].ToString(), out DateTime date) ? date.ToString("yyyy-MM-dd HH:mm:ss") : dataRow[item.Name].ToString(),
						double => double.Parse(dataRow[item.Name].ToString()),
						float => float.Parse(dataRow[item.Name].ToString()),
						int => int.Parse(dataRow[item.Name].ToString()),
						long => long.Parse(dataRow[item.Name].ToString()),
						DateTime => DateTime.Parse(dataRow[item.Name].ToString()),
						//如果可以解析为数字 则数字不为0为true
						bool => (int.TryParse(dataRow[item.Name].ToString(), out int i) && i != 0) || (bool.TryParse(dataRow[item.Name].ToString(), out bool b) && b),
						_ => dataRow[item.Name],
					}
					);
				}
				catch (Exception ex)
				{
					throw new ArgumentException(item.Name + "转换失败", ex);
				}
			}
			return obj;
		}
#endif
	}
}
