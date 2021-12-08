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
		public void AutoOrder()
		{
			Dictionary<int, string> dic = new Dictionary<int, string>() { { 1, "C" }, { 3, "A" }, { 2, "B" } };

			var result = dic.AsQueryable().AutoOrder("", "Key").ToList();
			Assert.Equal(3, result.Count);
			Assert.Equal(1, result.First().Key);
			Assert.Equal(2, result.Last().Key);

			result = dic.AsQueryable().AutoOrder("asc", "Key").ToList();
			Assert.Equal(3, result.Count);
			Assert.Equal(1, result.First().Key);
			Assert.Equal(3, result.Last().Key);
			result = dic.AsQueryable().AutoOrder("asc", "Value").ToList();
			Assert.Equal(3, result.Count);
			Assert.Equal(3, result.First().Key);
			Assert.Equal(1, result.Last().Key);

			result = dic.AsQueryable().AutoOrder("ASC", "Key").ToList();
			Assert.Equal(3, result.Count);
			Assert.Equal(1, result.First().Key);
			Assert.Equal(3, result.Last().Key);
			result = dic.AsQueryable().AutoOrder("ASC", "Value").ToList();
			Assert.Equal(3, result.Count);
			Assert.Equal(3, result.First().Key);
			Assert.Equal(1, result.Last().Key);
			Assert.Throws<ArgumentException>(() => dic.AsQueryable().AutoOrder("asc", "").ToList());
			Assert.Throws<ArgumentException>(() => dic.AsQueryable().AutoOrder("asc", "AA").ToList());

			result = dic.AsQueryable().AutoOrder("desc", "Key").ToList();
			Assert.Equal(3, result.Count);
			Assert.Equal(3, result.First().Key);
			Assert.Equal(1, result.Last().Key);
			result = dic.AsQueryable().AutoOrder("desc", "Value").ToList();
			Assert.Equal(3, result.Count);
			Assert.Equal(1, result.First().Key);
			Assert.Equal(3, result.Last().Key);

			result = dic.AsQueryable().AutoOrder("DESC", "Key").ToList();
			Assert.Equal(3, result.Count);
			Assert.Equal(3, result.First().Key);
			Assert.Equal(1, result.Last().Key);
			result = dic.AsQueryable().AutoOrder("DESC", "Value").ToList();
			Assert.Equal(3, result.Count);
			Assert.Equal(1, result.First().Key);
			Assert.Equal(3, result.Last().Key);
			Assert.Throws<ArgumentException>(() => dic.AsQueryable().AutoOrder("desc", "").ToList());
			Assert.Throws<ArgumentException>(() => dic.AsQueryable().AutoOrder("desc", "AA").ToList());
		}
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
