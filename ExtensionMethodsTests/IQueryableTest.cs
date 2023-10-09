using ExtensionMethods;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using Xunit;

namespace ExtensionMethodsTests
{
	public class IQueryableTest
	{
		IQueryable<KeyValuePair<int, string>> dicForOrder = new Dictionary<int, string>() { { 1, "C" }, { 3, "A" }, { 2, "B" } }.AsQueryable();
		IQueryable<int> listForWhere = new List<int>() { 2, 1, 3, 4, 5 }.AsQueryable();
		IQueryable<int> listForPageing = new List<int>() { 2, 1, 3, 4, 5, 6, 7 }.AsQueryable();

		[Fact]
		public void AutoOrder()
		{
			var result = dicForOrder.AutoOrder(null, "Key").ToList();
			Assert.Equal(3, result.Count);
			Assert.Equal(1, result.First().Key);
			Assert.Equal(2, result.Last().Key);

			result = dicForOrder.AutoOrder("", "Key").ToList();
			Assert.Equal(3, result.Count);
			Assert.Equal(1, result.First().Key);
			Assert.Equal(2, result.Last().Key);
		}

		[Theory]
		[InlineData("asc")]
		[InlineData("ASC")]
		[InlineData("ascending")]
		public void AutoOrderAsc(string order)
		{
			var result = dicForOrder.AutoOrder(order, "Key").ToList();
			Assert.Equal(3, result.Count);
			Assert.Equal(1, result.First().Key);
			Assert.Equal(3, result.Last().Key);
			result = dicForOrder.AutoOrder(order, "Value").ToList();
			Assert.Equal(3, result.Count);
			Assert.Equal(3, result.First().Key);
			Assert.Equal(1, result.Last().Key);

			Assert.Throws<ArgumentException>(() => dicForOrder.AutoOrder(order, "").ToList());
			Assert.Throws<ArgumentException>(() => dicForOrder.AutoOrder(order, "AA").ToList());
		}

		[Theory]
		[InlineData("desc")]
		[InlineData("DESC")]
		[InlineData("descending")]
		public void AutoOrderDesc(string order)
		{
			var result = dicForOrder.AutoOrder(order, "Key").ToList();
			Assert.Equal(3, result.Count);
			Assert.Equal(3, result.First().Key);
			Assert.Equal(1, result.Last().Key);
			result = dicForOrder.AutoOrder(order, "Value").ToList();
			Assert.Equal(3, result.Count);
			Assert.Equal(1, result.First().Key);
			Assert.Equal(3, result.Last().Key);
			Assert.Throws<ArgumentException>(() => dicForOrder.AutoOrder(order, "").ToList());
			Assert.Throws<ArgumentException>(() => dicForOrder.AutoOrder(order, "AA").ToList());
		}

		[Fact]
		public void OrderBy()
		{
			var result = dicForOrder.OrderBy("Key").ToList();
			Assert.Equal(3, result.Count);
			Assert.Equal(1, result.First().Key);
			Assert.Equal(3, result.Last().Key);
			result = dicForOrder.OrderBy("Value").ToList();
			Assert.Equal(3, result.Count);
			Assert.Equal(3, result.First().Key);
			Assert.Equal(1, result.Last().Key);
			Assert.Throws<ArgumentException>(() => dicForOrder.OrderBy("").ToList());
			Assert.Throws<ArgumentException>(() => dicForOrder.OrderBy("AA").ToList());
		}
		[Fact]
		public void OrderByDescending()
		{
			var result = dicForOrder.OrderByDescending("Key").ToList();
			Assert.Equal(3, result.Count);
			Assert.Equal(3, result.First().Key);
			Assert.Equal(1, result.Last().Key);
			result = dicForOrder.OrderByDescending("Value").ToList();
			Assert.Equal(3, result.Count);
			Assert.Equal(1, result.First().Key);
			Assert.Equal(3, result.Last().Key);
			Assert.Throws<ArgumentException>(() => dicForOrder.OrderByDescending("").ToList());
			Assert.Throws<ArgumentException>(() => dicForOrder.OrderByDescending("AA").ToList());
		}
		[Fact]
		public void Where()
		{
			var result = listForWhere.Where(true, x => x % 2 == 0).ToList();
			Assert.Equal(2, result.Count);
			Assert.Equal(2, result.First());
			Assert.Equal(4, result.Last());
			result = listForWhere.Where(false, x => x % 2 == 0).ToList();
			Assert.Equal(5, result.Count);
			Assert.Equal(2, result.First());
			Assert.Equal(5, result.Last());
		}
		[Fact]
		public void WhereOr()
		{
			var expression = new List<Expression<Func<int, bool>>>();
			var result = listForWhere.WhereOr(expression).ToList();
			Assert.True(result.SequenceEqual(listForWhere));
			expression.Add(x => x == 2);
			result = listForWhere.Where(false, x => x % 2 == 0).ToList();
			Assert.Equal(5, result.Count);
			Assert.Equal(2, result.First());
			Assert.Equal(5, result.Last());
		}
		[Fact]
		public void Pageing()
		{
			//每页尺寸小于等于0不分页
			Assert.Equal(7, listForPageing.Pageing(-1, -1).Count());
			Assert.Equal(7, listForPageing.Pageing(-1, 0).Count());
			Assert.Equal(7, listForPageing.Pageing(0, -1).Count());
			Assert.Equal(7, listForPageing.Pageing(0, 0).Count());
			Assert.Equal(7, listForPageing.Pageing(1, -1).Count());
			Assert.Equal(7, listForPageing.Pageing(1, 0).Count());

			//页号小于等于0返回第一页
			Assert.Equal(1, listForPageing.Pageing(-1, 1).Count());
			Assert.Equal(1, listForPageing.Pageing(0, 1).Count());
			Assert.Equal(1, listForPageing.Pageing(1, 1).Count());
			Assert.Equal(2, listForPageing.Pageing(-1, 2).Count());
			Assert.Equal(2, listForPageing.Pageing(0, 2).Count());
			Assert.Equal(2, listForPageing.Pageing(1, 2).Count());

			//正常分页
			Assert.Equal(1, listForPageing.Pageing(1, 1).Count());
			Assert.Equal(2, listForPageing.Pageing(1, 2).Count());
			Assert.Equal(2, listForPageing.Pageing(3, 2).Count());
			Assert.Equal(1, listForPageing.Pageing(4, 2).Count());
			Assert.Equal(7, listForPageing.Pageing(4, 2).First());
		}
		[Fact]
		public void LeftJoin()
		{
			IQueryable<(int, int)> data1 = new List<(int, int)>() { (1, 2), (2, 3) }.AsQueryable();
			IQueryable<(int, int)> data2 = new List<(int, int)>() { (1, 3), (4, 5) }.AsQueryable();
			var q = data1.LeftJoin(data2, x => x.Item1, x => x.Item1, (x, y) => new { A = x.Item1, B = y.Item2 });
			var res = q.ToList();
			Assert.Equal(2, res.Count);
			Assert.Equal(1, res[0].A);
			Assert.Equal(3, res[0].B);
			Assert.Equal(2, res[1].A);
			Assert.Equal(0, res[1].B);
		}
	}
}
