using System;
using System.Data;
using System.Text.Json;

namespace ExtensionMethods
{
	/// <summary>
	/// Object扩展
	/// </summary>
	public static class ObjectExtension
	{
		static ObjectExtension()
		{
			JsonConverterForType jsonConverterForType = new JsonConverterForType();
			//标准序列化
			JsonSerializerStandardOptions = new System.Text.Json.JsonSerializerOptions
			{
				//WriteIndented = true,
#if NET5_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
				IncludeFields = true,
#endif
				Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
			};
			JsonSerializerStandardOptions.Converters.Add(jsonConverterForType);

			//带时间格式的序列化
			JsonSerializerStandardOptionsWithCommonTimeFormat = new System.Text.Json.JsonSerializerOptions
			{
				//WriteIndented = true,

#if NET5_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
				IncludeFields = true,
#endif
				Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All),
			};
			JsonSerializerStandardOptionsWithCommonTimeFormat.Converters.Add(jsonConverterForType);
			JsonSerializerStandardOptionsWithCommonTimeFormat.Converters.Add(new JsonConverterDateTimeStandard());
		}
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
#if NETCOREAPP3_0_OR_GREATER || NET5_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
		#region JsonConverter
		/// <summary>
		/// Json日期转换器,格式为<code>yyyy-MM-dd HH:mm:ss</code>
		/// </summary>
		private class JsonConverterDateTimeStandard : System.Text.Json.Serialization.JsonConverter<DateTime>
		{
			/// <inheritdoc cref="System.Text.Json.Serialization.JsonConverter{T}.Read(ref Utf8JsonReader, Type, JsonSerializerOptions)"/>
			public override DateTime Read(ref System.Text.Json.Utf8JsonReader reader, Type typeToConvert, System.Text.Json.JsonSerializerOptions options) =>
					DateTime.ParseExact(reader.GetString(), "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
			/// <inheritdoc cref="System.Text.Json.Serialization.JsonConverter{T}.Write(Utf8JsonWriter, T, JsonSerializerOptions)"/>
			public override void Write(System.Text.Json.Utf8JsonWriter writer, DateTime dateTimeValue, System.Text.Json.JsonSerializerOptions options) =>
					writer.WriteStringValue(dateTimeValue.ToString("yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture));
		}
		/// <summary>
		/// 解决Type类型的序列化
		/// <a href="https://stackoverflow.com/questions/66919668/net-core-graphql-graphql-systemtextjson-serialization-and-deserialization-of/67001480">https://stackoverflow.com</a>
		/// </summary>
		private class JsonConverterForType : System.Text.Json.Serialization.JsonConverter<Type>
		{
			/// <inheritdoc cref="System.Text.Json.Serialization.JsonConverter{T}.Read(ref Utf8JsonReader, Type, JsonSerializerOptions)"/>
			public override Type Read(
				ref Utf8JsonReader reader,
				Type typeToConvert,
				JsonSerializerOptions options
				)
			{
				// Caution: Deserialization of type instances like this 
				// is not recommended and should be avoided
				// since it can lead to potential security issues.

				// If you really want this supported (for instance if the JSON input is trusted):
				// string assemblyQualifiedName = reader.GetString();
				// return Type.GetType(assemblyQualifiedName);
				throw new NotSupportedException();
			}

			/// <inheritdoc cref="System.Text.Json.Serialization.JsonConverter{T}.Write(Utf8JsonWriter, T, JsonSerializerOptions)"/>
			public override void Write(
				Utf8JsonWriter writer,
				Type value,
				JsonSerializerOptions options
				)
			{
				string assemblyQualifiedName = value?.AssemblyQualifiedName;
				// Use this with caution, since you are disclosing type information.
				writer.WriteStringValue(assemblyQualifiedName);
			}
		}
		#endregion

		/// <summary>
		/// 缓存的标准序列化选项,不编码任何内容,不带缩进的压缩格式
		/// </summary>
		private static System.Text.Json.JsonSerializerOptions JsonSerializerStandardOptions { get; set; }
		/// <summary>
		/// 采用System.Text.Json实现,效率更高,按照不进行编码的方式序列化对象
		/// </summary>
		/// <param name="_object"></param>
		/// <returns></returns>
		public static string ToJson(this object _object)
		{
			return System.Text.Json.JsonSerializer.Serialize(_object, JsonSerializerStandardOptions);
		}
		/// <summary>
		/// 缓存的标准序列化选项,在第一次调用的时候初始化,文字不进行编码,指定时间格式
		/// </summary>
		private static System.Text.Json.JsonSerializerOptions JsonSerializerStandardOptionsWithCommonTimeFormat { get; set; }
		/// <summary>
		/// 采用System.Text.Json实现,效率更高,默认将时间序列化为yyyy-MM-dd HH:mm:ss格式
		/// </summary>
		/// <param name="_object"></param>
		/// <returns></returns>
		public static string ToJsonWithCommonTimeFormat(this object _object)
		{
			return System.Text.Json.JsonSerializer.Serialize(_object, JsonSerializerStandardOptionsWithCommonTimeFormat);
		}
		/// <summary>
		/// 按照自定的序列化选项进行序列化
		/// </summary>
		/// <param name="_object"></param>
		/// <param name="jsonSerializerOptions"></param>
		/// <returns></returns>
		public static string ToJson(this object _object, JsonSerializerOptions jsonSerializerOptions)
		{
			return System.Text.Json.JsonSerializer.Serialize(_object, jsonSerializerOptions);
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
						// pattern-matching 不支持 Nullable 所有的nullable类型都会被null匹配 所以nullable对象在Model中的类型和长度必须与Datatable中的一模一样
						short => dataRow[item.Name] == DBNull.Value ? null : short.Parse(dataRow[item.Name].ToString()),
						int => dataRow[item.Name] == DBNull.Value ? null : int.Parse(dataRow[item.Name].ToString()),
						long => dataRow[item.Name] == DBNull.Value ? null : long.Parse(dataRow[item.Name].ToString()),
						decimal => dataRow[item.Name] == DBNull.Value ? null : decimal.Parse(dataRow[item.Name].ToString()),
						float => dataRow[item.Name] == DBNull.Value ? null : float.Parse(dataRow[item.Name].ToString()),
						double => dataRow[item.Name] == DBNull.Value ? null : double.Parse(dataRow[item.Name].ToString()),
						DateTime => dataRow[item.Name] == DBNull.Value ? null : DateTime.Parse(dataRow[item.Name].ToString()),
						//如果可以解析为数字 则数字不为0为true
						bool => dataRow[item.Name] == DBNull.Value ? null : (int.TryParse(dataRow[item.Name].ToString(), out int i) && i != 0) || (bool.TryParse(dataRow[item.Name].ToString(), out bool b) && b),
						//字符串会被null匹配而不是string
						null => dataRow[item.Name] == DBNull.Value ? null : dataRow[item.Name],
						string => dataRow[item.Name] == DBNull.Value ? null : dataRow[item.Name],
						//类成员会被object匹配,不进行更改
						object => item.GetValue(obj),
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
