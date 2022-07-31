
using ExtensionMethods;

using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text.Json;

using Xunit;

namespace ExtensionMethodsTests
{
#if NET6_0_OR_GREATER

	public class DateOnlyTest
	{
		[Fact]
		public void AsUTCToDateTime()
		{
			var date = new DateOnly(2006, 1, 2);
			Assert.Equal(new DateTime(2006, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), date.AsLocalToDateTime());
			Assert.Equal(new DateTime(2006, 1, 2, 15, 4, 5, 0, DateTimeKind.Utc), date.AsLocalToDateTime(new TimeOnly(15, 4, 5)));
		}
		[Fact]
		public void AsLocalToDateTime()
		{
			var date = new DateOnly(2006, 1, 2);
			Assert.Equal(new DateTime(2006, 1, 2, 0, 0, 0, 0, DateTimeKind.Local), date.AsLocalToDateTime());
			Assert.Equal(new DateTime(2006, 1, 2, 15, 4, 5, 0, DateTimeKind.Local), date.AsLocalToDateTime(new TimeOnly(15, 4, 5)));
		}
	}
#endif
}
