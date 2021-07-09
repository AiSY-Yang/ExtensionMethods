using System.Text;

namespace ExtensionMethods
{
	/// <summary>
	/// 
	/// </summary>
	public static class ByteExtension
	{
		/// <summary>
		/// 转换为UTF8字符串
		/// </summary>
		/// <param name="_byte"></param>
		/// <returns></returns>
		public static string ToUtf8String(this byte[] _byte) => Encoding.UTF8.GetString(_byte);
		/// <summary>
		/// 转换为UTF8字符串
		/// </summary>
		/// <param name="_byte"></param>
		/// <param name="index">开始位置</param>
		/// <param name="count">结束位置</param>
		/// <returns></returns>
		public static string ToUtf8String(this byte[] _byte, int index, int count) => Encoding.UTF8.GetString(_byte, index, count);
		/// <summary>
		/// 转换为UTF8字符串
		/// </summary>
		/// <param name="_byte"></param>
		/// <returns></returns>
		public static string ToBase64String(this byte[] _byte) => System.Convert.ToBase64String(_byte);
	}
}
