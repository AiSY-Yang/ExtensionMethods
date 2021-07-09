using Microsoft.AspNetCore.Mvc;
namespace ExtensionMethods.AspNetCore
{
	/// <summary>
	/// 控制器扩展
	/// </summary>
	public static class ControllerBaseExtension
	{
		/// <summary>
		/// 得到GET请求中的指定查询字符串 无结果返回null
		/// </summary>
		/// <param name="controller"></param>
		/// <param name="str">key</param>
		/// <returns></returns>
		public static string GetQueryString(this ControllerBase controller, string str)
		{
			return controller.HttpContext.Request.QueryString.GetValue(str);
		}
		/// <summary>
		/// 得到GET请求中的指定查询字符串并转化为指定类型 无结果返回default(T)
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="controller"></param>
		/// <param name="str"></param>
		/// <returns></returns>
		public static T GetQueryString<T>(this ControllerBase controller, string str)
		{
			string result = controller.HttpContext.Request.QueryString.GetValue(str);
			if (result == null)
			{
				return default;
			}
			else
			{
				return result.Convert<T>();
			}
		}
	}
}
