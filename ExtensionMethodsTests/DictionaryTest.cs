
using ExtensionMethods;

using System;
using System.Collections.Generic;
using System.Data;

using Xunit;

namespace ExtensionMethodsTests
{
	public class DictionaryTest
	{
		[Fact]
		public void GetFirstDayOfMonth()
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.AddOrUpdate("hello", "world");
			Assert.Single(dictionary);
			dictionary.AddOrUpdate("hello", "china");
			Assert.Single(dictionary);
			dictionary.AddOrUpdate("你好", "世界");
			Assert.Equal(2, dictionary.Count);
			dictionary.AddOrUpdate("你好", "中国");
			Assert.Equal(2, dictionary.Count);
		}
	}
}
