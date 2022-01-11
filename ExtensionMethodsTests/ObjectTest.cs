
using ExtensionMethods;

using System;
using System.Collections.Generic;
using System.Data;
using System.Text.Json;

using Xunit;

namespace ExtensionMethodsTests
{
	public class ObjectTest
	{
		public class Model
		{
			public int Int { get; set; } = 1;
			public double @Double { get; set; } = 1.23;
			public DateTime @DateTime { get; set; } = DateTime.Parse("2006-01-02 15:04:05");
			public Model2 @Class { get; set; } = new Model2();
			public string @String { get; set; } = "string";
			public string @Null { get; set; } = null;

			public class Model2
			{
				public string P1 { get; set; } = "Upper";
#pragma warning disable IDE1006 // 命名样式
				public string p2 { get; set; } = "lower";
#pragma warning restore IDE1006 // 命名样式
				public string P3 { get; set; } = "中文";
			}
		}
		[Fact]
		public void ToJson()
		{
			var obj = new Model();
			Assert.Equal(@"{""Int"":1,""Double"":1.23,""DateTime"":""2006-01-02T15:04:05"",""Class"":{""P1"":""Upper"",""p2"":""lower"",""P3"":""中文""},""String"":""string"",""Null"":null}", obj.ToJson());
			JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions()
			{
				Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
#if NETSTANDARD2_1_OR_GREATER || NET6_0_OR_GREATER
				DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
#else
				IgnoreNullValues = true,
#endif
			};
			Assert.Equal(@"{""Int"":1,""Double"":1.23,""DateTime"":""2006-01-02T15:04:05"",""Class"":{""P1"":""Upper"",""p2"":""lower"",""P3"":""中文""},""String"":""string""}", obj.ToJson(jsonSerializerOptions));
		}
	}
}
