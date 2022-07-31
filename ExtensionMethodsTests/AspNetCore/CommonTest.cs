using ExtensionMethods.Common;

using Xunit;

namespace ExtensionMethodsTests
{
	public class CommonTest
	{
		[Fact]
		public void GetMIMETest()
		{
			using var s = System.IO.File.Create("./test.md");
			s.Close();
			Assert.Equal("text/markdown", Tools.GetMIME("./test.md"));
			System.IO.File.Delete("./test.md");
			Assert.Equal("application/octet-stream", Tools.GetMIME("aaa"));
			Assert.Equal("application/postscript", Tools.GetMIME("ai"));
		}
	}
}
