using System;
using System.Text;

namespace ExtensionMethods
{
	/// <summary>
	/// <c>decimal</c>的相关扩展
	/// </summary>
	public static class DecimalExtension
	{
		private static readonly char[] RmbDigits = {
			'零', '壹', '贰', '叁', '肆', '伍', '陆', '柒', '捌', '玖' };

		private static readonly string[] SectionChars = {
			 string.Empty, "拾", "佰", "仟", "万" };
		/// <summary>
		/// 转换为人民币大写形式
		/// <a href="https://github.com/Sandwych/rmb_converter/blob/master/src/dotnet/Sandwych.RmbConverter/RmbUpperConverter.cs"></a>
		/// </summary>
		/// <param name="price"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		public static string ToRmbUpper(this decimal price)
		{
			if (price < 0M || price >= 9999999999999999.99M)
			{
				throw new ArgumentOutOfRangeException(nameof(price));
			}

			price = Math.Round(price, 2);
			var sb = new StringBuilder();

			var integerPart = (long)price;
			var wanyiPart = integerPart / 1000000000000L;
			var yiPart = integerPart % 1000000000000L / 100000000L;
			var wanPart = integerPart % 100000000L / 10000L;
			var qianPart = integerPart % 10000L;
			var decPart = (long)(price * 100) % 100;

			int zeroCount = 0;
			//处理万亿以上的部分
			if (integerPart >= 1000000000000L && wanyiPart > 0)
			{
				zeroCount = ParseInteger(sb, wanyiPart, true, zeroCount);
				sb.Append('万');
			}

			//处理亿到千亿的部分
			if (integerPart >= 100000000L && yiPart > 0)
			{
				var isFirstSection = integerPart >= 100000000L && integerPart < 1000000000000L;
				zeroCount = ParseInteger(sb, yiPart, isFirstSection, zeroCount);
				sb.Append('亿');
			}

			//处理万的部分
			if (integerPart >= 10000L && wanPart > 0)
			{
				var isFirstSection = integerPart >= 1000L && integerPart < 10000000L;
				zeroCount = ParseInteger(sb, wanPart, isFirstSection, zeroCount);
				sb.Append('万');
			}

			//处理千及以后的部分
			if (qianPart > 0)
			{
				var isFirstSection = integerPart < 1000L;
				zeroCount = ParseInteger(sb, qianPart, isFirstSection, zeroCount);
			}
			else
			{
				zeroCount += 1;
			}

			if (integerPart > 0)
			{
				sb.Append('元');
			}

			//处理小数
			if (decPart > 0)
			{
				ParseDecimal(sb, integerPart, decPart, zeroCount);
			}
			else if (decPart <= 0 && integerPart > 0)
			{
				sb.Append('整');
			}
			else
			{
				sb.Append("零元整");
			}

			return sb.ToString();
		}
		private static int ParseInteger(StringBuilder sb, long integer, bool isFirstSection, int zeroCount)
		{
			var nDigits = (int)Math.Floor(Math.Log10(integer)) + 1;
			if (!isFirstSection && integer < 1000)
			{
				zeroCount++;
			}
			for (var i = 0; i < nDigits; i++)
			{
				var factor = (long)Math.Pow(10, nDigits - 1 - i);
				var digit = integer / factor;
				if (digit > 0)
				{
					if (zeroCount > 0)
					{
						sb.Append('零');
					}
					sb.Append(RmbDigits[digit]);
					sb.Append(SectionChars[nDigits - i - 1]);
					zeroCount = 0;
				}
				else
				{
					zeroCount++;
				}
				integer -= integer / factor * factor;
			}
			return zeroCount;
		}
		private static void ParseDecimal(StringBuilder sb, long integerPart, long decPart, int zeroCount)
		{
			var jiao = decPart / 10;
			var fen = decPart % 10;

			if (zeroCount > 0 && (jiao > 0 || fen > 0) && integerPart > 0)
			{
				sb.Append('零');
			}

			if (jiao > 0)
			{
				sb.Append(RmbDigits[jiao]);
				sb.Append('角');
			}
			if (zeroCount == 0 && jiao == 0 && fen > 0 && integerPart > 0)
			{
				sb.Append('零');
			}
			if (fen > 0)
			{
				sb.Append(RmbDigits[fen]);
				sb.Append('分');
			}
			else
			{
				sb.Append('整');
			}
		}
	}
}
