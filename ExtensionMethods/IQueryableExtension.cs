using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using LinqKit;
using LinqKit.Core;

namespace ExtensionMethods
{
	/// <summary>
	/// IQueryable扩展
	/// </summary>
	public static class IQueryableExtension
	{
		/// <summary>
		/// 符合条件之后再查询
		/// </summary>
		/// <inheritdoc cref="System.Linq.Queryable.Where{TSource}(IQueryable{TSource}, System.Linq.Expressions.Expression{Func{TSource, bool}})"/>
		/// <typeparam name="TSource"></typeparam>
		/// <param name="sources">源对象</param>
		/// <param name="condition">条件</param>
		/// <param name="expression">查询表达式</param>
		/// <returns></returns>
		public static IQueryable<TSource> Where<TSource>(this IQueryable<TSource> sources, bool condition, System.Linq.Expressions.Expression<Func<TSource, bool>> expression) => condition ? sources.Where(expression) : sources;

		/// <summary>
		/// 分页 页码&lt;=0时返回第一页 页尺寸&lt;=0时不进行分页
		/// </summary>
		/// <typeparam name="TSource">The type of the elements of source.</typeparam>
		/// <param name="source">An System.Collections.Generic.IEnumerable`1 to return elements from.</param>
		/// <param name="page">The page of return, return first page when page less 1</param>
		/// <param name="pageSize">pageSize</param>
		/// <returns></returns>
		public static IQueryable<TSource> Pageing<TSource>(this IQueryable<TSource> source, int page, int pageSize) => pageSize < 1 ? source : source.Skip(pageSize * (page < 0 ? 0 : page - 1)).Take(pageSize);

		/// <summary>
		/// 按照指定字段和指定的方法进行排序,如果排序方法不匹配则不执行排序
		/// </summary>
		/// <typeparam name="TSource"></typeparam>
		/// <param name="source"></param>
		/// <param name="sortMethod">排序方式</param>
		/// <param name="sortField">排序字段名</param>
		/// <returns></returns>
		/// <exception cref="ArgumentException">排序字段为空或不存在排序字段</exception>
		public static IQueryable<TSource> AutoOrder<TSource>(this IQueryable<TSource> source, string sortMethod, string sortField) => sortMethod.IsNullOrWhiteSpace() ? source : sortMethod.ToLower() switch
		{
			"asc" => source.OrderBy(sortField),
			"desc" => source.OrderByDescending(sortField),
			"ascending" => source.OrderBy(sortField),
			"descending" => source.OrderByDescending(sortField),
			_ => source,
		};
		/// <summary>
		/// 按指定字段升序排列
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="query"></param>
		/// <param name="sortField"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentException">排序字段为空或不存在排序字段</exception>
		public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> query, string sortField)
		{
			if (string.IsNullOrEmpty(sortField))
				throw new ArgumentException("排序字段为空!");
			PropertyInfo sortProperty = typeof(T).GetProperty(sortField, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
			if (sortProperty == null)
				throw new ArgumentException($"查询对象中不存在排序字段{ sortField }！");
			ParameterExpression param = Expression.Parameter(typeof(T));
			var body = Expression.MakeMemberAccess(param, sortProperty);
			LambdaExpression keySelectorLambda = Expression.Lambda(body, param);
			return (IOrderedQueryable<T>)query.Provider.CreateQuery<T>(Expression.Call(typeof(Queryable), "OrderBy", new Type[] { typeof(T), body.Type }, query.Expression, Expression.Quote(keySelectorLambda)));
		}

		/// <summary>
		/// 按指定字段降序排列
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="query"></param>
		/// <param name="sortField"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentException">排序字段为空或不存在排序字段</exception>
		public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> query, string sortField)
		{
			if (string.IsNullOrEmpty(sortField))
				throw new ArgumentException("排序字段为空!");
			PropertyInfo sortProperty = typeof(T).GetProperty(sortField, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
			if (sortProperty == null)
				throw new ArgumentException($"查询对象中不存在排序字段{ sortField }！");
			ParameterExpression param = Expression.Parameter(typeof(T));
			var body = Expression.MakeMemberAccess(param, sortProperty);
			LambdaExpression keySelectorLambda = Expression.Lambda(body, param);
			return (IOrderedQueryable<T>)query.Provider.CreateQuery<T>(Expression.Call(typeof(Queryable), "OrderByDescending", new Type[] { typeof(T), body.Type }, query.Expression, Expression.Quote(keySelectorLambda)));
		}
		/// <summary>
		/// Implement Left Outer join implemented by calling GroupJoin and
		/// SelectMany within this extension method
		/// </summary>
		/// <typeparam name="TOuter">Outer Type</typeparam>
		/// <typeparam name="TInner">Inner Type</typeparam>
		/// <typeparam name="TKey">Key Type</typeparam>
		/// <typeparam name="TResult">Result Type</typeparam>
		/// <param name="outer">Outer set</param>
		/// <param name="inner">Inner set</param>
		/// <param name="outerKeySelector">Outer Key Selector</param>
		/// <param name="innerKeySelector">Inner Key Selector</param>
		/// <param name="resultSelector">Result Selector</param>
		/// <returns>IQueryable Result set</returns>
		public static IQueryable<TResult> LeftJoin<TOuter, TInner, TKey, TResult>(
		   this IQueryable<TOuter> outer,
		   IQueryable<TInner> inner,
		   Expression<Func<TOuter, TKey>> outerKeySelector,
		   Expression<Func<TInner, TKey>> innerKeySelector,
		   Expression<Func<TOuter, TInner, TResult>> resultSelector)
		{
			//LinqKit allows easy runtime evaluation of inline invoked expressions
			// without manually writing expression trees.
			return outer
				.AsExpandable()// Tell LinqKit to convert everything into an expression tree.
				.GroupJoin(
					inner,
					outerKeySelector,
					innerKeySelector,
					(outerItem, innerItems) => new { outerItem, innerItems })
				.SelectMany(
					joinResult => joinResult.innerItems.DefaultIfEmpty(),
					(joinResult, innerItem) =>
						resultSelector.Invoke(joinResult.outerItem, innerItem));
		}
	}
}
