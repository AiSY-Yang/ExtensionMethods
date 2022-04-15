
using System.Collections.Generic;

using ExtensionMethods;

using Xunit;

namespace ExtensionMethodsTests
{
	public class ClaimsPrincipalTest
	{
		System.Security.Claims.ClaimsPrincipal claimsPrincipal = new System.Security.Claims.ClaimsPrincipal();
		System.Security.Claims.ClaimsPrincipal claimsPrincipal2 = new System.Security.Claims.ClaimsPrincipal();
		public ClaimsPrincipalTest()
		{
			var identity = new System.Security.Claims.ClaimsIdentity("测试");
			identity.AddClaim(new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Role, "测试"));
			claimsPrincipal.AddIdentity(identity);
			var identity2 = new System.Security.Claims.ClaimsIdentity("测试");
			identity2.AddClaim(new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Role, "管理"));
			claimsPrincipal2.AddIdentity(identity2);
		}

		[Fact]
		public void IsInRole()
		{
			IEnumerable<string> allow = new List<string>() { "系统管理员", "管理员" };
			IEnumerable<string> allow2 = new List<string>() { "系统管理员", "管理员", "测试" };
			Assert.False(claimsPrincipal.IsInRole(allow));
			Assert.True(claimsPrincipal.IsInRole(allow2));
			Assert.False(claimsPrincipal.IsInRole("系统管理员", "管理员"));
			Assert.True(claimsPrincipal.IsInRole("系统管理员", "管理员", "测试"));

			Assert.False(claimsPrincipal2.IsInRole(allow));
			Assert.False(claimsPrincipal2.IsInRole(allow2));
			Assert.False(claimsPrincipal2.IsInRole("系统管理员", "管理员"));
			Assert.False(claimsPrincipal2.IsInRole("系统管理员", "管理员", "测试"));

		}
	}
}
