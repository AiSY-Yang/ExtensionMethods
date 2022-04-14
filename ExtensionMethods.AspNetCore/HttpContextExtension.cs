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
		/// example:采用Nginx且转发所有X-Forwarded-For的情况下,first()为客户端IP,last()为网关IP
		/// 信任所有X-Forwarded-For中的信息
		/// </summary>
		/// <returns></returns>
		public static List<string> GetIpAddress(this HttpContext httpContext)
		{
			List<string> iPAddresses = new List<string>();
			if (httpContext.Request.Headers.ContainsKey("X-Forwarded-For"))
			{
				foreach (var header in httpContext.Request.Headers["X-Forwarded-For"])
				{
					var l = header.Split(',', System.StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim());
					foreach (var item in l)
					{
						iPAddresses.Add(item);
					}
				}
			}
			iPAddresses.Add(httpContext.Connection.RemoteIpAddress.ToString());
			return iPAddresses;
		}
		/// <summary>
		/// 返回UA
		/// </summary>
		/// <param name="httpContext"></param>
		/// <returns></returns>
		public static string GetUserAgent(this HttpContext httpContext) => httpContext.Request.Headers["User-Agent"][0];
	}
}
