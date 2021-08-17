using System.Threading.Tasks.Dataflow;

namespace ExtensionMethods
{
	/// <summary>
	/// <c>byte</c>和<c>byte[]</c>的相关扩展
	/// </summary>
	public static class ByteExtension
	{
		/// <summary>
		/// 转换为UTF8字符串
		/// </summary>
		/// <param name="_byte"></param>
		/// <returns></returns>
		public static string ToUtf8String(this byte[] _byte) => System.Text.Encoding.UTF8.GetString(_byte);
		/// <summary>
		/// 转换为UTF8字符串
		/// </summary>
		/// <param name="_byte"></param>
		/// <param name="index">开始位置</param>
		/// <param name="count">结束位置</param>
		/// <returns></returns>
		public static string ToUtf8String(this byte[] _byte, int index, int count) => System.Text.Encoding.UTF8.GetString(_byte, index, count);
		/// <summary>
		/// 转换为UTF8字符串
		/// </summary>
		/// <param name="_byte"></param>
		/// <returns></returns>
		public static string ToBase64String(this byte[] _byte) => System.Convert.ToBase64String(_byte);

		/// <summary>
		/// 计算CRC校验码
		/// </summary>
		/// <param name="byteArray"></param>
		/// <param name="crcOption"></param>
		/// <returns></returns>
		/// <exception cref="System.ArgumentException"></exception>
		public static string CRC(this byte[] byteArray, CrcOption crcOption)
		{
			switch (crcOption)
			{
				case CrcOption.CRC16_HJT212:
					ushort crc = 0xFFFF;
					int len = byteArray.Length;
					for (int i = 0; i < len; i++)
					{
						crc = (ushort)((crc >> 8) ^ byteArray[i]);
						for (int j = 0; j < 8; j++)
							crc = (crc & 1) == 1 ? (ushort)((crc >> 1) ^ 0xA001) : (ushort)(crc >> 1);
					}
					return string.Format("{0:X}", crc).PadLeft(4, '0');
				default:
					throw new System.ArgumentException("枚举值不存在");
			}
		}
		/// <summary>
		/// 计算HASH
		/// </summary>
		/// <param name="byteArray"></param>
		/// <param name="hashOption"></param>
		/// <param name="secret">HMAC形式的HASH计算需要密钥</param>
		/// <returns></returns>
		public static string Hash(this byte[] byteArray, HashOption hashOption, string secret = null)
		{
			switch (hashOption)
			{
				case HashOption.MD5_16:
					return byteArray.Hash(HashOption.MD5_32)[8..24];
				case HashOption.MD5_32:
				case HashOption.SHA1:
				case HashOption.SHA256:
				case HashOption.SHA384:
				case HashOption.SHA512:
					byte[] hash = hashOption switch
					{
						HashOption.MD5_32 => System.Security.Cryptography.MD5.Create().ComputeHash(byteArray),
						HashOption.SHA1 => System.Security.Cryptography.SHA1.Create().ComputeHash(byteArray),
						HashOption.SHA256 => System.Security.Cryptography.SHA256.Create().ComputeHash(byteArray),
						HashOption.SHA384 => System.Security.Cryptography.SHA384.Create().ComputeHash(byteArray),
						HashOption.SHA512 => System.Security.Cryptography.SHA512.Create().ComputeHash(byteArray),
						_ => throw new System.ArgumentException(),
					};
					System.Text.StringBuilder builder = new System.Text.StringBuilder();
					foreach (var item in hash)
					{
						builder.Append(item.ToString("x2"));
					}
					return builder.ToString();
				case HashOption.HmacMD5:
				case HashOption.HmacSHA1:
				case HashOption.HmacSHA256:
				case HashOption.HmacSHA384:
				case HashOption.HmacSHA512:
				case HashOption.HmacMD5_Base64:
				case HashOption.HmacSHA1_Base64:
				case HashOption.HmacSHA256_Base64:
				case HashOption.HmacSHA384_Base64:
				case HashOption.HmacSHA512_Base64:
					if (string.IsNullOrEmpty(secret))
					{
						throw new System.ArgumentException("for Hmac algorithm,The secret is necessary");
					}
					System.Security.Cryptography.HMAC hMAC = hashOption switch
					{
						HashOption.HmacMD5 => new System.Security.Cryptography.HMACMD5(System.Text.Encoding.UTF8.GetBytes(secret)),
						HashOption.HmacSHA1 => new System.Security.Cryptography.HMACSHA1(System.Text.Encoding.UTF8.GetBytes(secret)),
						HashOption.HmacSHA256 => new System.Security.Cryptography.HMACSHA256(System.Text.Encoding.UTF8.GetBytes(secret)),
						HashOption.HmacSHA384 => new System.Security.Cryptography.HMACSHA384(System.Text.Encoding.UTF8.GetBytes(secret)),
						HashOption.HmacSHA512 => new System.Security.Cryptography.HMACSHA512(System.Text.Encoding.UTF8.GetBytes(secret)),
						HashOption.HmacMD5_Base64 => new System.Security.Cryptography.HMACMD5(System.Text.Encoding.UTF8.GetBytes(secret)),
						HashOption.HmacSHA1_Base64 => new System.Security.Cryptography.HMACSHA1(System.Text.Encoding.UTF8.GetBytes(secret)),
						HashOption.HmacSHA256_Base64 => new System.Security.Cryptography.HMACSHA256(System.Text.Encoding.UTF8.GetBytes(secret)),
						HashOption.HmacSHA384_Base64 => new System.Security.Cryptography.HMACSHA384(System.Text.Encoding.UTF8.GetBytes(secret)),
						HashOption.HmacSHA512_Base64 => new System.Security.Cryptography.HMACSHA512(System.Text.Encoding.UTF8.GetBytes(secret)),
						_ => throw new System.ArgumentException(),
					};
					byte[] hashmessage = hMAC.ComputeHash(byteArray);
					switch (hashOption)
					{
						case HashOption.HmacMD5_Base64:
						case HashOption.HmacSHA1_Base64:
						case HashOption.HmacSHA256_Base64:
						case HashOption.HmacSHA384_Base64:
						case HashOption.HmacSHA512_Base64:
							return System.Convert.ToBase64String(hashmessage);
					}
					System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
					foreach (var item in hashmessage)
					{
						stringBuilder.Append(item.ToString("x2"));
					}
					return stringBuilder.ToString();
				default:
					throw new System.ArgumentException("枚举值不存在");
			}

		}
	}
	/// <summary>
	/// CRC类别
	/// </summary>
	public enum CrcOption
	{
		/// <summary>
		/// HJT212协议CRC校验
		/// </summary>
		CRC16_HJT212,
	}
	/// <summary>
	/// HASH类别
	/// </summary>
	public enum HashOption
	{
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
		/// 计算HmacMD5
		/// </summary>
		HmacMD5,
		/// <summary>
		/// 计算HmacSHA1
		/// </summary>
		/// 	
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
		/// 计算HmacMD5并base64编码
		/// </summary>
		HmacMD5_Base64,
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
	}
}
