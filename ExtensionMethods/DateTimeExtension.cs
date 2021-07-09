using System;

namespace ExtensionMethods
{
	/// <summary>
	/// 日期时间扩展函数
	/// </summary>
	public static class DateTimeExtension
	{
		/// <summary>
		/// 转换为秒级时间戳
		/// </summary>
		/// <param name="dateTime"></param>
		/// <returns></returns>
		public static long ToSecondTimestamp(this DateTime dateTime) => (dateTime.ToUniversalTime().Ticks - 621355968000000000) / 1000_0000;
		/// <summary>
		/// 转换为毫秒级时间戳
		/// </summary>
		/// <param name="dateTime"></param>
		/// <returns></returns>
		public static long ToMilliSecondTimestamp(this DateTime dateTime) => (dateTime.ToUniversalTime().Ticks - 621355968000000000) / 1_0000;
		/// <summary>
		/// 返回当前日期所在月份的第一天
		/// </summary>
		/// <param name="dateTime"></param>
		/// <returns></returns>
		public static DateTime GetFirstDayOfMonth(this DateTime dateTime)
		{
			return new DateTime(dateTime.Year, dateTime.Month, 1);
		}
		/// <summary>
		/// 返回当前日期所在月份最后一天
		/// </summary>
		/// <param name="dateTime"></param>
		/// <returns></returns>
		public static DateTime GetLastDayOfMonth(this DateTime dateTime)
		{
			return dateTime.GetFirstDayOfMonth().AddMonths(1).AddDays(-1);
		}
		/// <summary>
		/// 对时间按照秒数取整,返回不大于指定时间且秒数为指定数字整数倍的时间,默认将秒数置0,毫秒部分被丢弃
		/// </summary>
		/// <param name="dateTime"></param>
		/// <param name="second">取整秒数,取值范围[1,60]</param>
		/// <returns></returns>
		public static DateTime TruncateSecend(this DateTime dateTime, int second = 60)
		{
			if (second <= 0 || second > 60)
				throw new ArgumentException($"参数异常 second∈[1,60]  实际second={second}");
			return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second / second * second);
		}
		/// <summary>
		/// 对时间按照分钟数取整
		/// </summary>
		/// <param name="dateTime"></param>
		/// <param name="minute">取整分钟数,取值范围[1,60]</param>
		/// <returns></returns>
		public static DateTime TruncateMinute(this DateTime dateTime, int minute = 60)
		{
			if (minute <= 0 || minute > 60)
				throw new ArgumentException($"参数异常 minute∈[1,60] 实际minute={minute}");
			return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute / minute * minute, dateTime.Second, dateTime.Millisecond);
		}
		/// <summary>
		/// 对时间按照小时数取整
		/// </summary>
		/// <param name="dateTime"></param>
		/// <param name="hour">取整分钟数,取值范围[1,24]</param>
		/// <returns></returns>
		public static DateTime TruncateHour(this DateTime dateTime, int hour = 24)
		{
			if (hour <= 0 || hour > 24)
				throw new ArgumentException($"参数异常 hour∈[1,24] 实际hour={hour}");
			return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour / hour * hour == 0 ? 1 : dateTime.Hour / hour * hour, dateTime.Minute, dateTime.Second, dateTime.Millisecond);
		}
	}
}
