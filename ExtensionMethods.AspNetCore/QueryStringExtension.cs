namespace ExtensionMethods.AspNetCore
{
	/// <summary>
	/// 查询字符串扩展函数
	/// </summary>
	public static class QueryStringExtend
	{
		/// <summary>
		/// 得到指定的参数 不区分大小写
		/// </summary>
		/// <param name="queryString"></param>
		/// <param name="key"></param>
		/// <returns>无此参数时返回null</returns>
		public static string GetValue(this Microsoft.AspNetCore.Http.QueryString queryString, string key)
		{
			try
			{
				string s = queryString.Value;
				s = s.TrimStart("?");
				string[] ss = s.Split('&');
				foreach (var item in ss)
				{
					if (item.Split('=')[0].ToLower() == key.ToLower())
					{
						return item.Split('=')[1];
					}
				}
			}
			catch
			{
				return null;
			}
			return null;
		}
	}
}
