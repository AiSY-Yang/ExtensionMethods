using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace ExtensionMethods.Common
{
	/// <summary>
	/// 常用正则
	/// </summary>
	public class RegexString
	{
		/// <summary>
		/// 中国手机号 不包含区号
		/// </summary>
#if NET7_0_OR_GREATER
		[StringSyntax(StringSyntaxAttribute.Regex)]
#endif
		public const string ChinesePhone = @"^1(3[0-9]|4[01456879]|5[0-35-9]|6[2567]|7[0-8]|8[0-9]|9[0-35-9])\d{8}$";
		/// <summary>
		/// 邮箱
		/// </summary>
#if NET7_0_OR_GREATER
		[StringSyntax(StringSyntaxAttribute.Regex)]
#endif
		public const string Email = @"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";
		/// <summary>
		/// 中国身份证号
		/// </summary>
#if NET7_0_OR_GREATER
		[StringSyntax(StringSyntaxAttribute.Regex)]
#endif
		public const string ChineseId = @"(^\d{15}$)|(^\d{18}$)|(^\d{17}(\d|X|x)$)";
		/// <summary>
		/// IPv4地址
		/// </summary>
#if NET7_0_OR_GREATER
		[StringSyntax(StringSyntaxAttribute.Regex)]
#endif
		public const string IPV4 = @"((2(5[0-5]|[0-4]\d))|[0-1]?\d{1,2})(\.((2(5[0-5]|[0-4]\d))|[0-1]?\d{1,2})){3}";
	}
}
