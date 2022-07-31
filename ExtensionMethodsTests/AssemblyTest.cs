
using ExtensionMethods;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text.Json;

using Xunit;

namespace ExtensionMethodsTests
{
	public class AssemblyTest
	{
		/// <summary>
		/// 获取XML注释
		/// </summary>
		/// <remarks>备注信息</remarks>\
		/// <pa name="aaa">1</pa>
		/// <pa name="bbb">2</pa>
		/// <returns>返回值</returns>
		[Fact]
		public void GetXMLMember()
		{
			var xml = Assembly.GetExecutingAssembly().GetXMLMember();
			Assert.Equal("获取XML注释", xml.FirstOrDefault(x => x.ID == "M:" + typeof(AssemblyTest).FullName + "." + nameof(GetXMLMember)).Summary);
			Assert.Equal("返回值", xml.FirstOrDefault(x => x.ID == "M:" + typeof(AssemblyTest).FullName + "." + nameof(GetXMLMember)).Return);
			Assert.Equal("备注信息", xml.FirstOrDefault(x => x.ID == "M:" + typeof(AssemblyTest).FullName + "." + nameof(GetXMLMember)).Content.FirstOrDefault(x => x.Type == "remarks").Content);
			Assert.Equal("1", xml.FirstOrDefault(x => x.ID == "M:" + typeof(AssemblyTest).FullName + "." + nameof(GetXMLMember)).Content.FirstOrDefault(x => x.Type == "pa" && x.Name == "aaa").Content);
			Assert.Equal("2", xml.FirstOrDefault(x => x.ID == "M:" + typeof(AssemblyTest).FullName + "." + nameof(GetXMLMember)).Content.FirstOrDefault(x => x.Type == "pa" && x.Name == "bbb").Content);
			ExtensionMethods.AssemblyExtension.AssemblyXmlCache[Assembly.GetExecutingAssembly()].Clear();
			xml = Assembly.GetExecutingAssembly().GetXMLMember();
			Assert.Null(xml.FirstOrDefault(x => x.ID == "M:" + typeof(AssemblyTest).FullName + "." + nameof(GetXMLMember))?.Summary);

		}
	}
}
