using System;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;

namespace ExtensionMethods
{
	/// <summary>
	/// IQueryable扩展
	/// </summary>
	public static class IQueryableExtension
	{
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
	}
}
