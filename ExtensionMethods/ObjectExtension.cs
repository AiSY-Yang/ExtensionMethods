using System;
using System.Diagnostics;
using System.Net.Sockets;

namespace ExtensionMethods
{
	/// <summary>
	/// Object扩展
	/// </summary>
	public static class ObjectExtension
	{
		static ObjectExtension()
		{
			//带时间格式的序列化
			JsonSerializerStandardOptionsWithCommonTimeFormat.Converters.Add(new JsonConverterDateTimeStandard());
		}
#if DEBUG
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
#endif
		#region Json
		#region JsonConverter
		/// <summary>
		/// Json日期转换器,格式为<code>yyyy-MM-dd HH:mm:ss</code>
		/// </summary>
		class JsonConverterDateTimeStandard : System.Text.Json.Serialization.JsonConverter<DateTime>
		{
			/// <inheritdoc cref="System.Text.Json.Serialization.JsonConverter{T}.Read(ref System.Text.Json.Utf8JsonReader, Type, System.Text.Json.JsonSerializerOptions)"/>
			public override DateTime Read(ref System.Text.Json.Utf8JsonReader reader, Type typeToConvert, System.Text.Json.JsonSerializerOptions options) =>
					DateTime.Parse(reader.GetString() ?? throw new ArgumentNullException($"null can't convert to DateTime in position {reader.Position}"));
			/// <inheritdoc cref="System.Text.Json.Serialization.JsonConverter{T}.Write(System.Text.Json.Utf8JsonWriter, T, System.Text.Json.JsonSerializerOptions)"/>
			public override void Write(System.Text.Json.Utf8JsonWriter writer, DateTime dateTimeValue, System.Text.Json.JsonSerializerOptions options) =>
					writer.WriteStringValue(dateTimeValue.ToString("yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture));
		}
		#endregion

		/// <summary>
		/// 缓存的标准序列化选项,不编码任何内容,不带缩进的压缩格式
		/// </summary>
		static readonly System.Text.Json.JsonSerializerOptions JsonSerializerStandardOptions = new System.Text.Json.JsonSerializerOptions
		{
			//WriteIndented = true,
#if NET5_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
			IncludeFields = true,
#endif
			Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
		}.AddTypeConverter();

#if NET5_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
		/// <summary>
		/// 按照不进行编码的方式序列化对象 包含字段
		/// </summary>
		/// <param name="_object"></param>
		/// <returns></returns>
#else
		/// <summary>
		/// 按照不进行编码的方式序列化对象 不包含字段
		/// </summary>
		/// <param name="_object"></param>
		/// <returns></returns>
#endif
		public static string ToJson(this object _object) => System.Text.Json.JsonSerializer.Serialize(_object, JsonSerializerStandardOptions);

		/// <summary>
		/// 缓存的标准序列化选项,在第一次调用的时候初始化,文字不进行编码,指定时间格式
		/// </summary>
		static readonly System.Text.Json.JsonSerializerOptions JsonSerializerStandardOptionsWithCommonTimeFormat = new System.Text.Json.JsonSerializerOptions
		{
			//WriteIndented = true,
#if NET5_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
			IncludeFields = true,
#endif
			Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
		}.AddTypeConverter();
		/// <summary>
		/// 将时间序列化为yyyy-MM-dd HH:mm:ss格式
		/// </summary>
		/// <param name="_object"></param>
		/// <returns></returns>
		public static string ToJsonWithCommonTimeFormat(this object _object) => System.Text.Json.JsonSerializer.Serialize(_object, JsonSerializerStandardOptionsWithCommonTimeFormat);

		/// <summary>
		/// 按照自定的序列化选项进行序列化
		/// </summary>
		/// <param name="_object"></param>
		/// <param name="jsonSerializerOptions"></param>
		/// <returns></returns>
		public static string ToJson(this object _object, System.Text.Json.JsonSerializerOptions jsonSerializerOptions) => System.Text.Json.JsonSerializer.Serialize(_object, jsonSerializerOptions);

		#endregion
#if NET5_0_OR_GREATER
		/// <summary>
		/// 使用DataRow内的同名数据填充对象的属性
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="dataRow"></param>
		public static T FillPropertiesWithDataRow<T>(this T obj, System.Data.DataRow dataRow)
		{
			if (obj is null)
			{
				throw new System.ArgumentNullException(nameof(obj));
			};
			System.Reflection.PropertyInfo[] ps = obj.GetType().GetProperties();
			foreach (var item in ps)
			{
				try
				{
					item.SetValue(obj, item.GetValue(obj) switch
					{
						// pattern-matching 不支持 Nullable 所有的nullable类型都会被null匹配 所以nullable对象在Model中的类型和长度必须与Datatable中的一模一样
						short => dataRow[item.Name] == DBNull.Value ? null : short.Parse(dataRow[item.Name].ToString() ?? "0"),
						int => dataRow[item.Name] == DBNull.Value ? null : int.Parse(dataRow[item.Name].ToString() ?? "0"),
						long => dataRow[item.Name] == DBNull.Value ? null : long.Parse(dataRow[item.Name].ToString() ?? "0"),
						decimal => dataRow[item.Name] == DBNull.Value ? null : decimal.Parse(dataRow[item.Name].ToString() ?? "0"),
						float => dataRow[item.Name] == DBNull.Value ? null : float.Parse(dataRow[item.Name].ToString() ?? "0"),
						double => dataRow[item.Name] == DBNull.Value ? null : double.Parse(dataRow[item.Name].ToString() ?? "0"),
						DateTime => dataRow[item.Name] == DBNull.Value ? null : DateTime.Parse(dataRow[item.Name].ToString() ?? "0"),
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
