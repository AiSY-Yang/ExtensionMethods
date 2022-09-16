using System;

namespace ExtensionMethods
{
#if NET6_0_OR_GREATER

	/// <summary>
	/// 日期时间扩展函数
	/// </summary>
	public static class DateOnlyExtension
	{
		///<inheritdoc cref="AsUTCToDateTime(DateOnly, TimeOnly)"/>
		public static DateTime AsUTCToDateTime(this DateOnly date) => date.ToDateTime(new TimeOnly(), DateTimeKind.Utc);

		/// <summary>
		/// 当作UTC时间转换为DateTime
		/// </summary>
		/// <param name="date">日期</param>
		/// <param name="time"></param>
		/// <returns></returns>
		public static DateTime AsUTCToDateTime(this DateOnly date, TimeOnly time) => date.ToDateTime(time, DateTimeKind.Utc);

		///<inheritdoc cref="AsLocalToDateTime(DateOnly, TimeOnly)"/>
		public static DateTime AsLocalToDateTime(this DateOnly date) => date.ToDateTime(new TimeOnly(), DateTimeKind.Local);
		/// <summary>
		/// 当作本地时间转换为DateTime
		/// </summary>
		/// <param name="date">日期</param>
		/// <param name="time">时间</param>
		/// <returns></returns>
		public static DateTime AsLocalToDateTime(this DateOnly date, TimeOnly time) => date.ToDateTime(time, DateTimeKind.Local);

		/// <inheritdoc cref="GetAge(DateOnly, DateOnly)"/>
		public static int GetAge(this DateOnly birthday) => birthday.GetAge(DateOnly.FromDateTime(DateTime.Now));

		/// <summary>
		/// 获取年龄
		/// </summary>
		/// <param name="birthday">生日</param>
		/// <param name="date">计算时间</param>
		/// <returns></returns>
		public static int GetAge(this DateOnly birthday, DateOnly date) => date.Year - birthday.Year + (((date.Month << 5) + date.Day - ((birthday.Month << 5) + birthday.Day)) >> 31);
	}
#endif
}
