using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace ExtensionMethods
{
	/// <summary>
	/// 字符串扩展函数
	/// </summary>
	public static class StringExtension
	{
		/// <summary>
		/// 移除开始指定字符串
		/// </summary>
		/// <param name="_string"></param>
		/// <param name="startstr">开始字符串</param>
		/// <returns></returns>
		public static string TrimStart(this string _string, string startstr) => _string.StartsWith(startstr) ? _string[startstr.Length..] : _string;

		/// <summary>
		/// 移除末尾指定字符串
		/// </summary>
		/// <param name="_string"></param>
		/// <param name="endstr">末尾字符串</param>
		/// <returns></returns>
		public static string TrimEnd(this string _string, string endstr) => _string.EndsWith(endstr) ? _string[..(_string.Length - endstr.Length)] : _string;

		/// <summary>
		/// 重复字符串
		/// </summary>
		/// <param name="_string"></param>
		/// <param name="repeatCount">重复次数</param>
		/// <returns></returns>
		public static string Repeat(this string _string, int repeatCount) => string.Concat(Enumerable.Repeat(_string, repeatCount));

		/// <summary>
		/// 是否包含其中任意一个元素
		/// </summary>
		/// <param name="str"></param>
		/// <param name="shortStringArray">字符串数组</param>
		/// <returns></returns>
		public static bool Contains(this string str, IEnumerable<string> shortStringArray)
		{
			foreach (var item in shortStringArray)
			{
				if (str.Contains(item))
				{
					return true;
				}
			}
			return false;
		}
		/// <summary>
		/// 删除字符串中的中文
		/// </summary>
		public static string RemoveChineseChar(this string str)
		{
			if (System.Text.RegularExpressions.Regex.IsMatch(str, @"[\u4e00-\u9fa5]"))
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (var item in str)
				{
					if (item >= 0x4e00 && item <= 0x9fa5)
					{
						continue;
					}
					else
					{
						stringBuilder.Append(item);
					}
				}
				return stringBuilder.ToString();
			}
			else
			{
				return str;
			}
		}

		/// <summary>
		/// 判断空
		/// </summary>
		/// <param name="inputStr"></param>
		/// <returns></returns>
		public static bool IsNullOrEmpty(this string inputStr) => string.IsNullOrEmpty(inputStr);

		/// <summary>
		/// 判断空
		/// </summary>
		/// <param name="inputStr"></param>
		/// <returns></returns>
		public static bool IsNullOrWhiteSpace(this string inputStr) => string.IsNullOrWhiteSpace(inputStr);

		///<inheritdoc cref="ByteExtension.CRC(byte[], CrcOption)"/>
		public static string CRC(this string _string, CrcOption crcOption) => Encoding.UTF8.GetBytes(_string).CRC(crcOption);

		///<inheritdoc cref="ByteExtension.Hash(byte[], HashOption, byte[])"/>
		public static string Hash(this string _string, HashOption hashOption, string secret = null) => Encoding.UTF8.GetBytes(_string).Hash(hashOption, secret?.ToByteArray()).ToHexString();

		/// <summary>
		/// 字符串加密
		/// </summary>
		/// <param name="_string"></param>
		/// <param name="encryptOption"></param>
		/// <param name="secret">秘钥</param>
		/// <param name="iv">DES加密向量</param>
		/// <returns></returns>
		/// <exception cref="ArgumentException">Thrown when string is null or empty.</exception>
		public static string Encrypt(this string _string, EncryptOption encryptOption, string secret = null, string iv = null)
		{
			if (string.IsNullOrEmpty(_string))
			{
				throw new ArgumentException("String is null or empty");
			}
			switch (encryptOption)
			{
				case EncryptOption.DES_CBC_None:
				case EncryptOption.DES_CBC_PKCS7:
				case EncryptOption.DES_CBC_Zeros:
				case EncryptOption.DES_CBC_ANSIX923:
					using (var des = System.Security.Cryptography.DES.Create())
					{
						des.Mode = encryptOption.ToString()[4..7] switch
						{
							"CBC" => System.Security.Cryptography.CipherMode.CBC,
							"ECB" => System.Security.Cryptography.CipherMode.ECB,
							"OFB" => System.Security.Cryptography.CipherMode.OFB,
							"CFB" => System.Security.Cryptography.CipherMode.CFB,
							"CTS" => System.Security.Cryptography.CipherMode.CTS,
							_ => throw new ArgumentException("加密模式枚举值不存在"),
						};
						des.Padding = encryptOption.ToString()[8..] switch
						{
							"None" => System.Security.Cryptography.PaddingMode.None,
							"PKCS7" => System.Security.Cryptography.PaddingMode.PKCS7,
							"Zeros" => System.Security.Cryptography.PaddingMode.Zeros,
							"ANSIX923" => System.Security.Cryptography.PaddingMode.ANSIX923,
							"ISO10126" => System.Security.Cryptography.PaddingMode.ISO10126,
							_ => throw new ArgumentException("填充模式枚举值不存在"),
						};
						if (!string.IsNullOrEmpty(secret))
						{
							des.Key = Encoding.UTF8.GetBytes(secret);
						}
						if (!string.IsNullOrEmpty(iv))
						{
							des.IV = Encoding.UTF8.GetBytes(iv);
						}
						using System.Security.Cryptography.ICryptoTransform ct = des.CreateEncryptor();
						byte[] by = Encoding.UTF8.GetBytes(_string);
						using System.IO.MemoryStream outStream = new System.IO.MemoryStream();
						using var cs = new System.Security.Cryptography.CryptoStream(outStream, ct, System.Security.Cryptography.CryptoStreamMode.Write);
						cs.Write(by, 0, by.Length);
						cs.FlushFinalBlock();
						return System.Convert.ToBase64String(outStream.ToArray());
					}
				default:
					throw new ArgumentException("枚举值不存在");
			}
		}

		/// <summary>
		/// 解密字符串
		/// </summary>
		/// <param name="_string">字符串</param>
		/// <param name="decryptOption">解密方式</param>
		/// <param name="secret">密钥</param>
		/// <param name="iv">偏移量</param>
		/// <returns></returns>
		public static string Decrypt(this string _string, EncryptOption decryptOption, string secret = null, string iv = null)
		{
			if (string.IsNullOrEmpty(_string))
			{
				throw new ArgumentException("String is null or empty");
			}
			switch (decryptOption)
			{
				case EncryptOption.DES_CBC_None:
				case EncryptOption.DES_CBC_PKCS7:
				case EncryptOption.DES_CBC_Zeros:
				case EncryptOption.DES_CBC_ANSIX923:
					using (var des = System.Security.Cryptography.DES.Create())
					{
						des.Mode = decryptOption.ToString()[4..7] switch
						{
							"CBC" => System.Security.Cryptography.CipherMode.CBC,
							"ECB" => System.Security.Cryptography.CipherMode.ECB,
							"OFB" => System.Security.Cryptography.CipherMode.OFB,
							"CFB" => System.Security.Cryptography.CipherMode.CFB,
							"CTS" => System.Security.Cryptography.CipherMode.CTS,
							_ => throw new ArgumentException("加密模式枚举值不存在"),
						};
						des.Padding = decryptOption.ToString()[8..] switch
						{
							"None" => System.Security.Cryptography.PaddingMode.None,
							"PKCS7" => System.Security.Cryptography.PaddingMode.PKCS7,
							"Zeros" => System.Security.Cryptography.PaddingMode.Zeros,
							"ANSIX923" => System.Security.Cryptography.PaddingMode.ANSIX923,
							"ISO10126" => System.Security.Cryptography.PaddingMode.ISO10126,
							_ => throw new ArgumentException("填充模式枚举值不存在"),
						};
						if (!string.IsNullOrEmpty(secret))
						{
							des.Key = Encoding.UTF8.GetBytes(secret);
						}
						if (!string.IsNullOrEmpty(iv))
						{
							des.IV = Encoding.UTF8.GetBytes(iv);
						}
						using System.Security.Cryptography.ICryptoTransform ct = des.CreateDecryptor();
						byte[] by = System.Convert.FromBase64String(_string);
						using System.IO.MemoryStream outStream = new System.IO.MemoryStream();
						using (var cs = new System.Security.Cryptography.CryptoStream(outStream, ct, System.Security.Cryptography.CryptoStreamMode.Write))
						{
							cs.Write(by, 0, by.Length);
							cs.FlushFinalBlock();
						}
						return Encoding.UTF8.GetString(outStream.ToArray());
					}
				default:
					throw new ArgumentException("枚举值不存在");
			}
		}
		#region Convert
		/// <summary>
		/// 转换为整型,如果为小数则截断小数部分
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		/// <exception cref="FormatException"></exception>
		/// <exception cref="OverflowException"></exception>
		public static int ToInt(this string str)
		{
			checked
			{
				try
				{
					return int.Parse(str);
				}
				catch (FormatException)
				{

					return (int)double.Parse(str);
				}
			}
		}
		/// <summary>
		/// 转换为整型,如果为小数则截断小数部分,转换失败(NAN or overflow)则返回指定数字
		/// </summary>
		/// <param name="str"></param>
		/// <param name="defaultResult">转换失败后返回的结果</param>
		/// <returns></returns>
		public static int ToInt(this string str, int defaultResult)
		{
			try
			{
				return str.ToInt();
			}
			catch (Exception)
			{
				return defaultResult;
			}
		}
		/// <summary>
		/// 转换为double
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		/// <exception cref="FormatException"></exception>
		/// <exception cref="OverflowException"></exception>
		public static double ToDouble(this string str)
		{
			checked
			{
				try
				{
					var x = double.Parse(str);
					if (double.IsInfinity(x))
						throw new OverflowException($"result overflow");
					else return x;
				}
				catch (FormatException ex)
				{
					throw new FormatException($"{str} is not a number", ex);
				}
			}
		}
		/// <summary>
		/// 转换为double,转换失败(NAN or overflow)则返回指定数字
		/// </summary>
		/// <param name="str"></param>
		/// <param name="defaultResult">转换失败后返回的结果</param>
		/// <returns></returns>
		public static double ToDouble(this string str, double defaultResult)
		{
			try
			{
				return str.ToDouble();
			}
			catch (Exception)
			{
				return defaultResult;
			}
		}
		/// <summary>
		/// 转换到日期时间类型,转换失败时抛出异常
		/// <code>"yyyy-MM-dd HH:mm:ss"</code>
		/// <code>"yyyy-MM-dd HH:mm"</code>
		/// <code>"yyyy-MM-dd HH"</code>
		/// <code>"yyyyMMddHHmmss"</code>
		/// <code>"yyyyMMddHHmm"</code>
		/// <code>"yyyyMMddHH"</code>
		/// <code>"yyyyMMdd"</code>
		/// <code>"yyyyMM"</code>
		/// <code>"yyyy"</code>
		/// </summary>
		/// <param name="str"></param>
		/// <exception cref="FormatException">不是有效的时间格式</exception>
		/// <exception cref="ArgumentNullException">字符串为空</exception>
		/// <returns></returns>
		public static DateTime ToDateTime(this string str)
		{
			//加快速度 先按照最常用方法解析
			try
			{
				return DateTime.Parse(str);
			}
			catch (Exception)
			{
				if (RegexCache.yyyyMMddHHWithSplit.IsMatch(str)) return DateTime.Parse(str + ":00");
				if (RegexCache.yyyyMMddHHmmss.IsMatch(str)) return DateTime.Parse($"{str[0..4]}-{str[4..6]}-{str[6..8]} {str[8..10]}:{str[10..12]}:{str[12..14]}");
				if (RegexCache.yyyyMMddHHmm.IsMatch(str)) return DateTime.Parse($"{str[0..4]}-{str[4..6]}-{str[6..8]} {str[8..10]}:{str[10..12]}:00");
				if (RegexCache.yyyyMMddHH.IsMatch(str)) return DateTime.Parse($"{str[0..4]}-{str[4..6]}-{str[6..8]} {str[8..10]}:00:00");
				if (RegexCache.yyyyMMdd.IsMatch(str)) return DateTime.Parse($"{str[0..4]}-{str[4..6]}-{str[6..8]} 00:00:00");
				if (RegexCache.yyyyMM.IsMatch(str)) return DateTime.Parse($"{str[0..4]}-{str[4..6]}-01 00:00:00");
				if (RegexCache.yyyy.IsMatch(str)) return DateTime.Parse($"{str[0..4]}-01-01 00:00:00");
				throw;
			}
		}
		/// <summary>
		/// 转换到日期时间类型,转换失败时返回defaultDateTime,支持以下格式
		/// <code>"yyyy-MM-dd HH:mm:ss"</code>
		/// <code>"yyyy-MM-dd HH:mm"</code>
		/// <code>"yyyy-MM-dd HH"</code>
		/// <code>"yyyyMMddHHmmss"</code>
		/// <code>"yyyyMMddHHmm"</code>
		/// <code>"yyyyMMddHH"</code>
		/// <code>"yyyyMMdd"</code>
		/// <code>"yyyyMM"</code>
		/// <code>"yyyy"</code>
		/// </summary>
		/// <param name="str"></param>
		/// <param name="defaultDateTime">转换失败的时候返回的时间</param>
		/// <returns></returns>
		public static DateTime ToDateTime(this string str, DateTime defaultDateTime)
		{
			try
			{
				return str.ToDateTime();
			}
			catch (Exception)
			{
				return defaultDateTime;
			}
		}
		/// <summary>
		/// 转换到UTF8字节数组
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static byte[] ToByteArray(this string str) => Encoding.UTF8.GetBytes(str);

		/// <summary>
		/// 转换格式
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="str"></param>
		/// <returns></returns>
		public static T Convert<T>(this string str)
		{
			var converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(T));
			try
			{
				try
				{
					return (T)converter.ConvertFromString(str);
				}
				catch (Exception)
				{
					return str.AsJsonToObject<T>();
				}
			}
			catch (Exception)
			{
				throw new Exception("转换失败");
			}
		}
		/// <summary>
		/// 从UTF8转换为Base64字符串
		/// </summary>
		/// <param name="_string"></param>
		/// <returns></returns>
		public static string ToBsae64String(this string _string) => System.Convert.ToBase64String(Encoding.UTF8.GetBytes(_string));
		/// <summary>
		/// 将字符串当作base64转换为Stream
		/// </summary>
		/// <param name="base64"></param>
		/// <returns>转换成功标志</returns>
		public static System.IO.Stream AsBase64ToStream(this string base64)
		{
			byte[] bytes = System.Convert.FromBase64String(base64);
			System.IO.MemoryStream ms = new System.IO.MemoryStream();
			ms.Write(bytes, 0, bytes.Length);
			return ms;
		}
		#endregion


		#region Json
		/// <summary>
		/// Json转换为dynamic
		/// </summary>
		/// <param name="json"></param>
		/// <returns></returns>
		public static dynamic UseNewtonsoftAsJsonTodynamic(this string json)
		{
			return Newtonsoft.Json.JsonConvert.DeserializeObject(json);
		}
		/// <summary>
		/// 把字符串当作json转换到指定类型
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="json"></param>
		/// <returns></returns>
		[Obsolete("System.Text.Json在简单序列化时性能可以提高10倍,推荐使用System.Text.Json")]
		public static T UseNewtonsoftAsJsonToObject<T>(this string json)
		{
			return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
		}
		/// <summary>
		/// 反序列化选项
		/// </summary>
		static readonly System.Text.Json.JsonSerializerOptions JsonDeserializeOptions = new System.Text.Json.JsonSerializerOptions()
		{
			//允许并跳过注释
			ReadCommentHandling = System.Text.Json.JsonCommentHandling.Skip,
#if NET5_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
			//允许带引号的数字
			NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString,
#endif
			//允许尾随逗号
			AllowTrailingCommas = true,
		};
		/// <summary>
		/// 把字符串当作json转换到指定类型
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="json"></param>
		/// <returns></returns>
		public static T AsJsonToObject<T>(this string json) => System.Text.Json.JsonSerializer.Deserialize<T>(json, JsonDeserializeOptions);
		/// <summary>
		/// 把字符串当作json转换到指定类型
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="json"></param>
		/// <param name="jsonSerializerOptions">反序列化选项</param>
		/// <returns></returns>
		public static T AsJsonToObject<T>(this string json, System.Text.Json.JsonSerializerOptions jsonSerializerOptions) => System.Text.Json.JsonSerializer.Deserialize<T>(json, jsonSerializerOptions);

		/// <summary>
		/// 把字符串当作json转换为类定义字符串,保留大小写,对象为嵌套类且属性已进行初始化
		/// </summary>
		/// <param name="_json"></param>
		/// <returns></returns>
		public static string AsJsonToClassDefine(this string _json)
		{
			List<string> ls = new List<string>();
			System.Text.Json.JsonElement element = _json.AsJsonToObject<System.Text.Json.JsonElement>();
			ls.Add("public class ROOT {\r\n");
			RecursionJsonElement(element);
			ls.Add("}");
			void RecursionJsonElement(System.Text.Json.JsonElement element, int level = 0)
			{
				switch (element.ValueKind)
				{
					case System.Text.Json.JsonValueKind.Undefined:
						throw new ArgumentException("Impossible");
					case System.Text.Json.JsonValueKind.Object:
						foreach (var item in element.EnumerateObject())
							RecursionJsonProperty(item, level + 1);
						break;
					case System.Text.Json.JsonValueKind.Array:
						RecursionJsonElement(element.EnumerateArray().First(), level);
						break;
					case System.Text.Json.JsonValueKind.String:
						ls.Remove(ls[^1]);
						ls[^1] = System.Text.RegularExpressions.Regex.Replace(ls[^1], "<.*?>", "<string>");
						ls.Add("^1");
						break;
					case System.Text.Json.JsonValueKind.Number:
						ls.Remove(ls[^1]);
						if (element.GetRawText().Contains('.'))
							ls[^1] = System.Text.RegularExpressions.Regex.Replace(ls[^1], "<.*?>", "<double>");
						else
							ls[^1] = System.Text.RegularExpressions.Regex.Replace(ls[^1], "<.*?>", "<int>");
						ls.Add("^1");
						break;
					case System.Text.Json.JsonValueKind.True:
						ls.Remove(ls[^1]);
						ls[^1] = System.Text.RegularExpressions.Regex.Replace(ls[^1], "<.*?>", "<bool>");
						ls.Add("^1");
						break;
					case System.Text.Json.JsonValueKind.False:
						ls.Remove(ls[^1]);
						ls[^1] = System.Text.RegularExpressions.Regex.Replace(ls[^1], "<.*?>", "<bool>");
						ls.Add("^1");
						break;
					case System.Text.Json.JsonValueKind.Null:
						throw new ArgumentException("Impossible");
					default:
						throw new ArgumentException("Impossible");
				}
			}
			void RecursionJsonProperty(System.Text.Json.JsonProperty property, int level)
			{
				switch (property.Value.ValueKind)
				{
					case System.Text.Json.JsonValueKind.Undefined:
						break;
					case System.Text.Json.JsonValueKind.Object:
						ls.Add($"{new string('\t', level)}public {property.Name.ToUpper()} {property.Name} {{get;set;}} = new {property.Name.ToUpper()}();\r\n");
						ls.Add($"{new string('\t', level)}public class {property.Name.ToUpper()} {{\r\n");
						foreach (var item in property.Value.EnumerateObject())
							RecursionJsonProperty(item, level + 1);
						ls.Add($"{new string('\t', level)}}}\r\n");
						break;
					case System.Text.Json.JsonValueKind.Array:
						ls.Add($"{new string('\t', level)}public List<{property.Name.ToUpper()}> {property.Name} {{get;set;}} = new List<{property.Name.ToUpper()}>();\r\n");
						ls.Add($"{new string('\t', level)}public class {property.Name.ToUpper()}{{\r\n");
						if (property.Value.EnumerateArray().Any())
							RecursionJsonElement(property.Value[0], level + 1);
						if (ls[^1] != "^1")
							ls.Add($"{new string('\t', level)}}}\r\n");
						else
							ls.Remove(ls[^1]);
						break;
					case System.Text.Json.JsonValueKind.String:
						ls.Add($"{new string('\t', level)}public string {property.Name} {{get;set;}}\r\n");
						break;
					case System.Text.Json.JsonValueKind.Number:
						if (property.Value.ToString().Contains('.'))
							ls.Add($"{new string('\t', level)}public double {property.Name} {{get;set;}}\r\n");
						else
							ls.Add($"{new string('\t', level)}public int {property.Name} {{get;set;}}\r\n");
						break;
					case System.Text.Json.JsonValueKind.True:
						ls.Add($"{new string('\t', level)}public bool {property.Name} {{get;set;}}\r\n");
						break;
					case System.Text.Json.JsonValueKind.False:
						ls.Add($"{new string('\t', level)}public bool {property.Name} {{get;set;}}\r\n");
						break;
					case System.Text.Json.JsonValueKind.Null:
						ls.Add($"{new string('\t', level)}public object {property.Name} {{get;set;}}\r\n");
						break;
				}
			};
			return string.Join("", ls);
		}
		#endregion
	}
	/// <summary>
	/// 加密方式
	/// </summary>
	public enum EncryptOption
	{
		/// <summary>
		/// DES加密 采用CBC方式 无填充 明文字节数必须为8的倍数
		/// </summary>
		DES_CBC_None,
		/// <summary>
		/// DES加密 采用CBC方式 PKCS7方式填充
		/// </summary>
		DES_CBC_PKCS7,
		/// <summary>
		/// DES加密 采用CBC方式 零填充 解密后不足8位的部分补<c>\0</c>
		/// </summary>
		DES_CBC_Zeros,
		/// <summary>
		/// DES加密 采用CBC方式 ANSIX923填充
		/// </summary>
		DES_CBC_ANSIX923,
	}
	/// <summary>
	/// 缓存所用到的正则表达式
	/// </summary>
	public static class RegexCache
	{

		/// <summary>
		/// 转换日期时间所用到的正则表达式
		/// <code>yyyy-M-d H:m</code>
		/// </summary>
		internal static readonly System.Text.RegularExpressions.Regex yyyyMMddHHWithSplit = new System.Text.RegularExpressions.Regex(@"^\d{4}-\d{1,2}-\d{1,2} \d{1,2}$");
		/// <summary>
		/// 转换日期时间所用到的正则表达式
		/// <code>yyyyMMddHHmmss</code>
		/// </summary>
		internal static readonly System.Text.RegularExpressions.Regex yyyyMMddHHmmss = new System.Text.RegularExpressions.Regex(@"^\d{14}$");
		/// <summary>
		/// 转换日期时间所用到的正则表达式
		/// <code>yyyyMMddHHmm</code>
		/// </summary>
		internal static readonly System.Text.RegularExpressions.Regex yyyyMMddHHmm = new System.Text.RegularExpressions.Regex(@"^\d{12}$");
		/// <summary>
		/// 转换日期时间所用到的正则表达式
		/// <code>yyyyMMddHH</code>
		/// 与秒级时间戳timestampSecond形式相同
		/// </summary>
		internal static readonly System.Text.RegularExpressions.Regex yyyyMMddHH = new System.Text.RegularExpressions.Regex(@"^\d{10}$");
		/// <summary>
		/// 转换日期时间所用到的正则表达式
		/// <code>yyyyMMdd</code>
		/// </summary>
		internal static readonly System.Text.RegularExpressions.Regex yyyyMMdd = new System.Text.RegularExpressions.Regex(@"^\d{8}$");
		/// <summary>
		/// 转换日期时间所用到的正则表达式
		/// <code>yyyyMM</code>
		/// </summary>
		internal static readonly System.Text.RegularExpressions.Regex yyyyMM = new System.Text.RegularExpressions.Regex(@"^\d{6}$");
		/// <summary>
		/// 转换日期时间所用到的正则表达式
		/// <code>yyyy</code>
		/// </summary>
		internal static readonly System.Text.RegularExpressions.Regex yyyy = new System.Text.RegularExpressions.Regex(@"^\d{4}$");

	}
}
