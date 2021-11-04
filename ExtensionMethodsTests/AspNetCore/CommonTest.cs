using ExtensionMethods;

using Xunit;

namespace ExtensionMethodsTests
{
	public class CommonTest
	{
		[Fact]
		public void GetMIMETest()
		{
			Assert.Equal("application/octet-stream", Tools.GetMIME("aaa"));
			Assert.Equal("application/postscript", Tools.GetMIME("ai"));
		}
	}
}
