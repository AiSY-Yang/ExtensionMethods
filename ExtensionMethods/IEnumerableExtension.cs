using System;
using System.Collections.Generic;
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
				if (typeof(System.Collections.IEnumerable).IsAssignableFrom(typeX))
					if (!ContentEquals(x.Current as System.Collections.IEnumerable, y.Current as System.Collections.IEnumerable))
						return false;
				if (!x.Current.Equals(y.Current))
					return false;
			}
		}
		/// <summary>
		/// Performs a full outer join on two heterogeneous sequences.
		/// Additional arguments specify key selection functions, result
		/// projection functions and a key comparer.
		/// <br/><a href="https://github.com/morelinq/MoreLINQ/blob/master/MoreLinq/FullJoin.cs"></a>
		/// </summary>
		/// <typeparam name="TFirst">
		/// The type of elements in the first sequence.</typeparam>
		/// <typeparam name="TSecond">
		/// The type of elements in the second sequence.</typeparam>
		/// <typeparam name="TKey">
		/// The type of the key returned by the key selector functions.</typeparam>
		/// <typeparam name="TResult">
		/// The type of the result elements.</typeparam>
		/// <param name="first">
		/// The first sequence to join fully.</param>
		/// <param name="second">
		/// The second sequence to join fully.</param>
		/// <param name="firstKeySelector">
		/// Function that projects the key given an element from <paramref name="first"/>.</param>
		/// <param name="secondKeySelector">
		/// Function that projects the key given an element from <paramref name="second"/>.</param>
		/// <param name="firstSelector">
		/// Function that projects the result given just an element from
		/// <paramref name="first"/> where there is no corresponding element
		/// in <paramref name="second"/>.</param>
		/// <param name="secondSelector">
		/// Function that projects the result given just an element from
		/// <paramref name="second"/> where there is no corresponding element
		/// in <paramref name="first"/>.</param>
		/// <param name="bothSelector">
		/// Function that projects the result given an element from
		/// <paramref name="first"/> and an element from <paramref name="second"/>
		/// that match on a common key.</param>
		/// <param name="comparer">
		/// The <see cref="IEqualityComparer{T}"/> instance used to compare
		/// keys for equality.</param>
		/// <returns>A sequence containing results projected from a full
		/// outer join of the two input sequences.</returns>

		public static IEnumerable<TResult> FullJoin<TFirst, TSecond, TKey, TResult>(
			this IEnumerable<TFirst> first,
			IEnumerable<TSecond> second,
			Func<TFirst, TKey> firstKeySelector,
			Func<TSecond, TKey> secondKeySelector,
			Func<TFirst, TResult> firstSelector,
			Func<TSecond, TResult> secondSelector,
			Func<TFirst, TSecond, TResult> bothSelector,
			IEqualityComparer<TKey>? comparer)
		{
			if (first == null) throw new ArgumentNullException(nameof(first));
			if (second == null) throw new ArgumentNullException(nameof(second));
			if (firstKeySelector == null) throw new ArgumentNullException(nameof(firstKeySelector));
			if (secondKeySelector == null) throw new ArgumentNullException(nameof(secondKeySelector));
			if (firstSelector == null) throw new ArgumentNullException(nameof(firstSelector));
			if (secondSelector == null) throw new ArgumentNullException(nameof(secondSelector));
			if (bothSelector == null) throw new ArgumentNullException(nameof(bothSelector));

			var seconds = second.Select(e => new KeyValuePair<TKey, TSecond>(secondKeySelector(e), e)).ToArray();
			var secondLookup = seconds.ToLookup(e => e.Key, e => e.Value, comparer);
			var firstKeys = new HashSet<TKey>(comparer);

			foreach (var fe in first)
			{
				var key = firstKeySelector(fe);
				firstKeys.Add(key);

				using var se = secondLookup[key].GetEnumerator();

				if (se.MoveNext())
				{
					do { yield return bothSelector(fe, se.Current); }
					while (se.MoveNext());
				}
				else
				{
					se.Dispose();
					yield return firstSelector(fe);
				}
			}

			foreach (var se in seconds)
			{
				if (!firstKeys.Contains(se.Key))
					yield return secondSelector(se.Value);
			}
		}
	}
}
