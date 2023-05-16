using ExtensionMethods;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

using Xunit;

namespace ExtensionMethodsTests
{
	public class StringTest
	{
		[Fact]
		public void TrimStart()
		{
			Assert.Equal("tetetst", "tetetst".TrimStart("et"));
			Assert.Equal("etst", "etetst".TrimStart("et"));
			Assert.Equal("好", "你好".TrimStart("你"));
			Assert.Equal("好你好", "你好你好".TrimStart("你"));
			Assert.Equal("你好", "你好你好".TrimStart("你好"));
		}
		[Fact]
		public void TrimEnd()
		{
			Assert.Equal("etetst", "etetst".TrimEnd(""));
			Assert.Equal("etetst", "etetst".TrimEnd(null));
			Assert.Equal("etetst", "etetst".TrimEnd("et"));
			Assert.Equal("etet", "etetst".TrimEnd("st"));
			Assert.Equal("你好", "你好".TrimEnd("你"));
			Assert.Equal("你好", "你好你好".TrimEnd("你好"));
			Assert.Equal("你好你", "你好你好".TrimEnd("好"));
		}
		[Fact]
		public void Repeat()
		{
			Assert.Equal("", "a".Repeat(0));
			Assert.Equal("a", "a".Repeat(1));
			Assert.Equal("aa", "a".Repeat(2));
			Assert.Equal("", "好".Repeat(0));
			Assert.Equal("好", "好".Repeat(1));
			Assert.Equal("好好", "好".Repeat(2));
		}
		[Fact]
		public void Contains()
		{
			List<string> list = new List<string> { "1", "a", "森" };
			Assert.True("123456".Contains(list));
			Assert.True("asdfg".Contains(list));
			Assert.True("森林".Contains(list));
			Assert.True("123456asdfg".Contains(list));
			Assert.True("123456森林".Contains(list));
			Assert.True("asdfg森林".Contains(list));
			Assert.False("45678".Contains(list));
			Assert.False("qwert".Contains(list));
			Assert.False("你好".Contains(list));
		}
		[Fact]
		public void RemoveChineseChar()
		{
			Assert.Equal("qwer", "qwer".RemoveChineseChar());
			Assert.Equal("qw", "qw中文".RemoveChineseChar());
			Assert.Equal("qw", "中文qw".RemoveChineseChar());
			Assert.Equal("qw", "中文q中文w".RemoveChineseChar());
			Assert.Equal("qw1", "中文q中文w鑫森淼焱垚1".RemoveChineseChar());
		}
		[Fact]
		public void IsNullOrEmpty()
		{
			Assert.True("".IsNullOrEmpty());
			string s = null;
			Assert.True(s.IsNullOrEmpty());
			Assert.False(" ".IsNullOrEmpty());
			Assert.False("\r".IsNullOrEmpty());
			Assert.False("\n".IsNullOrEmpty());
			Assert.False("\r\n".IsNullOrEmpty());
		}
		[Fact]
		public void IsNullOrWhiteSpace()
		{
			Assert.True("".IsNullOrWhiteSpace());
			string s = null;
			Assert.True(s.IsNullOrWhiteSpace());
			Assert.True(" ".IsNullOrWhiteSpace());
			Assert.True("\r".IsNullOrWhiteSpace());
			Assert.True("\n".IsNullOrWhiteSpace());
			Assert.True("\r\n".IsNullOrWhiteSpace());
		}
		[Fact]
		public void CRC()
		{
			Assert.Throws<InvalidEnumArgumentException>(() => "QN=20180427125809000;ST=22;CN=2011;PW=123456;MN=LXHB0CS0306310;Flag=5;CP=&&DataTime=20180427125809;a34004-Rtd=75600.000,a34004-Flag=N;a34002-Rtd=140600.000,a34002-Flag=N;a34001-Rtd=178000.000,a34001-Flag=N;a01001-Rtd=24.8,a01001-Flag=N;a01002-Rtd=46.8,a01002-Flag=N;a01006-Rtd=101.380,a01006-Flag=N;a01007-Rtd=0.0,a01007-Flag=N;a01008-Rtd=25,a01008-Flag=N;LA-Rtd=57.9,LA-Flag=N&&".CRC((CrcOption)(-1)));
			Assert.Equal("5440", "QN=20180427125809000;ST=22;CN=2011;PW=123456;MN=LXHB0CS0306310;Flag=5;CP=&&DataTime=20180427125809;a34004-Rtd=75600.000,a34004-Flag=N;a34002-Rtd=140600.000,a34002-Flag=N;a34001-Rtd=178000.000,a34001-Flag=N;a01001-Rtd=24.8,a01001-Flag=N;a01002-Rtd=46.8,a01002-Flag=N;a01006-Rtd=101.380,a01006-Flag=N;a01007-Rtd=0.0,a01007-Flag=N;a01008-Rtd=25,a01008-Flag=N;LA-Rtd=57.9,LA-Flag=N&&".CRC(CrcOption.CRC16_HJ212));
		}
		[Fact]
		public void Hash()
		{
			Assert.Throws<InvalidEnumArgumentException>(() => "".Hash((HashOption)(-1)));
			Assert.Throws<ArgumentException>(() => "".Hash(HashOption.HmacMD5));
			Assert.Throws<ArgumentException>(() => "".Hash(HashOption.HmacSHA1));
			Assert.Throws<ArgumentException>(() => "".Hash(HashOption.HmacSHA256));
			Assert.Throws<ArgumentException>(() => "".Hash(HashOption.HmacSHA384));
			Assert.Throws<ArgumentException>(() => "".Hash(HashOption.HmacSHA512));
			Assert.Equal("611fc919a5d54f0f", "qwe".Hash(HashOption.MD5_16));
			Assert.Equal("0d3389d9dea66ae1", "你好".Hash(HashOption.MD5_16));
			Assert.Equal("76d80224611fc919a5d54f0ff9fba446", "qwe".Hash(HashOption.MD5_32));
			Assert.Equal("7eca689f0d3389d9dea66ae112e5cfd7", "你好".Hash(HashOption.MD5_32));
			Assert.Equal("056eafe7cf52220de2df36845b8ed170c67e23e3", "qwe".Hash(HashOption.SHA1));
			Assert.Equal("440ee0853ad1e99f962b63e459ef992d7c211722", "你好".Hash(HashOption.SHA1));
			Assert.Equal("489cd5dbc708c7e541de4d7cd91ce6d0f1613573b7fc5b40d3942ccb9555cf35", "qwe".Hash(HashOption.SHA256));
			Assert.Equal("670d9743542cae3ea7ebe36af56bd53648b0a1126162e78d81a32934a711302e", "你好".Hash(HashOption.SHA256));
			Assert.Equal("6ff673e67f72b6e39ce7c37bd7c533db7586973ec90c5789d19537a8af77528f00d7f07706f7f00e4cf8a79729e842ba", "qwe".Hash(HashOption.SHA384));
			Assert.Equal("05f076c7d180e91d80a56d70b226fca01e2353554c315ac1e8caaaeca2ce0dc0d9d84e206a2bf1143a0ae1b9be9bcfa8", "你好".Hash(HashOption.SHA384));
			Assert.Equal("b5ba77af1f7bda735894e746a199acb1d2c836424da2fc46bebb55423dccbff871877a30fab77a31e47b0a29ea0154882e532e9a29b220a8f2958773313bbb2a", "qwe".Hash(HashOption.SHA512));
			Assert.Equal("5232181bc0d9888f5c9746e410b4740eb461706ba5dacfbc93587cecfc8d068bac7737e92870d6745b11a25e9cd78b55f4ffc706f73cfcae5345f1b53fb8f6b5", "你好".Hash(HashOption.SHA512));


			Assert.Equal("609434e028486f31244b7fd4ce369112", "qwe".Hash(HashOption.HmacMD5, "q"));
			Assert.Equal("adf81d1a3a61302e1c800ac41f015f65", "你好".Hash(HashOption.HmacMD5, "你好"));
			Assert.Equal("c4ba4af2789b16d23017ab2a850eaadc1c0255ab", "qwe".Hash(HashOption.HmacSHA1, "q"));
			Assert.Equal("c35b67905094a86f35d77bab780c748a666d0914", "你好".Hash(HashOption.HmacSHA1, "你好"));
			Assert.Equal("e577412e980265f3ae26b8a0c1bb35524d20bedf08343ee8b1a90c5f1d6e2ea3", "qwe".Hash(HashOption.HmacSHA256, "q"));
			Assert.Equal("31a726c16c9c8de1ebcfd2008b7bc5cd737489f42b01278ee0ff8fb15e09d55e", "你好".Hash(HashOption.HmacSHA256, "你好"));
			Assert.Equal("b51cec99cd1dbc70c1099d2234c8c6743a81556bb97e1a28fc3aa6a571f78bc8a5459c7ddac5f5acb105f28a4468bb48", "qwe".Hash(HashOption.HmacSHA384, "q"));
			Assert.Equal("9d1657066be617417ddfc6dff4d6bf39c4e603be4791f113468ea2fcd991512b097054a26819a031bcad3ead1d0574df", "你好".Hash(HashOption.HmacSHA384, "你好"));
			Assert.Equal("d02a7013fe5cc998f3157cc1941b87301266c8309b69d425fd5b1a2248d8507b72f9db91177bb4a5744ddbb14893d9a688b103029f1b849791376377fb1cb976", "qwe".Hash(HashOption.HmacSHA512, "q"));
			Assert.Equal("7f11ca77e018cb7c0d68a8f89e537cfd1ff034c9a04f628309aaf91683df9087bdd36083c5892f049539fa5aba37d9636b760c2332aa2bf8fa57bc48e43e4b2e", "你好".Hash(HashOption.HmacSHA512, "你好"));
		}
		[Fact]
		public void Encrypt()
		{
			#region DES
			Assert.Throws<ArgumentException>(() => "".Encrypt(0, "12345678", "87654321"));
			Assert.Equal("VrHKOUNsGcA=", "abcdefgh".Encrypt(EncryptOption.DES_CBC_None, "12345678", "87654321"));
			Assert.Equal("UFAZxUlQ7Pc=", "你好nh".Encrypt(EncryptOption.DES_CBC_None, "12345678", "87654321"));
			Assert.Equal("BUhZfGyF0Mw=", "a".Encrypt(EncryptOption.DES_CBC_PKCS7, "12345678", "87654321"));
			Assert.Equal("5GgZng1JeGk=", "你好".Encrypt(EncryptOption.DES_CBC_PKCS7, "12345678", "87654321"));
			Assert.Equal("8KM3WaJc8iw=", "a".Encrypt(EncryptOption.DES_CBC_Zeros, "12345678", "87654321"));
			Assert.Equal("251F1V4AXBk=", "你好".Encrypt(EncryptOption.DES_CBC_Zeros, "12345678", "87654321"));
			Assert.Equal("euXBnQ9xZT8=", "a".Encrypt(EncryptOption.DES_CBC_ANSIX923, "12345678", "87654321"));
			Assert.Equal("c2Z+3TwvMYQ=", "你好".Encrypt(EncryptOption.DES_CBC_ANSIX923, "12345678", "87654321"));
			#endregion
		}
		[Fact]
		public void Decrypt()
		{
			#region DES
			Assert.Throws<ArgumentNullException>(() => "".Decrypt(0, "12345678", "87654321"));
			Assert.Equal("abcdefgh", "VrHKOUNsGcA=".Decrypt(EncryptOption.DES_CBC_None, "12345678", "87654321"));
			Assert.Equal("你好nh", "UFAZxUlQ7Pc=".Decrypt(EncryptOption.DES_CBC_None, "12345678", "87654321"));
			Assert.Equal("a", "BUhZfGyF0Mw=".Decrypt(EncryptOption.DES_CBC_PKCS7, "12345678", "87654321"));
			Assert.Equal("你好", "5GgZng1JeGk=".Decrypt(EncryptOption.DES_CBC_PKCS7, "12345678", "87654321"));
			Assert.Equal("a\0\0\0\0\0\0\0", "8KM3WaJc8iw=".Decrypt(EncryptOption.DES_CBC_Zeros, "12345678", "87654321"));
			Assert.Equal("你好\0\0", "251F1V4AXBk=".Decrypt(EncryptOption.DES_CBC_Zeros, "12345678", "87654321"));
			Assert.Equal("a", "euXBnQ9xZT8=".Decrypt(EncryptOption.DES_CBC_ANSIX923, "12345678", "87654321"));
			Assert.Equal("你好", "c2Z+3TwvMYQ=".Decrypt(EncryptOption.DES_CBC_ANSIX923, "12345678", "87654321"));
			#endregion
		}
		#region Convert
		[Fact]
		public void ToByteArray()
		{
			Assert.Equal(new byte[] { 104, 101, 108, 108, 111, 32, 119, 111, 114, 108, 100, 33 }, "hello world!".ToByteArray());
			Assert.Equal(new byte[] { 228, 189, 160, 229, 165, 189 }, "你好".ToByteArray());
		}
		[Fact]
		public void ToDateTime()
		{
			Assert.Equal(DateTime.Parse("2006-01-02 14:04:05", null, System.Globalization.DateTimeStyles.AssumeUniversal), "2006-01-02T15:04:05.0000000+01:00".ToDateTime());
			Assert.Equal(DateTime.Parse("2006-01-02 15:04:05"), "2006-01-02 15:04:05".ToDateTime());
			Assert.Equal(DateTime.Parse("2006-01-02 15:04:00"), "2006-01-02 15:04".ToDateTime());
			Assert.Equal(DateTime.Parse("2006-01-02 15:00:00"), "2006-01-02 15".ToDateTime());
			Assert.Equal(DateTime.Parse("2006-01-02 00:00:00"), "2006-01-02".ToDateTime());
			Assert.Equal(DateTime.Parse("2006-01-02 15:04:05"), "20060102150405".ToDateTime());
			Assert.Equal(DateTime.Parse("2006-01-02 15:04:00"), "200601021504".ToDateTime());
			Assert.Equal(DateTime.Parse("2006-01-02 15:00:00"), "2006010215".ToDateTime());
			Assert.Equal(DateTime.Parse("2006-01-02 00:00:00"), "20060102".ToDateTime());
			Assert.Equal(DateTime.Parse("2006-01-01 00:00:00"), "200601".ToDateTime());
			Assert.Equal(DateTime.Parse("2006-02-01 00:00:00"), "200602".ToDateTime());
			Assert.Throws<FormatException>(() => "1".ToDateTime());

			Assert.Equal(DateTime.Parse("2006-01-02 15:04:05"), "2006-01-02 15:04:05".ToDateTime(DateTime.Parse("2006-01-01 00:00:00")));
			Assert.Equal(DateTime.Parse("2006-01-02 15:04:00"), "2006-01-02 15:04".ToDateTime(DateTime.Parse("2006-01-01 00:00:00")));
			Assert.Equal(DateTime.Parse("2006-01-02 15:00:00"), "2006-01-02 15".ToDateTime(DateTime.Parse("2006-01-01 00:00:00")));
			Assert.Equal(DateTime.Parse("2006-01-02 00:00:00"), "2006-01-02".ToDateTime(DateTime.Parse("2006-01-01 00:00:00")));
			Assert.Equal(DateTime.Parse("2006-01-02 15:04:05"), "20060102150405".ToDateTime(DateTime.Parse("2006-01-01 00:00:00")));
			Assert.Equal(DateTime.Parse("2006-01-02 15:04:00"), "200601021504".ToDateTime(DateTime.Parse("2006-01-01 00:00:00")));
			Assert.Equal(DateTime.Parse("2006-01-02 15:00:00"), "2006010215".ToDateTime(DateTime.Parse("2006-01-01 00:00:00")));
			Assert.Equal(DateTime.Parse("2006-01-02 00:00:00"), "20060102".ToDateTime(DateTime.Parse("2006-01-01 00:00:00")));
			Assert.Equal(DateTime.Parse("2006-01-01 00:00:00"), "200601".ToDateTime(DateTime.Parse("2006-01-01 00:00:00")));
			Assert.Equal(DateTime.Parse("2006-02-01 00:00:00"), "200602".ToDateTime(DateTime.Parse("2006-01-01 00:00:00")));
			Assert.Equal(DateTime.Parse("2006-01-01 00:00:00"), "2006".ToDateTime(DateTime.Parse("2006-01-01 00:00:00")));

			Assert.Equal(DateTime.Parse("2006-01-01 00:00:00"), "1".ToDateTime(DateTime.Parse("2006-01-01 00:00:00")));
		}
		[Fact]
		public void ToDateTimeOffset()
		{
			Assert.Equal(DateTimeOffset.Parse("2006-01-02 06:04:05+00:00"), "2006-01-02 15:04:05+09:00".ToDateTimeOffset().ToUniversalTime());
			Assert.Throws<FormatException>(() => "1".ToDateTime());

			Assert.Equal(DateTimeOffset.Parse("2005-12-31 16:00:00+00:00"), "1".ToDateTimeOffset(DateTimeOffset.Parse("2006-01-01 00:00:00").ToUniversalTime()).ToUniversalTime());
		}

		[Fact]
		public void ToInt()
		{
			Assert.Throws<FormatException>(() => "你好".ToInt());
			Assert.Throws<OverflowException>(() => "2147483648".ToInt());
			Assert.Equal(123, "123".ToInt());
			Assert.Equal(123, "123.".ToInt());
			Assert.Equal(123, "123.123".ToInt());
			Assert.Equal(0, "0.123".ToInt());
			Assert.Equal(0, ".123".ToInt());

			Assert.Equal(7, "你好".ToInt(7));
			Assert.Equal(7, "2147483648".ToInt(7));
			Assert.Equal(123, "123".ToInt(7));
			Assert.Equal(123, "123.".ToInt(7));
			Assert.Equal(123, "123.123".ToInt(7));
			Assert.Equal(0, "0.123".ToInt(7));
			Assert.Equal(0, ".123".ToInt(7));
		}
		[Fact]
		public void ToDouble()
		{
			Assert.Throws<FormatException>(() => "你好".ToDouble());
			Assert.Throws<OverflowException>(() => "17976931348623157E+309".ToDouble());
			Assert.Equal(123, "123".ToDouble());
			Assert.Equal(123, "123.".ToDouble());
			Assert.Equal(123.123, "123.123".ToDouble());
			Assert.Equal(0.123, "0.123".ToDouble());
			Assert.Equal(0.123, ".123".ToDouble());

			Assert.Equal(1.234, "你好".ToDouble(1.234));
			Assert.Equal(1.234, "1.7976931348623157E+309".ToDouble(1.234));
			Assert.Equal(123, "123".ToDouble(1.234));
			Assert.Equal(123, "123.".ToDouble(1.234));
			Assert.Equal(123.123, "123.123".ToDouble(1.234));
			Assert.Equal(0.123, "0.123".ToDouble(1.234));
			Assert.Equal(0.123, ".123".ToDouble(1.234));
		}
		[Fact]
		public void ToBsae64String()
		{
			Assert.Equal("5L2g5aW9", "你好".ToBsae64String());
		}

		[Fact]
		public void AsBase64ToStream()
		{
			var s = "5L2g5aW9".AsBase64ToStream();
			s.Position = 0;
			byte[] buffer = new byte[s.Length];
			s.Read(buffer);
			Assert.Equal("5L2g5aW9", buffer.ToBase64String());
		}

		[Theory]
		[InlineData("AbbCddEff")]
		[InlineData("abbCddEff")]
		[InlineData("abb-Cdd-Eff")]
		[InlineData("abb-Cdd_Eff")]
		public void ToNamingConvention(string identifier)
		{
			Assert.Throws<InvalidEnumArgumentException>(() => identifier.ToNamingConvention(0));
			Assert.Equal("abbcddeff", identifier.ToNamingConvention(NamingConvention.flatcase));
			Assert.Equal("ABBCDDEFF", identifier.ToNamingConvention(NamingConvention.UPPERCASE));
			Assert.Equal("abbCddEff", identifier.ToNamingConvention(NamingConvention.camelCase));
			Assert.Equal("AbbCddEff", identifier.ToNamingConvention(NamingConvention.PascalCase));
			Assert.Equal("abb_cdd_eff", identifier.ToNamingConvention(NamingConvention.snake_case));
			Assert.Equal("abb_Cdd_Eff", identifier.ToNamingConvention(NamingConvention.camel_Snake_Case));
			Assert.Equal("ABB_CDD_EFF", identifier.ToNamingConvention(NamingConvention.MACRO_CASE));
			Assert.Equal("Abb_Cdd_Eff", identifier.ToNamingConvention(NamingConvention.Pascal_Snake_Case));
			Assert.Equal("abb-cdd-eff", identifier.ToNamingConvention(NamingConvention.kebab一case));
			Assert.Equal("ABB-CDD-EFF", identifier.ToNamingConvention(NamingConvention.TRAIN一CASE));
			Assert.Equal("Abb-Cdd-Eff", identifier.ToNamingConvention(NamingConvention.Train一Case));
		}

		[Fact]
		public void Convert()
		{
			Assert.Throws<InvalidCastException>(() => "一".Convert<int>());
			Assert.Equal(1, "1".Convert<int>());
			Assert.Equal(1D, "1".Convert<double>());
			Assert.Equal(1.1F, "1.1".Convert<float>());
			Assert.Equal(1.1D, "1.1".Convert<double>());
			Assert.Equal(DateTime.Parse("2006-01-01 00:00:00"), "2006-01-01 00:00:00".Convert<DateTime>());
			Assert.Equal(1, "1".Convert<int?>());
			Assert.Null("".Convert<int?>());
			Assert.Equal(new Obj(), new Obj().ToJson().Convert<Obj>());
		}
		#endregion
		#region Json
		[Fact]
		public void AsJsonToObject()
		{
			List<int> ls = new List<int> { 1, 2, 3 };
			string lsJson = @"[1,2,3]";
			Assert.Equal(ls, lsJson.AsJsonToObject<List<int>>());
			Obj obj = new Obj() { N = 1, S = "a", Array = null };
			string objJson = @"{""N"":1,""S"":""a"",""Array"":null}";
			Assert.Equal(obj, objJson.AsJsonToObject<Obj>());
			obj.Array = ls;
			objJson = @"{""N"":1,""S"":""a"",""Array"":[1,2,3]}";
			Assert.Equal(obj, objJson.AsJsonToObject<Obj>());
		}
		#endregion
	}
	class Obj
	{
		public int N { get; set; }
		public string S { get; set; }
		public List<int> Array { get; set; }
		public override bool Equals(object obj)
		{
			if (obj == null || GetType() != obj.GetType())
			{
				return false;
			}
			Obj o = obj as Obj;
			return N == o.N
				&& S == o.S
				&& (Array == null || Array.SequenceEqual(o.Array));
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}
}
