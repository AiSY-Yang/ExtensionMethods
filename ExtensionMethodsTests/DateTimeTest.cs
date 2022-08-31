
using ExtensionMethods;

using System;
using System.Data;

using Xunit;

namespace ExtensionMethodsTests
{
	public class DateTimeTest
	{
		[Fact]
		public void ToTimestamp()
		{
			Assert.Equal(1136185445L, DateTime.Parse("2006-01-02 15:04:05").ToSecondTimestamp());
			Assert.Equal(1136185445000L, DateTime.Parse("2006-01-02 15:04:05").ToMilliSecondTimestamp());
		}

		[Fact]
		public void GetFirstDayOfMonth()
		{
			Assert.Equal(DateTime.Parse("1994-10-01"), DateTime.Parse("1994-10-08").GetFirstDayOfMonth());
			Assert.Equal(DateTime.Parse("1994-02-01"), DateTime.Parse("1994-02-28").GetFirstDayOfMonth());
			Assert.Equal(DateTime.Parse("1994-12-01"), DateTime.Parse("1994-12-31").GetFirstDayOfMonth());
			Assert.Equal(DateTime.Parse("2000-02-01"), DateTime.Parse("2000-02-28").GetFirstDayOfMonth());
			Assert.Equal(DateTime.Parse("2000-02-01"), DateTime.Parse("2000-02-29").GetFirstDayOfMonth());
		}
		[Fact]
		public void GetLastDayOfMonth()
		{
			Assert.Equal(DateTime.Parse("1994-10-31"), DateTime.Parse("1994-10-08").GetLastDayOfMonth());
			Assert.Equal(DateTime.Parse("1994-10-31"), DateTime.Parse("1994-10-01").GetLastDayOfMonth());
			Assert.Equal(DateTime.Parse("1994-11-30"), DateTime.Parse("1994-11-01").GetLastDayOfMonth());
			Assert.Equal(DateTime.Parse("2000-02-29"), DateTime.Parse("2000-02-01").GetLastDayOfMonth());
			Assert.Equal(DateTime.Parse("2001-02-28"), DateTime.Parse("2001-02-01").GetLastDayOfMonth());
			Assert.Equal(DateTime.Parse("2004-02-29"), DateTime.Parse("2004-02-01").GetLastDayOfMonth());
		}
		[Theory]
		[InlineData("1994-10-29", "1994-10-29", 0)]
		[InlineData("1994-10-29", "1995-10-28", 0)]
		[InlineData("1994-10-29", "1995-10-29", 1)]
		[InlineData("1994-10-29", "1995-10-30", 1)]

		[InlineData("1994-10-29", "2008-10-28", 13)]
		[InlineData("1994-10-29", "2008-10-29", 14)]
		[InlineData("1994-10-29", "2008-10-30", 14)]

		[InlineData("1994-10-31", "2008-10-30", 13)]
		[InlineData("1994-10-31", "2008-10-31", 14)]
		[InlineData("1994-10-31", "2008-11-01", 14)]
	
		[InlineData("2000-02-28", "2000-02-29", 0)]
		[InlineData("2000-02-28", "2004-02-27", 3)]
		[InlineData("2000-02-28", "2004-02-28", 4)]
		[InlineData("2000-02-28", "2004-02-29", 4)]
		
		[InlineData("2000-02-29", "2000-02-29", 0)]
		[InlineData("2000-02-29", "2004-02-27", 3)]
		[InlineData("2000-02-29", "2004-02-28", 3)]
		[InlineData("2000-02-29", "2004-02-29", 4)]
		public void GetAge(string birthday, string currentDay, int age)
		{
			Assert.Equal(age, DateTime.Parse(birthday).GetAge(DateTime.Parse(currentDay)));
		}

		[Fact]
		public void TruncateSecend()
		{
			Assert.Throws<ArgumentException>(() => DateTime.Parse("2006-01-02 15:04:05").TruncateSecend(-1));
			Assert.Throws<ArgumentException>(() => DateTime.Parse("2006-01-02 15:04:05").TruncateSecend(0));
			Assert.Equal(DateTime.Parse("2006-01-02 15:04:05"), DateTime.Parse("2006-01-02 15:04:05").TruncateSecend(1));
			Assert.Equal(DateTime.Parse("2006-01-02 15:04:04"), DateTime.Parse("2006-01-02 15:04:05").TruncateSecend(2));
			Assert.Equal(DateTime.Parse("2006-01-02 15:04:03"), DateTime.Parse("2006-01-02 15:04:05").TruncateSecend(3));
			Assert.Equal(DateTime.Parse("2006-01-02 15:04:05"), DateTime.Parse("2006-01-02 15:04:05").TruncateSecend(5));
			Assert.Equal(DateTime.Parse("2006-01-02 15:04:00"), DateTime.Parse("2006-01-02 15:04:05").TruncateSecend(10));
			Assert.Equal(DateTime.Parse("2006-01-02 15:04:00"), DateTime.Parse("2006-01-02 15:04:05").TruncateSecend(30));
			Assert.Equal(DateTime.Parse("2006-01-02 15:04:00"), DateTime.Parse("2006-01-02 15:04:05").TruncateSecend(55));
			Assert.Equal(DateTime.Parse("2006-01-02 15:04:00"), DateTime.Parse("2006-01-02 15:04:05").TruncateSecend(60));
			Assert.Equal(DateTime.Parse("2006-01-02 15:04:00"), DateTime.Parse("2006-01-02 15:04:05").TruncateSecend());
			Assert.Throws<ArgumentException>(() => DateTime.Parse("2006-01-02 15:04:05").TruncateSecend(61));
		}

		[Fact]
		public void TruncateMinute()
		{
			Assert.Throws<ArgumentException>(() => DateTime.Parse("2006-01-02 15:04:05").TruncateMinute(-1));
			Assert.Throws<ArgumentException>(() => DateTime.Parse("2006-01-02 15:04:05").TruncateMinute(0));
			Assert.Equal(DateTime.Parse("2006-01-02 15:04:05"), DateTime.Parse("2006-01-02 15:04:05").TruncateMinute(1));
			Assert.Equal(DateTime.Parse("2006-01-02 15:04:05"), DateTime.Parse("2006-01-02 15:04:05").TruncateMinute(2));
			Assert.Equal(DateTime.Parse("2006-01-02 15:03:05"), DateTime.Parse("2006-01-02 15:04:05").TruncateMinute(3));
			Assert.Equal(DateTime.Parse("2006-01-02 15:00:05"), DateTime.Parse("2006-01-02 15:04:05").TruncateMinute(5));
			Assert.Equal(DateTime.Parse("2006-01-02 15:00:05"), DateTime.Parse("2006-01-02 15:04:05").TruncateMinute(10));
			Assert.Equal(DateTime.Parse("2006-01-02 15:00:05"), DateTime.Parse("2006-01-02 15:04:05").TruncateMinute(30));
			Assert.Equal(DateTime.Parse("2006-01-02 15:00:05"), DateTime.Parse("2006-01-02 15:04:05").TruncateMinute(55));
			Assert.Equal(DateTime.Parse("2006-01-02 15:00:05"), DateTime.Parse("2006-01-02 15:04:05").TruncateMinute(60));
			Assert.Equal(DateTime.Parse("2006-01-02 15:00:05"), DateTime.Parse("2006-01-02 15:04:05").TruncateMinute());
			Assert.Throws<ArgumentException>(() => DateTime.Parse("2006-01-02 15:04:05").TruncateMinute(61));
			for (int i = 0; i < 60; i++)
			{
				Assert.Contains(DateTime.Parse($"2006-01-02 15:{i:00}:05").TruncateMinute(15).Minute, new int[] { 0, 15, 30, 45 });
			}
		}
		[Fact]
		public void TruncateHour()
		{
			Assert.Throws<ArgumentException>(() => DateTime.Parse("2006-01-02 15:04:05").TruncateHour(-1));
			Assert.Throws<ArgumentException>(() => DateTime.Parse("2006-01-02 15:04:05").TruncateHour(0));
			Assert.Equal(DateTime.Parse("2006-01-02 15:04:05"), DateTime.Parse("2006-01-02 15:04:05").TruncateHour(1));
			Assert.Equal(DateTime.Parse("2006-01-02 14:04:05"), DateTime.Parse("2006-01-02 15:04:05").TruncateHour(2));
			Assert.Equal(DateTime.Parse("2006-01-02 15:04:05"), DateTime.Parse("2006-01-02 15:04:05").TruncateHour(3));
			Assert.Equal(DateTime.Parse("2006-01-02 15:04:05"), DateTime.Parse("2006-01-02 15:04:05").TruncateHour(5));
			Assert.Equal(DateTime.Parse("2006-01-02 10:04:05"), DateTime.Parse("2006-01-02 15:04:05").TruncateHour(10));
			Assert.Equal(DateTime.Parse("2006-01-02 01:04:05"), DateTime.Parse("2006-01-02 15:04:05").TruncateHour());
			Assert.Throws<ArgumentException>(() => DateTime.Parse("2006-01-02 15:04:05").TruncateHour(25));
		}
	}
}
