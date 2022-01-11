using System.Linq;

namespace ExtensionMethods
{
	/// <summary>
	/// ClaimsPrincipal扩展函数
	/// </summary>
	public static class ClaimsPrincipalExtension
	{
		/// <summary>
		/// 用户是否包含此权限
		/// </summary>
		/// <param name="claimsPrincipal"></param>
		/// <param name="roles">权限列表</param>
		/// <returns></returns>
		public static bool IsInRole(this System.Security.Claims.ClaimsPrincipal claimsPrincipal, System.Collections.Generic.IEnumerable<string> roles)
		{
			return roles.Any(x => claimsPrincipal.IsInRole(x));
		}
		/// <summary>
		/// 用户是否包含此权限
		/// </summary>
		/// <param name="claimsPrincipal"></param>
		/// <param name="roles">权限列表</param>
		/// <returns></returns>
		public static bool IsInRole(this System.Security.Claims.ClaimsPrincipal claimsPrincipal, params string[] roles)
		{
			return roles.Any(x => claimsPrincipal.IsInRole(x));
		}
	}
}