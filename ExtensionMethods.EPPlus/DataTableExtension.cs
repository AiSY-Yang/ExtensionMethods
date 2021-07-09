using OfficeOpenXml;

using System.Data;
using System.Linq;

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
		/// <param name="SheetName">表名,如为空则使用DataTable.TableName,如有分页会在表名后加"-Page1..N"</param>
		/// <returns>页数</returns>
		public static int ToExcel(this DataTable dataTable, in ExcelPackage excelPackage, string SheetName = null)
		{
			bool NeedSplitPage = dataTable.Rows.Count > 1048576;
			for (int i = 0; i <= dataTable.Rows.Count / 1048575; i++)
			{
				var ws = excelPackage.Workbook.Worksheets.Add(SheetName ?? dataTable.TableName + (NeedSplitPage ? ("-Page" + (i + 1)) : ""));
				ws.Cells[1, 1].LoadFromCollection(dataTable.AsEnumerable().Skip(i * 1048575).Take(1048575).ToList());
			}
			return dataTable.Rows.Count / 1048575;
		}
	}

}
