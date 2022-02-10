using System.Linq;

namespace ExtensionMethods
{
	/// <summary>
	/// <c>byte</c>和<c>byte[]</c>的相关扩展
	/// </summary>
	public static class ByteExtension
	{
		static readonly System.Text.Encoding strictUtf8 = new System.Text.UTF8Encoding(false, true);
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
		/// 转换为Base64字符串
		/// </summary>
		/// <param name="_byte"></param>
		/// <returns></returns>
		public static string ToBase64String(this byte[] _byte) => System.Convert.ToBase64String(_byte);
		/// <summary>
		/// 转换为16进制字符串
		/// </summary>
		/// <param name="_byte"></param>
		/// <returns></returns>
		public static string ToHexString(this byte[] _byte) => _byte.Select(x => x.ToString("x2")).Aggregate((x, y) => x + y);
		/// <summary>
		/// 转换为字符串,默认使用UTF-8 转换失败后使用此编码
		/// <list type="table">
		///		 <listheader>
		///			 <encoding>编码</encoding>
		///			 <desp>描述</desp>
		///			 <head>文件头</head>
		///		 </listheader>
		///		 <item>
		///			 <encoding>UTF-8</encoding>
		///			 <desp>无顺序,可以忽略文件头</desp>
		///			 <head>EF BB BF</head>
		///		 </item>	
		///		  <item>
		///			 <encoding>UTF-16LE</encoding>
		///			 <desp>低位在前</desp>
		///			 <head>FF FE</head>
		///		 </item>
		///		  <item>
		///			 <encoding>UTF-16BE</encoding>
		///			 <desp>高位在前</desp>
		///			 <head>FE  FF</head>
		///		 </item>
		///		  <item>
		///			 <encoding>UTF-32LE</encoding>
		///			 <desp>低位在前</desp>
		///			 <head>FF FE 00 00</head>
		///		 </item>
		///		  <item>
		///			 <encoding>UTF-32BE</encoding>
		///			 <desp>高位在前</desp>
		///			 <head>00 00 FE FF</head>
		///		 </item>
		///	</list>
		/// </summary>
		/// <param name="_byte"></param>
		/// <param name="reserveEncoding">默认使用UTF8转换,转换失败后采用此编码进行转换</param>
		/// <returns></returns>
		public static string ToString(this byte[] _byte, System.Text.Encoding reserveEncoding)
		{
			//去掉UTF8的BOM头
			if (_byte[0] == 0xEF && _byte[1] == 0xBB && _byte[2] == 0xBF)
			{
				_byte = _byte[3..];
			}
			try
			{
				return strictUtf8.GetString(_byte);
			}
			catch (global::System.Exception)
			{
				return reserveEncoding.GetString(_byte);
			};
		}

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
				case CrcOption.CRC16_HJ212:
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
		public static byte[] Hash(this byte[] byteArray, HashOption hashOption, byte[] secret = null)
		{
			switch (hashOption)
			{
				case HashOption.MD5_16:
					return byteArray.Hash(HashOption.MD5_32)[4..12];
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
						_ => throw new System.ArgumentException("枚举值不存在"),
					};
					return hash;
				case HashOption.HmacMD5:
				case HashOption.HmacSHA1:
				case HashOption.HmacSHA256:
				case HashOption.HmacSHA384:
				case HashOption.HmacSHA512:
					if (secret is null)
					{
						throw new System.ArgumentException("for Hmac algorithm,The secret is necessary");
					}
					System.Security.Cryptography.HMAC hMAC = hashOption switch
					{
						HashOption.HmacMD5 => new System.Security.Cryptography.HMACMD5(secret),
						HashOption.HmacSHA1 => new System.Security.Cryptography.HMACSHA1(secret),
						HashOption.HmacSHA256 => new System.Security.Cryptography.HMACSHA256(secret),
						HashOption.HmacSHA384 => new System.Security.Cryptography.HMACSHA384(secret),
						HashOption.HmacSHA512 => new System.Security.Cryptography.HMACSHA512(secret),
						_ => throw new System.ArgumentException("枚举值不存在"),
					};
					byte[] hashmessage = hMAC.ComputeHash(byteArray);
					return hashmessage;
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
		CRC16_HJ212,
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
		/// HmacMD5的16进制表现形式
		/// </summary>
		HmacMD5,
		/// <summary>
		/// HmacSHA1的16进制表现形式
		/// </summary>
		/// 	
		HmacSHA1,
		/// <summary>
		/// HmacSHA256的16进制表现形式
		/// </summary>
		HmacSHA256,
		/// <summary>
		/// HmacSHA384的16进制表现形式
		/// </summary>
		HmacSHA384,
		/// <summary>
		/// HmacSHA512的16进制表现形式
		/// </summary>
		HmacSHA512,
	}
}
