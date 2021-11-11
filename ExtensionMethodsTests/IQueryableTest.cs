using ExtensionMethods;

using System;
using System.Collections.Generic;
using System.Linq;

using Xunit;

namespace ExtensionMethodsTests
{
	public class IQueryableTest
	{
		[Fact]
		public void OrderBy()
		{
			Dictionary<int, string> dic = new Dictionary<int, string>() { { 1, "C" }, { 3, "A" }, { 2, "B" } };
			var result = dic.AsQueryable().OrderBy("Key").ToList();
			Assert.Equal(3, result.Count);
			Assert.Equal(1, result.First().Key);
			Assert.Equal(3, result.Last().Key);
			result = dic.AsQueryable().OrderBy("Value").ToList();
			Assert.Equal(3, result.Count);
			Assert.Equal(3, result.First().Key);
			Assert.Equal(1, result.Last().Key);
			Assert.Throws<ArgumentException>(() => dic.AsQueryable().OrderBy("").ToList());
			Assert.Throws<ArgumentException>(() => dic.AsQueryable().OrderBy("AA").ToList());
		}
		[Fact]
		public void OrderByDescending()
		{
			Dictionary<int, string> dic = new Dictionary<int, string>() { { 1, "C" }, { 3, "A" }, { 2, "B" } };
			var result = dic.AsQueryable().OrderByDescending("Key").ToList();
			Assert.Equal(3, result.Count);
			Assert.Equal(3, result.First().Key);
			Assert.Equal(1, result.Last().Key);
			result = dic.AsQueryable().OrderByDescending("Value").ToList();
			Assert.Equal(3, result.Count);
			Assert.Equal(1, result.First().Key);
			Assert.Equal(3, result.Last().Key);
			Assert.Throws<ArgumentException>(() => dic.AsQueryable().OrderByDescending("").ToList());
			Assert.Throws<ArgumentException>(() => dic.AsQueryable().OrderByDescending("AA").ToList());
		}
	}
}
