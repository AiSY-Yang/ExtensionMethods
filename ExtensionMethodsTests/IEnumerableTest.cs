﻿using ExtensionMethods;

using MoreLinq;

using System;
using System.Collections.Generic;
using System.Linq;

using Xunit;

namespace ExtensionMethodsTests
{
	public class IEnumerableTest
	{
		[Fact]
		public void Where()
		{
			List<int> list = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
			Assert.Equal(5, list.Where(x => x > 5).Count());
			Assert.Equal(5, list.Where(true, x => x > 5).Count());
			Assert.Equal(10, list.Where(false, x => x > 5).Count());
		}
		[Fact]
		public void LeftJoin()
		{
			Dictionary<int, string> left = new Dictionary<int, string>();
			Dictionary<int, string> right = new Dictionary<int, string>();
			left.Add(1, "left1");
			left.Add(2, "left2");
			right.Add(1, "right1");
			var result = left.LeftJoin(right, x => x.Key, y => y.Key, (x, y) => new { key = x.Key, value1 = x.Value, value2 = y.Value, value3 = y.Value ?? x.Value }).ToList();
			Assert.Equal(2, result.Count);
			Assert.Equal(1, result[0].key);
			Assert.Equal("left1", result[0].value1);
			Assert.Equal("right1", result[0].value2);
			Assert.Equal("right1", result[0].value3);
			Assert.Equal(2, result[1].key);
			Assert.Equal("left2", result[1].value1);
			Assert.Null(result[1].value2);
			Assert.Equal("left2", result[1].value3);
			Dictionary<int, int> a = new Dictionary<int, int>();
			Dictionary<int, int> b = new Dictionary<int, int>();
			a.Add(1, 1);
			a.Add(2, 2);
			b.Add(1, 1);
		}
		[Fact]
		public void Pageing()
		{
			List<int> list = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
			//when page<=0 return page1
			//when pagesize<=0 return pageSize is list.Count()
			var result = list.Pageing(-1, -1);
			Assert.Equal(1, result.FirstOrDefault());
			Assert.Equal(10, result.Count());
			result = list.Pageing(-1, 0);
			Assert.Equal(1, result.FirstOrDefault());
			Assert.Equal(10, result.Count());
			result = list.Pageing(-1, 1);
			Assert.Equal(1, result.FirstOrDefault());
			Assert.Single(result);
			result = list.Pageing(-1, 3);
			Assert.Equal(1, result.FirstOrDefault());
			Assert.Equal(3, result.Count());
			result = list.Pageing(-1, 20);
			Assert.Equal(1, result.FirstOrDefault());
			Assert.Equal(10, result.Count());

			result = list.Pageing(0, -1);
			Assert.Equal(1, result.FirstOrDefault());
			Assert.Equal(10, result.Count());
			result = list.Pageing(0, 0);
			Assert.Equal(1, result.FirstOrDefault());
			Assert.Equal(10, result.Count());
			result = list.Pageing(0, 1);
			Assert.Equal(1, result.FirstOrDefault());
			Assert.Single(result);
			result = list.Pageing(0, 3);
			Assert.Equal(1, result.FirstOrDefault());
			Assert.Equal(3, result.Count());
			result = list.Pageing(0, 20);
			Assert.Equal(1, result.FirstOrDefault());
			Assert.Equal(10, result.Count());

			result = list.Pageing(1, -1);
			Assert.Equal(1, result.FirstOrDefault());
			Assert.Equal(10, result.Count());
			result = list.Pageing(1, 0);
			Assert.Equal(1, result.FirstOrDefault());
			Assert.Equal(10, result.Count());
			result = list.Pageing(1, 1);
			Assert.Equal(1, result.FirstOrDefault());
			Assert.Single(result);
			result = list.Pageing(1, 3);
			Assert.Equal(1, result.FirstOrDefault());
			Assert.Equal(3, result.Count());
			result = list.Pageing(1, 20);
			Assert.Equal(1, result.FirstOrDefault());
			Assert.Equal(10, result.Count());

			result = list.Pageing(2, -1);
			Assert.Equal(1, result.FirstOrDefault());
			Assert.Equal(10, result.Count());
			result = list.Pageing(2, 0);
			Assert.Equal(1, result.FirstOrDefault());
			Assert.Equal(10, result.Count());
			result = list.Pageing(2, 1);
			Assert.Equal(2, result.FirstOrDefault());
			Assert.Single(result);
			result = list.Pageing(2, 3);
			Assert.Equal(4, result.FirstOrDefault());
			Assert.Equal(3, result.Count());
			result = list.Pageing(2, 20);
			Assert.Equal(default, result.FirstOrDefault());
			Assert.Empty(result);

			result = list.Pageing(20, -1);
			Assert.Equal(1, result.FirstOrDefault());
			Assert.Equal(10, result.Count());
			result = list.Pageing(20, 0);
			Assert.Equal(1, result.FirstOrDefault());
			Assert.Equal(10, result.Count());
			result = list.Pageing(20, 1);
			Assert.Equal(default, result.FirstOrDefault());
			Assert.Empty(result);
			result = list.Pageing(20, 3);
			Assert.Equal(default, result.FirstOrDefault());
			Assert.Empty(result);
			result = list.Pageing(20, 20);
			Assert.Equal(default, result.FirstOrDefault());
			Assert.Empty(result);

		}
		[Fact]
		public void Belong()
		{
			List<int> small = new List<int>() { 1, 2, 3 };
			List<int> big1 = new List<int>() { 1, 2, 3, 4, 5 };
			List<int> big2 = new List<int>() { 2, 3, 4, 5 };
			Assert.True(small.Belong(big1));
			Assert.True(small.Belong(big1, false));
			Assert.False(small.Belong(big2));
			Assert.False(small.Belong(big2, false));
			Assert.True(small.Belong(small));
			Assert.False(small.Belong(small, false));
		}
#if NET7_0_OR_GREATER
		[Fact]
		public void Median()
		{
			Assert.Equal(1, new[] { 1 }.Shuffle().Median());
			Assert.Equal(1.5, new[] { 1, 2 }.Shuffle().Median());
			Assert.Equal(2, new[] { 1, 2, 3 }.Shuffle().Median());
			Assert.Equal(2.5, new[] { 1, 2, 3, 4 }.Shuffle().Median());
			Assert.Equal(1.1, new[] { 1.1 }.Shuffle().Median());
			Assert.Equal(1.15, new[] { 1.1, 1.2 }.Shuffle().Median());
			Assert.Equal(1.2, new[] { 1.1, 1.2, 1.3 }.Shuffle().Median());
			Assert.Equal(1.25, new[] { 1.1, 1.2, 1.3, 1.4 }.Shuffle().Median());
		}
		[Fact]
		public void Mode()
		{
			Assert.Throws<InvalidOperationException>(() => Array.Empty<int>().Mode());
			Assert.Equal(new[] { 1 }.Order(), new[] { 1 }.Shuffle().Mode().Order());
			Assert.Equal(new[] { 1, 2 }.Order(), new[] { 1, 2 }.Shuffle().Mode().Order());
			Assert.Equal(new[] { 2 }.Order(), new[] { 1, 2, 2 }.Shuffle().Mode().Order());
		}
#endif
	}
}
