﻿using System;
using System.Linq;

namespace ExtensionMethods
{
	/// <summary>
	/// IEnumerable扩展
	/// </summary>
	public static class IEnumerableExtension
	{
		/// <summary>
		/// 左外连接
		/// </summary>
		/// <inheritdoc cref="System.Linq.Enumerable.Join{TOuter, TInner, TKey, TResult}(System.Collections.Generic.IEnumerable{TOuter}, System.Collections.Generic.IEnumerable{TInner}, Func{TOuter, TKey}, Func{TInner, TKey}, Func{TOuter, TInner, TResult})"/>
		/// <returns></returns>
		public static System.Collections.Generic.IEnumerable<TResult> LeftJoin<TOuter, TInner, TKey, TResult>(this System.Collections.Generic.IEnumerable<TOuter> outer, System.Collections.Generic.IEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, TInner, TResult> resultSelector)
		{
			return outer.GroupJoin(inner, outerKeySelector, innerKeySelector, (x, y) => new { x, y }).SelectMany(a => a.y.DefaultIfEmpty(), (a, b) => resultSelector(a.x, b));
		}
		/// <summary>
		/// 分页 页码&lt;=0时返回第一页 页尺寸&lt;=0时不进行分页
		/// </summary>
		/// <typeparam name="TSource">The type of the elements of source.</typeparam>
		/// <param name="source">An System.Collections.Generic.IEnumerable`1 to return elements from.</param>
		/// <param name="page">The page of return, return first page when page less 1</param>
		/// <param name="pageSize">pageSize</param>
		/// <returns></returns>
		public static System.Collections.Generic.IEnumerable<TSource> Pageing<TSource>(this System.Collections.Generic.IEnumerable<TSource> source, int page, int pageSize)
		{
			return pageSize < 1 ? source : source.Skip(pageSize * (page < 0 ? 0 : page - 1)).Take(pageSize);
		}
		/// <summary>
		/// 集合属于另一个集合
		/// </summary>
		/// <typeparam name="TSource"></typeparam>
		/// <param name="smallSet">小的集合</param>
		/// <param name="bigSet">大的集合</param>
		/// <param name="canEqual">是否可以两个集合相同</param>
		/// <returns></returns>
		public static bool Belong<TSource>(this System.Collections.Generic.IEnumerable<TSource> smallSet, System.Collections.Generic.IEnumerable<TSource> bigSet, bool canEqual = true)
		{
			return smallSet.All(x => bigSet.Contains(x)) && (canEqual || (!canEqual && bigSet.Any(x => !smallSet.Contains(x))));
		}
	}
}