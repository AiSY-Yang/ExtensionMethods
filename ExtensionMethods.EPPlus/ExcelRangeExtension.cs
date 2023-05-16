using OfficeOpenXml;

using System;
using System.Data;
using System.Linq;
using ExtensionMethods;
using System.Runtime.CompilerServices;
using System.Collections.Generic;

namespace ExtensionMethods.EPPlus
{
	/// <summary>
	/// EPPLUS扩展
	/// </summary>
	public static class ExcelRangeExtension
	{
		/// <summary>
		/// 将指定数组按行加载
		/// </summary>
		/// <param name="cell"></param>
		/// <param name="data"></param>
		/// <returns></returns>
		public static ExcelRangeBase LoadRowFromArrays(this ExcelRange cell, object[] data)
		{
			return cell.LoadFromArrays(new List<object[]>() { data });
		}
	}
}
