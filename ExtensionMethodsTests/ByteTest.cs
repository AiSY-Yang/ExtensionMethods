
using System;
using System.Text;

using ExtensionMethods;

using OfficeOpenXml.ConditionalFormatting;

using Xunit;

namespace ExtensionMethodsTests
{
	public class ByteTest
	{
		public ByteTest()
		{
			Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
		}

		[Fact]
		public void ToUtf8String()
		{
			byte[] helloworld = { 104, 101, 108, 108, 111, 32, 119, 111, 114, 108, 100, 33 };
			Assert.Equal("hello world!", helloworld.ToUtf8String());
			Assert.Equal("lo", helloworld.ToUtf8String(3, 2));
			byte[] hello = { 228, 189, 160, 229, 165, 189 };
			Assert.Equal("你好", hello.ToUtf8String());
			Assert.Equal("好", hello.ToUtf8String(3, 3));
		}
		[Fact]
		public void ToBase64String()
		{
			byte[] helloworld = { 104, 101, 108, 108, 111, 32, 119, 111, 114, 108, 100, 33 };
			Assert.Equal("aGVsbG8gd29ybGQh", helloworld.ToBase64String());
			byte[] hello = { 228, 189, 160, 229, 165, 189 };
			Assert.Equal("5L2g5aW9", hello.ToBase64String());
		}
		[Fact]
		public void ToHexString()
		{
			byte[] hello = { 228, 189, 160, 229, 165, 189 };
			Assert.Equal("e4bda0e5a5bd", hello.ToHexString());
		}
		[Fact]
		public void ToStringTest()
		{
			byte[] helloUtf8NoBom = { 0xe4, 0xbd, 0xa0, 0xe5, 0xa5, 0xbd, };
			byte[] helloUtf8WithBom = { 0xEF, 0xBB, 0xBF, 0xe4, 0xbd, 0xa0, 0xe5, 0xa5, 0xbd, };
			byte[] helloGb2312 = { 0xc4, 0xe3, 0xba, 0xc3, };
			Assert.Equal("你好", helloUtf8NoBom.ToString(Encoding.GetEncoding("gb2312")));
			Assert.Equal("你好", helloUtf8WithBom.ToString(Encoding.GetEncoding("gb2312")));
			Assert.Equal("你好", helloGb2312.ToString(Encoding.GetEncoding("gb2312")));
		}

		byte[] key = "12345678".ToByteArray();
		byte[] iv = "87654321".ToByteArray();
		[Fact]
		public void DESEncrypt()
		{
			Assert.Equal(Array.Empty<byte>(), Array.Empty<byte>().DESEncrypt(key, iv, System.Security.Cryptography.CipherMode.CBC, System.Security.Cryptography.PaddingMode.None));
			Assert.Equal(Convert.FromBase64String("VrHKOUNsGcA="), "abcdefgh".ToByteArray().DESEncrypt(key, iv, System.Security.Cryptography.CipherMode.CBC, System.Security.Cryptography.PaddingMode.None));
			Assert.Equal(Convert.FromBase64String("UFAZxUlQ7Pc="), "你好nh".ToByteArray().DESEncrypt(key, iv, System.Security.Cryptography.CipherMode.CBC, System.Security.Cryptography.PaddingMode.None));
			Assert.Equal(Convert.FromBase64String("BUhZfGyF0Mw="), "a".ToByteArray().DESEncrypt(key, iv, System.Security.Cryptography.CipherMode.CBC, System.Security.Cryptography.PaddingMode.PKCS7));
			Assert.Equal(Convert.FromBase64String("5GgZng1JeGk="), "你好".ToByteArray().DESEncrypt(key, iv, System.Security.Cryptography.CipherMode.CBC, System.Security.Cryptography.PaddingMode.PKCS7));
			Assert.Equal(Convert.FromBase64String("8KM3WaJc8iw="), "a".ToByteArray().DESEncrypt(key, iv, System.Security.Cryptography.CipherMode.CBC, System.Security.Cryptography.PaddingMode.Zeros));
			Assert.Equal(Convert.FromBase64String("251F1V4AXBk="), "你好".ToByteArray().DESEncrypt(key, iv, System.Security.Cryptography.CipherMode.CBC, System.Security.Cryptography.PaddingMode.Zeros));
			Assert.Equal(Convert.FromBase64String("euXBnQ9xZT8="), "a".ToByteArray().DESEncrypt(key, iv, System.Security.Cryptography.CipherMode.CBC, System.Security.Cryptography.PaddingMode.ANSIX923));
			Assert.Equal(Convert.FromBase64String("c2Z+3TwvMYQ="), "你好".ToByteArray().DESEncrypt(key, iv, System.Security.Cryptography.CipherMode.CBC, System.Security.Cryptography.PaddingMode.ANSIX923));
		}
		[Fact]
		public void DESDecrypt()
		{
			Assert.Equal(Array.Empty<byte>(), (Array.Empty<byte>()).DESDecrypt(key, iv, System.Security.Cryptography.CipherMode.CBC, System.Security.Cryptography.PaddingMode.None));
			Assert.Equal("abcdefgh".ToByteArray(), Convert.FromBase64String("VrHKOUNsGcA=").DESDecrypt(key, iv, System.Security.Cryptography.CipherMode.CBC, System.Security.Cryptography.PaddingMode.None));
			Assert.Equal("你好nh".ToByteArray(), Convert.FromBase64String("UFAZxUlQ7Pc=").DESDecrypt(key, iv, System.Security.Cryptography.CipherMode.CBC, System.Security.Cryptography.PaddingMode.None));
			Assert.Equal("a".ToByteArray(), Convert.FromBase64String("BUhZfGyF0Mw=").DESDecrypt(key, iv, System.Security.Cryptography.CipherMode.CBC, System.Security.Cryptography.PaddingMode.PKCS7));
			Assert.Equal("你好".ToByteArray(), Convert.FromBase64String("5GgZng1JeGk=").DESDecrypt(key, iv, System.Security.Cryptography.CipherMode.CBC, System.Security.Cryptography.PaddingMode.PKCS7));
			Assert.Equal("a\0\0\0\0\0\0\0".ToByteArray(), Convert.FromBase64String("8KM3WaJc8iw=").DESDecrypt(key, iv, System.Security.Cryptography.CipherMode.CBC, System.Security.Cryptography.PaddingMode.Zeros));
			Assert.Equal("你好\0\0".ToByteArray(), Convert.FromBase64String("251F1V4AXBk=").DESDecrypt(key, iv, System.Security.Cryptography.CipherMode.CBC, System.Security.Cryptography.PaddingMode.Zeros));
			Assert.Equal("a".ToByteArray(), Convert.FromBase64String("euXBnQ9xZT8=").DESDecrypt(key, iv, System.Security.Cryptography.CipherMode.CBC, System.Security.Cryptography.PaddingMode.ANSIX923));
			Assert.Equal("你好".ToByteArray(), Convert.FromBase64String("c2Z+3TwvMYQ=").DESDecrypt(key, iv, System.Security.Cryptography.CipherMode.CBC, System.Security.Cryptography.PaddingMode.ANSIX923));
		}
	}
}
