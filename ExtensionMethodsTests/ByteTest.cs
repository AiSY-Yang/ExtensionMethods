
using ExtensionMethods;

using Xunit;

namespace ExtensionMethodsTests
{
	public class ByteTest
	{
		[Fact]
		public void ToUtf8String()
		{
			byte[] helloworld = { 104, 101, 108, 108, 111, 32, 119, 111, 114, 108, 100, 33 };
			Assert.Equal("hello world!", helloworld.ToUtf8String());
			Assert.Equal("lo", helloworld.ToUtf8String(3,2));
			byte[] hello = { 228, 189, 160, 229, 165, 189 };
			Assert.Equal("你好", hello.ToUtf8String());
			Assert.Equal("好", hello.ToUtf8String(3,3));
		}
		[Fact]
		public void ToBase64String()
		{ 
			byte[] helloworld = { 104, 101, 108, 108, 111, 32, 119, 111, 114, 108, 100, 33 };
			Assert.Equal("aGVsbG8gd29ybGQh", helloworld.ToBase64String());
			byte[] hello = { 228, 189, 160, 229, 165, 189 };
			Assert.Equal("5L2g5aW9", hello.ToBase64String());

		}
	}
}
