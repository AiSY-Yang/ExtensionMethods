using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using ExtensionMethods;

using RichardSzalay.MockHttp;

using Xunit;
using Xunit.Sdk;

namespace ExtensionMethodsTests
{
	public class FileInfoTest
	{
		[Fact]
		public async void Download()
		{
			var mockHttp = new MockHttpMessageHandler();
			mockHttp.When("download").Respond(Tools.GetMIME("txt"), "下载测试文件\r\n".Repeat(10000));

			string fileName = "/test.txt";
			FileInfo fileInfo = new FileInfo(fileName);
		await	fileInfo.Download("download");
		}
	}
}
