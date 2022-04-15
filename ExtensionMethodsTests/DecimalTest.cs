
using ExtensionMethods;

using System;
using System.Collections.Generic;
using System.Data;

using Xunit;

namespace ExtensionMethodsTests
{
	public class DecimalTest
	{
		/// <summary>
		/// 测试
		/// <a href="https://github.com/Sandwych/rmb_converter/blob/master/src/dotnet/Sandwych.RmbConverter.Test/RmbUpperTests.cs"></a>
		/// </summary>
		[Fact]
		public void ToRmbUpper()
		{
			Assert.Throws<ArgumentOutOfRangeException>(() => (-1M).ToRmbUpper());
			Assert.Throws<ArgumentOutOfRangeException>(() => 9999999999999999.99M.ToRmbUpper());
			Assert.Equal("零元整", 0.00M.ToRmbUpper());
			Assert.Equal("叁分", 0.03M.ToRmbUpper());
			Assert.Equal("叁角整", 0.30M.ToRmbUpper());
			Assert.Equal("叁角叁分", 0.33M.ToRmbUpper());
			Assert.Equal("壹元零叁分", 1.03M.ToRmbUpper());
			Assert.Equal("壹万元零壹角整", 10000.10M.ToRmbUpper());
			Assert.Equal("壹万元零壹分", 10000.01M.ToRmbUpper());
			Assert.Equal("壹仟元零壹角壹分", 1000.11M.ToRmbUpper());
			Assert.Equal("壹仟零壹元壹角壹分", 1001.11M.ToRmbUpper());
			Assert.Equal("贰仟叁佰贰拾叁元贰角贰分", 2323.22M.ToRmbUpper());
			Assert.Equal("叁仟壹佰伍拾元零伍角整", 3150.50M.ToRmbUpper());
			Assert.Equal("陆仟伍佰元整", 6500.00M.ToRmbUpper());
			Assert.Equal("壹万壹仟零壹拾元零壹角贰分", 11010.12M.ToRmbUpper());
			Assert.Equal("壹万零壹佰元零叁分", 10100.03M.ToRmbUpper());
			Assert.Equal("叁万伍仟元零玖角陆分", 35000.96M.ToRmbUpper());
			Assert.Equal("壹拾万零叁拾元整", 100030.00M.ToRmbUpper());
			Assert.Equal("壹拾万零伍仟元整", 105000.00M.ToRmbUpper());
			Assert.Equal("壹拾伍万零壹元整", 150001.00M.ToRmbUpper());
			Assert.Equal("壹拾玖万玖仟玖佰贰拾叁元整", 199923.00M.ToRmbUpper());
			Assert.Equal("陆仟零叁万陆仟元整", 60036000.00M.ToRmbUpper());
			Assert.Equal("壹仟万元整", 10000000.00M.ToRmbUpper());
			Assert.Equal("壹拾万零壹佰元整", 100100.00M.ToRmbUpper());
			Assert.Equal("叁佰肆拾伍万陆仟柒佰捌拾玖元壹角整", 3456789.10M.ToRmbUpper());
			Assert.Equal("玖亿玖仟玖佰玖拾万零玖仟零玖元壹角整", 999909009.10M.ToRmbUpper());
			Assert.Equal("叁亿肆仟伍佰万零壹拾贰元叁角叁分", 345000012.33M.ToRmbUpper());
			Assert.Equal("贰万元零壹角贰分", 20000.12M.ToRmbUpper());
			Assert.Equal("壹拾亿零叁仟肆佰伍拾陆万柒仟捌佰玖拾元零壹角叁分", 1034567890.1299999999999999M.ToRmbUpper());
			Assert.Equal("玖仟零玖万玖仟玖佰玖拾玖亿玖仟玖佰玖拾玖万玖仟玖佰玖拾玖元壹角贰分", 9009999999999999.12M.ToRmbUpper());
		}
	}
}
