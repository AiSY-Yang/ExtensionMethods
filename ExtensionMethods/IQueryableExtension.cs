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
		/// 按照指定字段和指定的方法进行排序,如果排序方法不匹配则不执行排序
		/// </summary>
		/// <typeparam name="TSource"></typeparam>
		/// <param name="source"></param>
		/// <param name="sortMethod">排序方式</param>
		/// <param name="sortField">排序字段名</param>
		/// <returns></returns>
		/// <exception cref="ArgumentException">排序字段为空或不存在排序字段</exception>
		public static IQueryable<TSource> AutoOrder<TSource>(this IQueryable<TSource> source, string sortMethod, string sortField) => sortMethod?.ToLower() switch
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
			PropertyInfo sortProperty = typeof(T).GetProperty(sortField, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase) ?? throw new ArgumentException($"查询对象中不存在排序字段{sortField}！");
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
			PropertyInfo sortProperty = typeof(T).GetProperty(sortField, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase) ?? throw new ArgumentException($"查询对象中不存在排序字段{sortField}！");
			ParameterExpression param = Expression.Parameter(typeof(T));
			var body = Expression.MakeMemberAccess(param, sortProperty);
			LambdaExpression keySelectorLambda = Expression.Lambda(body, param);
			return (IOrderedQueryable<T>)query.Provider.CreateQuery<T>(Expression.Call(typeof(Queryable), "OrderByDescending", new Type[] { typeof(T), body.Type }, query.Expression, Expression.Quote(keySelectorLambda)));
		}
		/// <summary>
		/// Implement Left Outer join implemented by calling GroupJoin and
		/// SelectMany within this extension method
		/// <br></br><a href="https://stackoverflow.com/questions/46537158/trying-to-implement-a-leftjoin-extension-method-to-work-with-ef-core-2-0/47357102#47357102"></a>
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
						resultSelector.Invoke(joinResult.outerItem, innerItem!));
		}

		/// <summary>
		/// 分页 页码&lt;=0时返回第一页 页尺寸&lt;=0时不进行分页
		/// </summary>
		/// <typeparam name="TSource">The type of the elements of source.</typeparam>
		/// <param name="source">An System.Collections.Generic.IEnumerable`1 to return elements from.</param>
		/// <param name="page">The page of return, return first page when page less 1</param>
		/// <param name="pageSize">pageSize</param>
		/// <returns></returns>
		public static IQueryable<TSource> Pageing<TSource>(this IQueryable<TSource> source, int page, int pageSize) => pageSize < 1 ? source : source.Skip(pageSize * (page <= 0 ? 0 : page - 1)).Take(pageSize);

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
		/// 按照Or条件组合多个表达式
		/// </summary>
		/// <typeparam name="TSource"></typeparam>
		/// <param name="query"></param>
		/// <param name="conditions"></param>
		/// <returns></returns>
		public static IQueryable<TSource> WhereOr<TSource>(this IQueryable<TSource> query, IEnumerable<Expression<Func<TSource, bool>>> conditions)
		{
#if NET6_0_OR_GREATER
			var count = conditions.TryGetNonEnumeratedCount(out var c) ? c : conditions.Count();
#else
			var count = conditions.Count();
#endif
			switch (count)
			{
				case 0:
					return query;
				case 1:
					return query.Where(conditions.First());
				default:
					Expression<Func<TSource, bool>> expression = (TSource u) => false;
					foreach (var item in conditions)
					{
						var parameterExpressionSetter = expression.Parameters
			.Select((u, i) => new { u, Parameter = item.Parameters[i] })
			.ToDictionary(d => d.Parameter, d => d.u);
						var extendExpressionBody = new ParameterReplaceExpressionVisitor(parameterExpressionSetter).Visit(item.Body);
						expression = Expression.Lambda<Func<TSource, bool>>(Expression.OrElse(expression.Body, extendExpressionBody), expression.Parameters);
					}
					return query.Where(expression);
			}
		}
	}

	/// <summary>
	/// 处理 Lambda 参数不一致问题
	/// </summary>
	internal sealed class ParameterReplaceExpressionVisitor : System.Linq.Expressions.ExpressionVisitor
	{
		/// <summary>
		/// 参数表达式映射集合
		/// </summary>
		private readonly Dictionary<ParameterExpression, ParameterExpression> parameterExpressionSetter;

		/// <summary>
		/// 构造函数
		/// </summary>
		/// <param name="parameterExpressionSetter">参数表达式映射集合</param>
		public ParameterReplaceExpressionVisitor(Dictionary<ParameterExpression, ParameterExpression> parameterExpressionSetter)
		{
			this.parameterExpressionSetter = parameterExpressionSetter ?? new Dictionary<ParameterExpression, ParameterExpression>();
		}

		/// <summary>
		/// 替换表达式参数
		/// </summary>
		/// <param name="parameterExpressionSetter">参数表达式映射集合</param>
		/// <param name="expression">表达式</param>
		/// <returns>新的表达式</returns>
		public static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression> parameterExpressionSetter, Expression expression)
		{
			return new ParameterReplaceExpressionVisitor(parameterExpressionSetter).Visit(expression);
		}

		/// <summary>
		/// 重写基类参数访问器
		/// </summary>
		/// <param name="parameterExpression"></param>
		/// <returns></returns>
		protected override Expression VisitParameter(ParameterExpression parameterExpression)
		{
			if (parameterExpressionSetter.TryGetValue(parameterExpression, out var replacement))
			{
				parameterExpression = replacement;
			}

			return base.VisitParameter(parameterExpression);
		}
	}

}