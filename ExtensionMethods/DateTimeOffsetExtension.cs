using System;

namespace ExtensionMethods
{
	/// <summary>
	/// 日期时间扩展函数
	/// </summary>
	public static class DateTimeOffsetExtension
	{
		/// <summary>
		/// 转换为秒级时间戳
		/// </summary>
		/// <param name="dateTime"></param>
		/// <returns></returns>
		public static long ToSecondTimestamp(this DateTimeOffset dateTime) => (dateTime.ToUniversalTime().Ticks - 621355968000000000) / 1000_0000;
		/// <summary>
		/// 转换为13位毫秒级时间戳
		/// </summary>
		/// <param name="dateTime"></param>
		/// <returns></returns>
		public static long ToMilliSecondTimestamp(this DateTimeOffset dateTime) => (dateTime.ToUniversalTime().Ticks - 621355968000000000) / 1_0000;
#if NET6_0_OR_GREATER
		/// <summary>
		/// 转换为DateOnly
		/// </summary>
		/// <param name="dateTime"></param>
		/// <returns></returns>
		public static DateOnly ToLocalDateOnly(this DateTimeOffset dateTime) => DateOnly.FromDateTime(dateTime.LocalDateTime);
#endif
		/// <summary>
		/// 返回当前日期所在月份的第一天
		/// </summary>
		/// <param name="dateTime"></param>
		/// <returns></returns>
		public static DateTimeOffset GetFirstDayOfMonth(this DateTimeOffset dateTime) => new DateTimeOffset(dateTime.Year, dateTime.Month, 1, 0, 0, 0, dateTime.Offset);
		/// <summary>
		/// 返回当前日期所在月份最后一天
		/// </summary>
		/// <param name="dateTime"></param>
		/// <returns></returns>
		public static DateTimeOffset GetLastDayOfMonth(this DateTimeOffset dateTime) => dateTime.GetFirstDayOfMonth().AddMonths(1).AddDays(-1);

		/// <inheritdoc cref="DateTimeExtension.GetAge(DateTime, DateTime)"/>
		public static int GetAge(this DateTimeOffset birthday) => birthday.DateTime.GetAge(DateTime.Now);

		/// <summary>
		/// 对时间按照秒数取整,返回不大于指定时间且秒数为指定数字整数倍的时间,默认将秒数置0,毫秒部分被丢弃
		/// </summary>
		/// <param name="dateTime"></param>
		/// <param name="second">取整秒数,取值范围[1,60]</param>
		/// <returns></returns>
		public static DateTimeOffset TruncateSecend(this DateTimeOffset dateTime, int second = 60)
		{
			if (second <= 0 || second > 60)
				throw new ArgumentException($"参数异常 second∈[1,60]  实际second={second}");
			return new DateTimeOffset(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second / second * second, dateTime.Offset);
		}
		/// <summary>
		/// 对时间按照分钟数取整
		/// </summary>
		/// <param name="dateTime"></param>
		/// <param name="minute">取整分钟数,取值范围[1,60]</param>
		/// <returns></returns>
		public static DateTimeOffset TruncateMinute(this DateTimeOffset dateTime, int minute = 60)
		{
			if (minute <= 0 || minute > 60)
				throw new ArgumentException($"参数异常 minute∈[1,60] 实际minute={minute}");
			return new DateTimeOffset(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute / minute * minute, dateTime.Second, dateTime.Millisecond, dateTime.Offset);
		}
		/// <summary>
		/// 对时间按照小时数取整
		/// </summary>
		/// <param name="dateTime"></param>
		/// <param name="hour">取整分钟数,取值范围[1,24]</param>
		/// <returns></returns>
		public static DateTimeOffset TruncateHour(this DateTimeOffset dateTime, int hour = 24)
		{
			if (hour <= 0 || hour > 24)
				throw new ArgumentException($"参数异常 hour∈[1,24] 实际hour={hour}");
			return new DateTimeOffset(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour / hour * hour == 0 ? 1 : dateTime.Hour / hour * hour, dateTime.Minute, dateTime.Second, dateTime.Millisecond, dateTime.Offset);
		}
	}
}
