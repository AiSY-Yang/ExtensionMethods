﻿using System.Data;
using System.Linq;

using OfficeOpenXml;

namespace ExtensionMethods.EPPlus
{
	/// <summary>
	/// EPPLUS扩展
	/// </summary>
	public static class ExcelWorksheetExtension
	{
		/// <summary>
		/// 转换为datatable,区域为左上到右下
		/// </summary>
		/// <param name="worksheet"></param>
		/// <param name="firstIsHead">第一行是否当作列名</param>
		/// <returns></returns>
		public static DataTable ToDataTable(this OfficeOpenXml.ExcelWorksheet worksheet, bool firstIsHead = true)
		{
			if (worksheet.Dimension == null)
			{
				return new DataTable(worksheet.Name);
			}
			//获取worksheet的行数
			int rows = worksheet.Dimension.End.Row;
			//获取worksheet的列数
			int cols = worksheet.Dimension.End.Column;
			DataTable dt = new DataTable(worksheet.Name);
			int dataStart = 1;

			if (firstIsHead)
			{
				//将第一行设置为datatable的标题
				for (int j = 1; j <= cols; j++)
					dt.Columns.Add(new DataColumn(worksheet.Cells[1, j].Value.ToString(), worksheet.Cells[2, j].Value.GetType()));
				dataStart = 2;
			}
			else
			{
				for (int j = 1; j <= cols; j++)
					dt.Columns.Add();
			}

			for (int i = dataStart; i <= rows; i++)
			{
				DataRow row = dt.Rows.Add();
				for (int j = 1; j <= cols; j++)
				{
					row[j - 1] = worksheet.Cells[i, j].Value;
				}
			}
			return dt;
		}
		/// <summary>
		/// 自适应列宽
		/// </summary>
		/// <param name="worksheet"></param>
		public static void AutoFit(this OfficeOpenXml.ExcelWorksheet worksheet)
		{
			foreach (var item in worksheet.Columns)
			{
				item.AutoFit();
			}
		}
		/// <summary>
		/// 获取表头
		/// </summary>
		/// <param name="sheet"></param>
		/// <returns></returns>
		public static string[] GetHeaderColumns(this ExcelWorksheet sheet)
		{
			return sheet.Cells[sheet.Dimension.Start.Row, sheet.Dimension.Start.Column, 1, sheet.Dimension.End.Column]
				.Select(firstRowCell => firstRowCell.Text).ToArray();
		}
	}
}
