using System;

namespace ExtensionMethods
{
	/// <summary>
	/// 枚举扩展
	/// </summary>
	public static class EnumExtension
	{
		/// <summary>
		/// 通过反射获取枚举的描述 如果不存在描述则返回字符串
		/// </summary>
		/// <param name="source"></param>
		/// <returns></returns>
		public static string GetDescription(this Enum source)
		{
			System.Reflection.FieldInfo fi = source.GetType().GetField(source.ToString());
			System.ComponentModel.DescriptionAttribute[] attributes = (System.ComponentModel.DescriptionAttribute[])fi.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);

			if (attributes != null && attributes.Length > 0) return attributes[0].Description;
			else return source.ToString();
		}
	}
}
