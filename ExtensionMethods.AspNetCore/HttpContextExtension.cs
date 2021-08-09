using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace ExtensionMethods.AspNetCore
{
	/// <summary>
	/// HttpContext扩展
	/// </summary>
	public static class HttpcontextExtension
	{
		/// <summary>
		/// 返回IP列表,依次是是真实IP,第一层代理,第二层代理
		/// 信任所有X-Forwarded-For中的信息
		/// </summary>
		/// <returns></returns>
		public static List<string> GetIpAddress(this HttpContext httpContext)
		{
			List<string> iPAddresses = new List<string>();
			if (httpContext.Request.Headers.ContainsKey("X-Forwarded-For"))
			{
				var l = httpContext.Request.Headers["X-Forwarded-For"][0].Split(',', System.StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim());
				foreach (var item in l)
				{
					iPAddresses.Add(item);
				}
			}
			iPAddresses.Add(httpContext.Connection.RemoteIpAddress.ToString());
			return iPAddresses;
		}
	}
}
