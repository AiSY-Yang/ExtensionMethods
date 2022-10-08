using System;
using System.Linq;

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
			System.Func<TOuter, TInner, TResult> resultSelector)
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

		/// <summary>
		/// 两个IEnumerable对象内的成员相同(包括顺序与值)
		/// </summary>
		/// <param name="obj1"></param>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static bool ContentEquals(this System.Collections.IEnumerable obj1, System.Collections.IEnumerable obj)
		{
			var x = obj1.GetEnumerator();
			var y = obj.GetEnumerator();
			while (true)
			{
				bool hasNextX = x.MoveNext();
				bool hasNextY = y.MoveNext();
				if (!hasNextX || !hasNextY)
					return hasNextX == hasNextY;
				System.Type typeX = x.Current.GetType();
				System.Type typeY = y.Current.GetType();
				if (!typeX.Equals(typeY))
					return false;
				if (typeX is System.Collections.IEnumerable xx && typeY is System.Collections.IEnumerable yy)
					if (!ContentEquals(xx, yy))
						return false;
				if (!x.Current.Equals(y.Current))
					return false;
			}
		}
	}
}
