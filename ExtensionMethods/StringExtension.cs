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
		public static string TrimStart(this string _string, string startstr)
		{
			if (_string.StartsWith(startstr))
			{
				return _string.Remove(0, startstr.Length);
			}
			else
			{
				return _string;
			}
		}

		/// <summary>
		/// 移除末尾指定字符串
		/// </summary>
		/// <param name="_string"></param>
		/// <param name="endstr">末尾字符串</param>
		/// <returns></returns>
		public static string TrimEnd(this string _string, string endstr)
		{
			if (endstr.IsNullOrEmpty())
			{
				return _string;
			}
			if (_string.EndsWith(endstr))
			{
				return _string.Remove(_string.Length - endstr.Length);
			}
			else
			{
				return _string;
			}
		}
		/// <summary>
		/// 重复字符串
		/// </summary>
		/// <param name="_string"></param>
		/// <param name="repeatCount">重复次数</param>
		/// <returns></returns>
		public static string Repeat(this string _string, int repeatCount)
		{
			return string.Concat(Enumerable.Repeat(_string, repeatCount));
		}
		/// <summary>
		/// 是否包含其中任意一个元素
		/// </summary>
		/// <param name="str"></param>
		/// <param name="ss">字符串数组</param>
		/// <returns></returns>
		public static bool Contains(this string str, IEnumerable<string> ss)
		{
			foreach (var item in ss)
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
				foreach (var item in str.ToCharArray())
				{
					if ((item >= 0x4e00 && item <= 0x9fa5))
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
		public static bool IsNullOrEmpty(this string inputStr)
		{
			return string.IsNullOrEmpty(inputStr);
		}
		/// <summary>
		/// 判断空
		/// </summary>
		/// <param name="inputStr"></param>
		/// <returns></returns>
		public static bool IsNullOrWhiteSpace(this string inputStr)
		{
			return string.IsNullOrWhiteSpace(inputStr);
		}
#if NET5_0 || NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_0_OR_GREATER
		/// <summary>
		/// 加密方式
		/// </summary>
		public enum EncryptOption
		{
			/// <summary>
			/// 转换为Base64编码
			/// </summary>
			Base64,
			/// <summary>
			/// 计算16位MD5
			/// </summary>
			MD5_16,
			/// <summary>
			/// 计算32位MD5
			/// </summary>
			MD5_32,
			/// <summary>
			/// 计算SHA1
			/// </summary>
			SHA1,
			/// <summary>
			/// 计算SHA256
			/// </summary>
			SHA256,
			/// <summary>
			/// 计算SHA384
			/// </summary>
			SHA384,
			/// <summary>
			/// 计算SHA512
			/// </summary>
			SHA512,
			/// <summary>
			/// HJT212协议CRC校验
			/// </summary>
			CRC16_HJT212,
			/// <summary>
			/// 计算HmacSHA1
			/// </summary>
			HmacSHA1,
			/// <summary>
			/// 计算HmacSHA256
			/// </summary>
			HmacSHA256,
			/// <summary>
			/// 计算HmacSHA384
			/// </summary>
			HmacSHA384,
			/// <summary>
			/// 计算HmacSHA512
			/// </summary>
			HmacSHA512,
			/// <summary>
			/// 计算HmacMD5
			/// </summary>
			HmacMD5,
			/// <summary>
			/// 计算HmacSHA1并base64编码
			/// </summary>
			HmacSHA1_Base64,
			/// <summary>
			/// 计算HmacSHA256并base64编码
			/// </summary>
			HmacSHA256_Base64,
			/// <summary>
			/// 计算HmacSHA384并base64编码
			/// </summary>
			HmacSHA384_Base64,
			/// <summary>
			/// 计算HmacSHA512并base64编码
			/// </summary>
			HmacSHA512_Base64,
			/// <summary>
			/// 计算HmacMD5并base64编码
			/// </summary>
			HmacMD5_Base64,
			/// <summary>
			/// DES加密 采用CBC方式 无填充
			/// </summary>
			DES_CBC_None,
			/// <summary>
			/// DES加密 采用CBC方式 PKCS7方式填充
			/// </summary>
			DES_CBC_PKCS7,
			/// <summary>
			/// DES加密 采用CBC方式 零填充
			/// </summary>
			DES_CBC_Zeros,
			/// <summary>
			/// DES加密 采用CBC方式 ANSIX923填充
			/// </summary>
			DES_CBC_ANSIX923,
			//DES_ECB_None,
			//DES_ECB_PKCS7,
			//DES_ECB_Zeros,
			//DES_ECB_ANSIX923,
			//DES_OFB_None,
			//DES_OFB_PKCS7,
			//DES_OFB_Zeros,
			//DES_OFB_ANSIX923,
			//DES_CFB_None,
			//DES_CFB_PKCS7,
			//DES_CFB_Zeros,
			//DES_CFB_ANSIX923,
			//DES_CTS_None,
			//DES_CTS_PKCS7,
			//DES_CTS_Zeros,
			//DES_CTS_ANSIX923,
		}
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
			if (_string.IsNullOrEmpty())
			{
				throw new ArgumentException("String is null or empty");
			}
			switch (encryptOption)
			{
				case EncryptOption.Base64:
					return System.Convert.ToBase64String(Encoding.UTF8.GetBytes(_string));
				case EncryptOption.CRC16_HJT212:
					byte[] data = _string.ToByteArray();
					ushort crc = 0xFFFF;
					int len = data.Length;
					for (int i = 0; i < len; i++)
					{
						crc = (ushort)((crc >> 8) ^ data[i]);
						for (int j = 0; j < 8; j++)
							crc = (crc & 1) == 1 ? (ushort)((crc >> 1) ^ 0xA001) : (ushort)(crc >> 1);
					}
					return string.Format("{0:X}", crc).PadLeft(4, '0');
				case EncryptOption.MD5_16:
					return _string.Encrypt(EncryptOption.MD5_32, secret)[8..24];
				case EncryptOption.MD5_32:
				case EncryptOption.SHA1:
				case EncryptOption.SHA256:
				case EncryptOption.SHA384:
				case EncryptOption.SHA512:
					byte[] hash = encryptOption switch
					{
						EncryptOption.MD5_32 => System.Security.Cryptography.MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(_string)),
						EncryptOption.SHA1 => System.Security.Cryptography.SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes(_string)),
						EncryptOption.SHA256 => System.Security.Cryptography.SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(_string)),
						EncryptOption.SHA384 => System.Security.Cryptography.SHA384.Create().ComputeHash(Encoding.UTF8.GetBytes(_string)),
						EncryptOption.SHA512 => System.Security.Cryptography.SHA512.Create().ComputeHash(Encoding.UTF8.GetBytes(_string)),
						_ => throw new ArgumentException(),
					};
					StringBuilder builder = new StringBuilder();
					foreach (var item in hash)
					{
						builder.Append(item.ToString("x2"));
					}
					return builder.ToString();
				case EncryptOption.HmacMD5:
				case EncryptOption.HmacSHA1:
				case EncryptOption.HmacSHA256:
				case EncryptOption.HmacSHA384:
				case EncryptOption.HmacSHA512:
				case EncryptOption.HmacMD5_Base64:
				case EncryptOption.HmacSHA1_Base64:
				case EncryptOption.HmacSHA256_Base64:
				case EncryptOption.HmacSHA384_Base64:
				case EncryptOption.HmacSHA512_Base64:
					if (secret.IsNullOrEmpty())
					{
						throw new ArgumentException("for Hmac algorithm,The secret is necessary");
					}
					System.Security.Cryptography.HMAC hMAC = encryptOption switch
					{
						EncryptOption.HmacMD5 => new System.Security.Cryptography.HMACMD5(Encoding.UTF8.GetBytes(secret)),
						EncryptOption.HmacSHA1 => new System.Security.Cryptography.HMACSHA1(Encoding.UTF8.GetBytes(secret)),
						EncryptOption.HmacSHA256 => new System.Security.Cryptography.HMACSHA256(Encoding.UTF8.GetBytes(secret)),
						EncryptOption.HmacSHA384 => new System.Security.Cryptography.HMACSHA384(Encoding.UTF8.GetBytes(secret)),
						EncryptOption.HmacSHA512 => new System.Security.Cryptography.HMACSHA512(Encoding.UTF8.GetBytes(secret)),
						EncryptOption.HmacMD5_Base64 => new System.Security.Cryptography.HMACMD5(Encoding.UTF8.GetBytes(secret)),
						EncryptOption.HmacSHA1_Base64 => new System.Security.Cryptography.HMACSHA1(Encoding.UTF8.GetBytes(secret)),
						EncryptOption.HmacSHA256_Base64 => new System.Security.Cryptography.HMACSHA256(Encoding.UTF8.GetBytes(secret)),
						EncryptOption.HmacSHA384_Base64 => new System.Security.Cryptography.HMACSHA384(Encoding.UTF8.GetBytes(secret)),
						EncryptOption.HmacSHA512_Base64 => new System.Security.Cryptography.HMACSHA512(Encoding.UTF8.GetBytes(secret)),
						_ => throw new ArgumentException(),
					};
					byte[] hashmessage = hMAC.ComputeHash(Encoding.UTF8.GetBytes(_string));
					switch (encryptOption)
					{
						case EncryptOption.HmacMD5_Base64:
						case EncryptOption.HmacSHA1_Base64:
						case EncryptOption.HmacSHA256_Base64:
						case EncryptOption.HmacSHA384_Base64:
						case EncryptOption.HmacSHA512_Base64:
							return System.Convert.ToBase64String(hashmessage);
					}
					StringBuilder stringBuilder = new StringBuilder();
					foreach (var item in hashmessage)
					{
						stringBuilder.Append(item.ToString("x2"));
					}
					return stringBuilder.ToString();
				case EncryptOption.DES_CBC_None:
				case EncryptOption.DES_CBC_PKCS7:
				case EncryptOption.DES_CBC_Zeros:
				case EncryptOption.DES_CBC_ANSIX923:
					//case EncryptOption.DES_ECB_None:
					//case EncryptOption.DES_ECB_PKCS7:
					//case EncryptOption.DES_ECB_Zeros:
					//case EncryptOption.DES_ECB_ANSIX923:
					//case EncryptOption.DES_OFB_None:
					//case EncryptOption.DES_OFB_PKCS7:
					//case EncryptOption.DES_OFB_Zeros:
					//case EncryptOption.DES_OFB_ANSIX923:
					//case EncryptOption.DES_CFB_None:
					//case EncryptOption.DES_CFB_PKCS7:
					//case EncryptOption.DES_CFB_Zeros:
					//case EncryptOption.DES_CFB_ANSIX923:
					//case EncryptOption.DES_CTS_None:
					//case EncryptOption.DES_CTS_PKCS7:
					//case EncryptOption.DES_CTS_Zeros:
					//case EncryptOption.DES_CTS_ANSIX923:
					System.Security.Cryptography.DESCryptoServiceProvider des = new System.Security.Cryptography.DESCryptoServiceProvider();
					switch (encryptOption.ToString()[4..7])
					{
						case "CBC": des.Mode = System.Security.Cryptography.CipherMode.CBC; break;
						case "ECB": des.Mode = System.Security.Cryptography.CipherMode.ECB; break;
						case "OFB": des.Mode = System.Security.Cryptography.CipherMode.OFB; break;
						case "CFB": des.Mode = System.Security.Cryptography.CipherMode.CFB; break;
						case "CTS": des.Mode = System.Security.Cryptography.CipherMode.CTS; break;
						default:
							throw new ArgumentException("加密模式枚举值不存在");
					}
					switch (encryptOption.ToString()[8..])
					{
						case "None": des.Padding = System.Security.Cryptography.PaddingMode.None; break;
						case "PKCS7": des.Padding = System.Security.Cryptography.PaddingMode.PKCS7; break;
						case "Zeros": des.Padding = System.Security.Cryptography.PaddingMode.Zeros; break;
						case "ANSIX923": des.Padding = System.Security.Cryptography.PaddingMode.ANSIX923; break;
						case "ISO10126": des.Padding = System.Security.Cryptography.PaddingMode.ISO10126; break;
						default:
							throw new ArgumentException("填充模式枚举值不存在");
					}
					if (!secret.IsNullOrEmpty())
					{
						des.Key = Encoding.UTF8.GetBytes(secret);
					}
					if (!iv.IsNullOrEmpty())
					{
						des.IV = Encoding.UTF8.GetBytes(iv);
					}
					using (System.Security.Cryptography.ICryptoTransform ct = des.CreateEncryptor())
					{
						byte[] by = Encoding.UTF8.GetBytes(_string);
						using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
						{
							using (var cs = new System.Security.Cryptography.CryptoStream(ms, ct,
															 System.Security.Cryptography.CryptoStreamMode.Write))
							{
								cs.Write(by, 0, by.Length);
								cs.FlushFinalBlock();
							}
							return System.Convert.ToBase64String(ms.ToArray());
						}
					}
				default:
					throw new ArgumentException("枚举值不存在");
			}
		}
		/// <summary>
		/// 解密方式
		/// </summary>
		public enum DncryptOption
		{

		}
		/// <summary>
		/// 解密字符串
		/// </summary>
		/// <param name="_string">字符串</param>
		/// <param name="dncryptOption">解密方式</param>
		/// <param name="secret">密钥</param>
		/// <param name="iv">偏移量</param>
		/// <returns></returns>
		public static string Decrypt(this string _string, DncryptOption dncryptOption, string secret = null,string iv=null)
		{
			if (_string.IsNullOrEmpty())
			{
				throw new ArgumentException("String is null or empty");
			}
			switch (dncryptOption)
			{
				default:
					throw new ArgumentException("枚举值不存在"); ;
			}
		}
#endif
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
					try
					{
						return (int)double.Parse(str);
					}
					catch (FormatException ex)
					{
						throw new FormatException($"{str} is not a number", ex);
					}
					catch (OverflowException ex)
					{
						throw new NotFiniteNumberException($"result overflow", ex);
					}
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
		/// 转换日期时间所用到的正则表达式
		/// </summary>
		static readonly System.Text.RegularExpressions.Regex yyyyMMddHHWithSplit = new System.Text.RegularExpressions.Regex(@"^\d{4}-\d{1,2}-\d{1,2} \d{1,2}$");
		static readonly System.Text.RegularExpressions.Regex yyyyMMddHHmmss = new System.Text.RegularExpressions.Regex(@"^\d{14}$");
		static readonly System.Text.RegularExpressions.Regex yyyyMMddHHmm = new System.Text.RegularExpressions.Regex(@"^\d{12}$");
		static readonly System.Text.RegularExpressions.Regex yyyyMMddHH = new System.Text.RegularExpressions.Regex(@"^\d{10}$");
		static readonly System.Text.RegularExpressions.Regex yyyyMMdd = new System.Text.RegularExpressions.Regex(@"^\d{8}$");
		static readonly System.Text.RegularExpressions.Regex yyyyMM = new System.Text.RegularExpressions.Regex(@"^\d{6}$");
		static readonly System.Text.RegularExpressions.Regex yyyy = new System.Text.RegularExpressions.Regex(@"^\d{4}$");
		static readonly System.Text.RegularExpressions.Regex timestampSecond = new System.Text.RegularExpressions.Regex(@"^\d{10}$");
		static readonly System.Text.RegularExpressions.Regex timestampMileSecond = new System.Text.RegularExpressions.Regex(@"^\d{13}$");

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
		/// <code>"时间戳/秒"</code>
		/// <code>"时间戳/毫秒"</code>
		/// </summary>
		/// <param name="str"></param>
		/// <exception cref="FormatException">不是有效的时间格式</exception>
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
				if (yyyyMMddHHWithSplit.IsMatch(str)) return DateTime.Parse(str + ":00");
				if (yyyyMMddHHmmss.IsMatch(str)) return DateTime.Parse($"{str[0..4]}-{str[4..6]}-{str[6..8]} {str[8..10]}:{str[10..12]}:{str[12..14]}");
				if (yyyyMMddHHmm.IsMatch(str)) return DateTime.Parse($"{str[0..4]}-{str[4..6]}-{str[6..8]} {str[8..10]}:{str[10..12]}:00");
				//10位数字先按照无分隔符日期判断,如果小于1970年则按照时间戳返回
				if (yyyyMMddHH.IsMatch(str))
				{
					try
					{
						DateTime dateTime;
						dateTime = DateTime.Parse($"{str[0..4]}-{str[4..6]}-{str[6..8]} {str[8..10]}:00:00");
						if (dateTime < new System.DateTime(1970, 1, 1))
						{
							return new System.DateTime(1970, 1, 1).ToLocalTime().AddSeconds(long.Parse(str));
						}
						return dateTime;
					}
					catch (FormatException)
					{
						return new System.DateTime(1970, 1, 1).ToLocalTime().AddSeconds(long.Parse(str));
					}
				}
				if (yyyyMMdd.IsMatch(str)) return DateTime.Parse($"{str[0..4]}-{str[4..6]}-{str[6..8]} 00:00:00");
				if (yyyyMM.IsMatch(str)) return DateTime.Parse($"{str[0..4]}-{str[4..6]}-01 00:00:00");
				if (yyyy.IsMatch(str)) return DateTime.Parse($"{str[0..4]}-01-01 00:00:00");
				if (timestampMileSecond.IsMatch(str)) return new DateTime(1970, 1, 1).ToLocalTime().AddMilliseconds(long.Parse(str));
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
		/// <code>"时间戳/秒"</code>
		/// <code>"时间戳/毫秒"</code>
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
		public static byte[] ToByteArray(this string str)
		{
			return Encoding.UTF8.GetBytes(str);
		}
		/// <summary>
		/// 转换格式
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="str"></param>
		/// <returns></returns>
		public static T Convert<T>(this string str)
		{
			var converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(T));
			if (converter == null)
			{
				return str.AsJsonToObject<T>();
			}
			else
			{
				try
				{
					return (T)converter.ConvertFromString(str);
				}
				catch (Exception)
				{

					throw new Exception("转换失败");
				}
			}
		}
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
#if NETCOREAPP3_0_OR_GREATER || NET5_0_OR_GREATER
		/// <summary>
		/// 把字符串当作json转换到指定类型
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="json"></param>
		/// <returns></returns>
		public static T AsJsonToObject<T>(this string json)
		{
			return System.Text.Json.JsonSerializer.Deserialize<T>(json);
		}
		/// <summary>
		/// 把字符串当作json转换为类定义字符串,保留大小写,对象为嵌套类且属性已进行初始化
		/// </summary>
		/// <param name="_json"></param>
		/// <returns></returns>
		public static string AsJsonToClassDefine(this string _json)
		{
			System.Text.Json.JsonElement dx = System.Text.Json.JsonSerializer.Deserialize<System.Text.Json.JsonElement>(_json);
			List<string> ls = new List<string>();
			ls.Add("public class ROOT {\r\n");
			RecursionJsonElement(dx);
			ls.Add("\r\n}");
			void RecursionJsonElement(System.Text.Json.JsonElement element, int level = 0)
			{
				switch (element.ValueKind)
				{
					case System.Text.Json.JsonValueKind.Undefined:
						throw new ArgumentException();
					case System.Text.Json.JsonValueKind.Object:
						foreach (var item in element.EnumerateObject())
							RecursionJsonProperty(item, level + 1);
						break;
					case System.Text.Json.JsonValueKind.Array:
						throw new ArgumentException();
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
						throw new ArgumentException();
					default:
						throw new ArgumentException();
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
#endif
		#endregion
	}
}
