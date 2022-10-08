
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

using ExtensionMethods;
using ExtensionMethods.JsonSerializerConverts;

using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

using OfficeOpenXml.ConditionalFormatting;

using Xunit;

namespace ExtensionMethodsTests
{
	public class JsonSerializerConvertsTest
	{
		public JsonSerializerConvertsTest()
		{
		}
		MyClass value = new MyClass()
		{
			MyEnumWith1 = MyEnum.Key,
			MyEnumWith0 = 0,
			NullableMyEnumWith0 = 0,
			NullableMyEnumWith1 = MyEnum.Key,
			NullableMyEnumWithNull = null,
			NullableMyEnumWith2 = MyEnum.NoDescriptionKey,
		};
#if NET5_0_OR_GREATER
		[Fact]
		public void EnumDescriptionJsonConverter()
		{
			JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions() { WriteIndented = true };
			Assert.Equal(@"{
  ""MyEnumWith0"": 0,
  ""MyEnumWith1"": 1,
  ""NullableMyEnumWith0"": 0,
  ""NullableMyEnumWith1"": 1,
  ""NullableMyEnumWithNull"": null,
  ""NullableMyEnumWith2"": 2
}", JsonSerializer.Serialize(value, jsonSerializerOptions));
			jsonSerializerOptions = new JsonSerializerOptions() { WriteIndented = true };
			jsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
			Assert.Equal(@"{
  ""MyEnumWith0"": 0,
  ""MyEnumWith1"": ""Key"",
  ""NullableMyEnumWith0"": 0,
  ""NullableMyEnumWith1"": ""Key"",
  ""NullableMyEnumWithNull"": null,
  ""NullableMyEnumWith2"": ""NoDescriptionKey""
}", JsonSerializer.Serialize(value, jsonSerializerOptions));
			jsonSerializerOptions = new JsonSerializerOptions() { WriteIndented = true };
			jsonSerializerOptions.Converters.Add(new EnumDescriptionJsonConverter());
			Assert.Equal(@"{
  ""MyEnumWith0"": ""0"",
  ""MyEnumWith1"": ""Description"",
  ""NullableMyEnumWith0"": ""0"",
  ""NullableMyEnumWith1"": ""Description"",
  ""NullableMyEnumWithNull"": null,
  ""NullableMyEnumWith2"": ""NoDescriptionKey""
}", JsonSerializer.Serialize(value, jsonSerializerOptions));

		}
#endif
		public class MyClass
		{
			public MyEnum MyEnumWith0 { get; set; }
			public MyEnum MyEnumWith1 { get; set; }
			public MyEnum? NullableMyEnumWith0 { get; set; }
			public MyEnum? NullableMyEnumWith1 { get; set; }
			public MyEnum? NullableMyEnumWithNull { get; set; }
			public MyEnum? NullableMyEnumWith2 { get; set; }
		}

		public enum MyEnum
		{
			[Description("Description")]
			Key = 1,
			NoDescriptionKey = 2,
		}
	}
}
