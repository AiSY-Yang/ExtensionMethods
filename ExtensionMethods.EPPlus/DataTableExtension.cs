using OfficeOpenXml;

using System;
using System.Data;
using System.Linq;
using ExtensionMethods;

namespace ExtensionMethods.EPPlus
{
	/// <summary>
	/// EPPLUS扩展
	/// </summary>
	public static class DataTableExtension
	{
		/// <summary>
		/// 将表数据导出至Excel中,第一行为列名
		/// </summary>
		/// <param name="dataTable"></param>
		/// <param name="excelPackage">要导出的Excel文件</param>
		/// <param name="SheetName">Sheet页名,如为空则使用dataTable.TableName属性,dataTable.TableName也为空则使用Sheet,Sheet页名后面为编号"</param>
		/// <returns>页数</returns>
		public static int InsertExcel(this DataTable dataTable, in ExcelPackage excelPackage, string SheetName = null)
		{
			for (int i = 0; i <= dataTable.Rows.Count / 1048575; i++)
			{
				var ws = excelPackage.Workbook.Worksheets.Add(((SheetName == null || SheetName == "") ? (dataTable.TableName == null || dataTable.TableName == "") ? "Sheet" : dataTable.TableName : SheetName) + (i + 1));
				DataTable table = dataTable.Clone();
				foreach (var item in dataTable.AsEnumerable().Skip(i * 1048575).Take(1048575).ToList())
				{
					table.ImportRow(item);
				}
				ws.Cells[1, 1].LoadFromDataTable(table, true);
			}
			excelPackage.Save();
			return (dataTable.Rows.Count / 1048575) + 1;
		}
	}
}
