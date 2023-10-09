using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace ExtensionMethods
{
	/// <summary>
	/// IEnumerable扩展
	/// </summary>
	public static class IEnumerableExtension
	{
		/// <summary>
		/// 符合条件之后再查询
		/// </summary>
		/// <inheritdoc cref="System.Linq.Enumerable.Where{TSource}(System.Collections.Generic.IEnumerable{TSource}, System.Func{TSource, bool})"/>
		/// <typeparam name="TSource"></typeparam>
		/// <param name="sources">源对象</param>
		/// <param name="condition">条件</param>
		/// <param name="expression">查询表达式</param>
		/// <returns></returns>
		public static System.Collections.Generic.IEnumerable<TSource> Where<TSource>(this System.Collections.Generic.IEnumerable<TSource> sources,
			bool condition,
			System.Func<TSource, bool> expression) => condition ? sources.Where(expression) : sources;

		/// <summary>
		/// 左外连接
		/// </summary>
		/// <inheritdoc cref="Enumerable.Join{TOuter, TInner, TKey, TResult}(System.Collections.Generic.IEnumerable{TOuter}, System.Collections.Generic.IEnumerable{TInner}, System.Func{TOuter, TKey}, System.Func{TInner, TKey}, System.Func{TOuter, TInner, TResult})"/>
		/// <returns></returns>
		public static System.Collections.Generic.IEnumerable<TResult> LeftJoin<TOuter, TInner, TKey, TResult>(this System.Collections.Generic.IEnumerable<TOuter> outer,
			System.Collections.Generic.IEnumerable<TInner> inner,
			System.Func<TOuter, TKey> outerKeySelector,
			System.Func<TInner, TKey> innerKeySelector,
#if NET5_0_OR_GREATER
			System.Func<TOuter, TInner?, TResult> resultSelector)
#else
			System.Func<TOuter, TInner, TResult> resultSelector)
#endif
			=> outer.GroupJoin(inner, outerKeySelector, innerKeySelector, (x, y) => new { x, y }).SelectMany(a => a.y.DefaultIfEmpty(), (a, b) => resultSelector(a.x, b));
		/// <summary>
		/// 分页 页码&lt;=0时返回第一页 页尺寸&lt;=0时不进行分页
		/// </summary>
		/// <typeparam name="TSource">The type of the elements of source.</typeparam>
		/// <param name="source">An System.Collections.Generic.IEnumerable`1 to return elements from.</param>
		/// <param name="page">The page of return, return first page when page less 1</param>
		/// <param name="pageSize">pageSize</param>
		/// <returns></returns>
		public static System.Collections.Generic.IEnumerable<TSource> Pageing<TSource>(this System.Collections.Generic.IEnumerable<TSource> source,
			int page,
			int pageSize) => pageSize < 1 ? source : source.Skip(pageSize * (page < 0 ? 0 : page - 1)).Take(pageSize);

		/// <summary>
		/// 集合属于另一个集合
		/// </summary>
		/// <typeparam name="TSource"></typeparam>
		/// <param name="smallSet">小的集合</param>
		/// <param name="bigSet">大的集合</param>
		/// <param name="canEqual">是否可以两个集合相同</param>
		/// <returns></returns>
		public static bool Belong<TSource>(this System.Collections.Generic.IEnumerable<TSource> smallSet,
			System.Collections.Generic.IEnumerable<TSource> bigSet,
			bool canEqual = true) => smallSet.All(x => bigSet.Contains(x)) && (canEqual || bigSet.Any(x => !smallSet.Contains(x)));
#if NET7_0_OR_GREATER
		/// <summary>
		/// 取中位数 如果数量为偶数则为中间两个数的平均
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source"></param>
		/// <returns></returns>
		/// <exception cref="NotSupportedException"></exception>
		public static double Median<T>(this IEnumerable<T> source) where T : System.Numerics.INumber<T>, IConvertible
		{
			var count = source.TryGetNonEnumeratedCount(out int c) ? c : source.Count();
			if (count == 0) return 0;
			if (count == 1) return Convert.ToDouble(source.First());
			var order = source.Order();
			return (count % 2) switch
			{
				0 => (Convert.ToDouble(order.Skip(count / 2 - 1).Take(2).First()) + Convert.ToDouble(order.Skip(count / 2).Take(2).First())) / 2,
				1 => Convert.ToDouble(order.Skip(count / 2).Take(1).First()),
				_ => throw new NotSupportedException()
			};
		}
		/// <summary>
		/// 取众数
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source"></param>
		/// <returns></returns>
		/// <exception cref="InvalidOperationException"></exception>
		public static List<T> Mode<T>(this IEnumerable<T> source) where T : INumber<T>
		{
			var count = source.TryGetNonEnumeratedCount(out int c) ? c : source.Count();
			if (count == 0) throw new InvalidOperationException("序列不包含任何元素");
			if (count == 1) return new List<T> { source.First() };
			var groups = source.GroupBy(x => x).Select(x => new { key = x.Key, count = x.Count() });
			var maxCount = groups.Max(x => x.count);
			return groups.Where(x => x.count == maxCount).Select(x => x.key).ToList();
		}
#endif
	}
}
