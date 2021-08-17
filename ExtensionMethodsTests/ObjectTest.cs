
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
			public int @int { get; set; } = 1;
			public double @double { get; set; } = 1.23;
			public DateTime @DateTime { get; set; } = DateTime.Parse("2006-01-02 15:04:05");
			public Model2 @class { get; set; } = new Model2();
			public string @string { get; set; } = "string";
			public string @null { get; set; } = null;

			public class Model2
			{
				public string p1 { get; set; } = "p1";
				public string p2 { get; set; } = "中文";
			}
		}
		[Fact]
		public void ToJson()
		{
			var obj = new Model();
			Assert.Equal(@"{
  ""int"": 1,
  ""double"": 1.23,
  ""DateTime"": ""2006-01-02T15:04:05"",
  ""class"": {
    ""p1"": ""p1"",
    ""p2"": ""中文""
  },
  ""string"": ""string"",
  ""null"": null
}", obj.ToJson());
			JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions()
			{
				Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
				IgnoreNullValues = true,
			};
			Assert.Equal(@"{""int"":1,""double"":1.23,""DateTime"":""2006-01-02T15:04:05"",""class"":{""p1"":""p1"",""p2"":""中文""},""string"":""string""}", obj.ToJson(jsonSerializerOptions));
		}
	}
}
