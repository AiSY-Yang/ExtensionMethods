using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExtensionMethods
{
	/// <summary>
	/// This is a Check for special case
	/// </summary>
	static class Private
	{
		public static IQueryable<TSource> Skip<TSource>(this IQueryable<TSource> source, int skip) => skip < 0 ? throw new ArgumentException("For IQueryable skip can't less than 0") : source.Skip(skip);
	}
}
