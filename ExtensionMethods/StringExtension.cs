using System;
using System.Collections.Generic;
using System.ComponentModel;
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
		public static bool Contains(this string str, IEnumerable<string> shortStringArray) => shortStringArray.Any(item => str.Contains(item));

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
						continue;
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

		///<inheritdoc cref="string.IsNullOrEmpty(string)"/>
		public static bool IsNullOrEmpty(this string inputStr) => string.IsNullOrEmpty(inputStr);

		///<inheritdoc cref="string.IsNullOrWhiteSpace(string)"/>
		public static bool IsNullOrWhiteSpace(this string inputStr) => string.IsNullOrWhiteSpace(inputStr);

		///<inheritdoc cref="ByteExtension.CRC(byte[], CrcOption)"/>
		public static string CRC(this string _string, CrcOption crcOption) => Encoding.UTF8.GetBytes(_string).CRC(crcOption);

		///<inheritdoc cref="ByteExtension.Hash(byte[], HashOption, byte[])"/>
		public static string Hash(this string _string, HashOption hashOption) => Encoding.UTF8.GetBytes(_string).Hash(hashOption).ToHexString();

		///<inheritdoc cref="ByteExtension.Hash(byte[], HashOption, byte[])"/>
		public static string Hash(this string _string, HashOption hashOption, string secret) => Encoding.UTF8.GetBytes(_string).Hash(hashOption, secret.ToByteArray()).ToHexString();

		/// <summary>
		/// 字符串加密
		/// </summary>
		/// <param name="_string"></param>
		/// <param name="encryptOption"></param>
		/// <param name="secret">秘钥</param>
		/// <param name="iv">DES加密向量</param>
		/// <returns>base64编码的结果</returns>
		/// <exception cref="ArgumentException">Thrown when string is null or empty.</exception>
		[Obsolete("更改为独立的方法 DESEncrypt")]
		public static string Encrypt(this string _string, EncryptOption encryptOption, string? secret = null, string? iv = null)
		{
			if (string.IsNullOrEmpty(_string))
				throw new ArgumentException("String is null or empty");
			using var des = System.Security.Cryptography.DES.Create();
			switch (encryptOption)
			{
				case EncryptOption.DES_CBC_None:
					des.Mode = System.Security.Cryptography.CipherMode.CBC;
					des.Padding = System.Security.Cryptography.PaddingMode.None;
					break;
				case EncryptOption.DES_CBC_PKCS7:
					des.Mode = System.Security.Cryptography.CipherMode.CBC;
					des.Padding = System.Security.Cryptography.PaddingMode.PKCS7;
					break;
				case EncryptOption.DES_CBC_Zeros:
					des.Mode = System.Security.Cryptography.CipherMode.CBC;
					des.Padding = System.Security.Cryptography.PaddingMode.Zeros;
					break;
				case EncryptOption.DES_CBC_ANSIX923:
					des.Mode = System.Security.Cryptography.CipherMode.CBC;
					des.Padding = System.Security.Cryptography.PaddingMode.ANSIX923;
					break;
				default:
					throw new System.ComponentModel.InvalidEnumArgumentException(nameof(EncryptOption), (int)encryptOption, typeof(EncryptOption));
			}
			if (!string.IsNullOrEmpty(secret))
				des.Key = Encoding.UTF8.GetBytes(secret);
			if (!string.IsNullOrEmpty(iv))
				des.IV = Encoding.UTF8.GetBytes(iv);
			using System.Security.Cryptography.ICryptoTransform ct = des.CreateEncryptor();
			byte[] by = Encoding.UTF8.GetBytes(_string);
			using System.IO.MemoryStream outStream = new System.IO.MemoryStream();
			using var cs = new System.Security.Cryptography.CryptoStream(outStream, ct, System.Security.Cryptography.CryptoStreamMode.Write);
			cs.Write(by, 0, by.Length);
			cs.FlushFinalBlock();
			return System.Convert.ToBase64String(outStream.ToArray());
		}

		/// <summary>
		/// 解密字符串
		/// </summary>
		/// <param name="_string">字符串</param>
		/// <param name="decryptOption">解密方式</param>
		/// <param name="secret">密钥</param>
		/// <param name="iv">偏移量</param>
		/// <returns></returns>
		[Obsolete("更改为独立的方法 DESEncrypt")]
		public static string Decrypt(this string _string, EncryptOption decryptOption, string? secret = null, string? iv = null)
		{
			if (string.IsNullOrEmpty(_string))
				throw new ArgumentNullException(nameof(_string));
			using var des = System.Security.Cryptography.DES.Create();
			switch (decryptOption)
			{
				case EncryptOption.DES_CBC_None:
					des.Mode = System.Security.Cryptography.CipherMode.CBC;
					des.Padding = System.Security.Cryptography.PaddingMode.None;
					break;
				case EncryptOption.DES_CBC_PKCS7:
					des.Mode = System.Security.Cryptography.CipherMode.CBC;
					des.Padding = System.Security.Cryptography.PaddingMode.PKCS7;
					break;
				case EncryptOption.DES_CBC_Zeros:
					des.Mode = System.Security.Cryptography.CipherMode.CBC;
					des.Padding = System.Security.Cryptography.PaddingMode.Zeros;
					break;
				case EncryptOption.DES_CBC_ANSIX923:
					des.Mode = System.Security.Cryptography.CipherMode.CBC;
					des.Padding = System.Security.Cryptography.PaddingMode.ANSIX923;
					break;
				default:
					throw new System.ComponentModel.InvalidEnumArgumentException(nameof(EncryptOption), (int)decryptOption, typeof(EncryptOption));
			}
			if (!string.IsNullOrEmpty(secret))
				des.Key = Encoding.UTF8.GetBytes(secret);
			if (!string.IsNullOrEmpty(iv))
				des.IV = Encoding.UTF8.GetBytes(iv);
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
		/// <summary>
		/// DES加密
		/// </summary>
		/// <param name="data">待加密数据</param>
		/// <param name="secret">密钥</param>
		/// <param name="iv">偏移量</param>
		/// <param name="cipherMode">加密方式</param>
		/// <param name="paddingMode">填充模式</param>
		/// <returns>base64的加密结果</returns>
		public static string DESEncrypt(this string data, string secret, string iv, System.Security.Cryptography.CipherMode cipherMode, System.Security.Cryptography.PaddingMode paddingMode)
		{
			return System.Convert.ToBase64String(data.ToByteArray().DESEncrypt(secret.ToByteArray(), iv.ToByteArray(), cipherMode, paddingMode));
		}
		/// <summary>
		/// 将字符串当作base64编码进行DES解密
		/// </summary>
		/// <param name="data">待解密数据</param>
		/// <param name="secret">密钥</param>
		/// <param name="iv">偏移量</param>
		/// <param name="cipherMode">解密方式</param>
		/// <param name="paddingMode">填充模式</param>
		/// <returns>UTF8编码的字符串</returns>
		public static string DESDecrypt(this string data, string secret, string iv, System.Security.Cryptography.CipherMode cipherMode, System.Security.Cryptography.PaddingMode paddingMode)
		{
			return Encoding.UTF8.GetString(System.Convert.FromBase64String(data).DESEncrypt(secret.ToByteArray(), iv.ToByteArray(), cipherMode, paddingMode));
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
			try
			{
				return int.Parse(str);
			}
			catch (FormatException)
			{
				checked
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
				var x = double.Parse(str);
				if (double.IsInfinity(x))
					throw new OverflowException($"result overflow");
				else return x;
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
		/// 转换到日期时间类型,转换失败时抛出异常 默认时区为当前时区,支持以下格式
		/// <code>"yyyy-MM-dd HH:mm:ss"</code>
		/// <code>"yyyy-MM-dd HH:mm"</code>
		/// <code>"yyyy-MM-dd HH"</code>
		/// <code>"yyyy-MM-dd"</code>
		/// <code>"yyyy-MM"</code>
		/// <code>"yyyyMMddHHmmss"</code>
		/// <code>"yyyyMMddHHmm"</code>
		/// <code>"yyyyMMddHH"</code>
		/// <code>"yyyyMMdd"</code>
		/// <code>"yyyyMM"</code>
		/// </summary>
		/// <param name="str"></param>
		/// <exception cref="FormatException">不是有效的时间格式</exception>
		/// <returns></returns>
		public static DateTime ToDateTime(this string str)
		{
			//加快速度 先按照最常用方法解析
			if (DateTime.TryParse(str, out DateTime result))
				return result;
			else
			{
				if (DateTime.TryParseExact(str, "yyyy-MM-ddTHH:mm:ss", System.Globalization.CultureInfo.CurrentCulture, System.Globalization.DateTimeStyles.None, out DateTime JsonFormat)) return JsonFormat;
				if (DateTime.TryParseExact(str, "yyyy-MM-dd HH", System.Globalization.CultureInfo.CurrentCulture, System.Globalization.DateTimeStyles.None, out DateTime yyyyMMddHHWithSplit)) return yyyyMMddHHWithSplit;
				if (DateTime.TryParseExact(str, "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture, System.Globalization.DateTimeStyles.None, out DateTime yyyyMMddHHmmss)) return yyyyMMddHHmmss;
				if (DateTime.TryParseExact(str, "yyyyMMddHHmm", System.Globalization.CultureInfo.CurrentCulture, System.Globalization.DateTimeStyles.None, out DateTime yyyyMMddHHmm)) return yyyyMMddHHmm;
				if (DateTime.TryParseExact(str, "yyyyMMddHH", System.Globalization.CultureInfo.CurrentCulture, System.Globalization.DateTimeStyles.None, out DateTime yyyyMMddHH)) return yyyyMMddHH;
				if (DateTime.TryParseExact(str, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture, System.Globalization.DateTimeStyles.None, out DateTime yyyyMMdd)) return yyyyMMdd;
				if (DateTime.TryParseExact(str, "yyyyMM", System.Globalization.CultureInfo.CurrentCulture, System.Globalization.DateTimeStyles.None, out DateTime yyyyMM)) return yyyyMM;
				throw new FormatException($"string \"{str}\" cannot be converted to a date");
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
		/// 转换到时间类型,转换失败时抛出异常,默认时区为当前时区,支持以下格式
		/// <code>"yyyy-MM-dd HH:mm:ss.ffffff+00:00"</code>
		/// </summary>
		/// <param name="str"></param>
		/// <exception cref="FormatException">不是有效的时间格式</exception>
		/// <returns></returns>
		public static DateTimeOffset ToDateTimeOffset(this string str)
		{
			//加快速度 先按照最常用方法解析
			if (DateTimeOffset.TryParse(str, out DateTimeOffset result))
				return result;
			else
			{
				throw new FormatException($"string \"{str}\" cannot be converted to a date");
			}
		}
		/// <summary>
		/// 转换到时间类型,转换失败时返回defaultDateTime
		/// </summary>
		/// <param name="str"></param>
		/// <param name="defaultDateTimeOffset">转换失败的时候返回的时间</param>
		/// <returns></returns>
		public static DateTimeOffset ToDateTimeOffset(this string str, DateTimeOffset defaultDateTimeOffset)
		{
			try
			{
				return str.ToDateTime();
			}
			catch (Exception)
			{
				return defaultDateTimeOffset;
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
		/// <exception cref="InvalidCastException">转换失败</exception>
		public static T Convert<T>(this string str)
		{
			var converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(T));
			try
			{
				try
				{
					var res = converter.ConvertFromString(str);
					if (res == null)
					{
						throw new ArgumentNullException();
					}
					return (T)res;
				}
				catch (Exception)
				{
					return str.AsJsonToObject<T>();
				}
			}
			catch (Exception)
			{
				throw new InvalidCastException("转换失败");
			}
		}
		/// <summary>
		/// 使用UTF8编码转换为Base64字符串
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
		/// <summary>
		/// 转换到指定的命名方式
		/// </summary>
		/// <param name="identifier"></param>
		/// <param name="namingConvention"></param>
		/// <returns></returns>
		/// <exception cref="InvalidEnumArgumentException"></exception>
		public static string ToNamingConvention(this string identifier, NamingConvention namingConvention)
		{
			int wordCount = 0;
			return namingConvention switch
			{
				NamingConvention.flatcase => identifier.Replace("-", "").Replace("_", "").ToLower(),
				NamingConvention.UPPERCASE => identifier.Replace("-", "").Replace("_", "").ToUpper(),
				NamingConvention.camelCase => string.Concat(SplitWord(identifier).Select(x => { return wordCount++ == 0 ? x.ToLower() : string.Concat(char.ToUpper(x[0]), x[1..].ToLower()); })),
				NamingConvention.PascalCase => string.Concat(SplitWord(identifier).Select(x => string.Concat(char.ToUpper(x[0]), x[1..].ToLower()))),
				NamingConvention.snake_case => string.Join('_', SplitWord(identifier).Select(x => x.ToLower())),
				NamingConvention.camel_Snake_Case => string.Join('_', SplitWord(identifier).Select(x => { return wordCount++ == 0 ? x.ToLower() : string.Concat(char.ToUpper(x[0]), x[1..].ToLower()); })),
				NamingConvention.MACRO_CASE => string.Join('_', SplitWord(identifier).Select(x => x.ToUpper())),
				NamingConvention.Pascal_Snake_Case => string.Join('_', SplitWord(identifier).Select(x => string.Concat(char.ToUpper(x[0]), x[1..].ToLower()))),
				NamingConvention.kebab一case => string.Join('-', SplitWord(identifier).Select(x => x.ToLower())),
				NamingConvention.TRAIN一CASE => string.Join('-', SplitWord(identifier).Select(x => x.ToUpper())),
				NamingConvention.Train一Case => string.Join('-', SplitWord(identifier).Select(x => string.Concat(char.ToUpper(x[0]), x[1..].ToLower()))),
				_ => throw new InvalidEnumArgumentException(nameof(NamingConvention)),
			};
			//分割标识符单词
			List<string> SplitWord(string identifier)
			{
				bool lastIsUpper = false;
				var charList = new char[identifier.Length * 2];
				for (int i = 0, j = 0; i < identifier.Length; i++, j++)
				{
					if (char.IsUpper(identifier[i]))
					{
						if (!lastIsUpper)
						{
							charList[j] = '-';
							j++;
							lastIsUpper = true;
						}
					}
					else
					{
						lastIsUpper = false;
					}

					charList[j] = identifier[i];
				}
				var wordList = new List<string>();
				var l = string.Concat(charList).TrimEnd('\0')
					.Split(new char[] { '-', '_' }, StringSplitOptions.RemoveEmptyEntries);
				for (int i = 0; i < l.Length; i++)
				{
					wordList.Add(l[i]);
				}
				return wordList;
			}
		}
		#endregion


		#region Json
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
						throw new NotSupportedException(element.GetRawText());
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
						throw new NotSupportedException(element.GetRawText());
					default:
						throw new NotSupportedException(element.GetRawText());
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
						if (DateTime.TryParse(property.Value.GetString(), out DateTime _))
							ls.Add($"{new string('\t', level)}public DateTime {property.Name} {{get;set;}}\r\n");
						else
						{
							ls.Add($"{new string('\t', level)}public string {property.Name} {{get;set;}}\r\n");
						}
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
			return string.Concat(ls);
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
	/// 标识符命名约定
	/// <a href="https://en.wikipedia.org/wiki/Naming_convention_(programming)#Multiple-word_identifiers"></a>
	/// </summary>
	public enum NamingConvention
	{
		/// <summary>
		/// 小写
		/// </summary>
		flatcase = 1,
		/// <summary>
		/// 大写
		/// </summary>
		UPPERCASE,
		/// <summary>
		/// 小驼峰
		/// </summary>
		camelCase,
		/// <summary>
		/// 
		/// </summary>
		dromedaryCase = camelCase,
		/// <summary>
		/// 帕斯卡
		/// </summary>
		PascalCase,
		/// <summary>
		/// 大驼峰
		/// </summary>
		UpperCamelCase = PascalCase,
		/// <summary>
		/// 
		/// </summary>
		StudlyCase = PascalCase,
		/// <summary>
		/// 蛇形命名
		/// </summary>
		snake_case,
		/// <summary>
		/// 
		/// </summary>
		pothole_case = snake_case,
		/// <summary>
		/// 
		/// </summary>
		SCREAMING_SNAKE_CASE,
		/// <summary>
		/// 宏命名
		/// </summary>
		MACRO_CASE = SCREAMING_SNAKE_CASE,
		/// <summary>
		/// 
		/// </summary>
		CONSTANT_CASE = SCREAMING_SNAKE_CASE,
		/// <summary>
		/// 
		/// </summary>
		camel_Snake_Case,
		/// <summary>
		/// 
		/// </summary>
		Pascal_Snake_Case,
		/// <summary>
		/// 
		/// </summary>
		kebab一case,
		/// <summary>
		/// 
		/// </summary>
		dash一case = kebab一case,
		/// <summary>
		/// 
		/// </summary>
		lisp一case = kebab一case,
		/// <summary>
		/// 
		/// </summary>
		spinal一case = kebab一case,
		/// <summary>
		/// 
		/// </summary>
		TRAIN一CASE,
		/// <summary>
		/// 
		/// </summary>
		COBOL一CASE = TRAIN一CASE,
		/// <summary>
		/// 
		/// </summary>
		SCREAMING一KEBAB一CASE = TRAIN一CASE,
		/// <summary>
		/// 
		/// </summary>
		Train一Case,
		/// <summary>
		/// HTTP Header形式
		/// </summary>
		HTTP一Header一Case = Train一Case,
	}
}
