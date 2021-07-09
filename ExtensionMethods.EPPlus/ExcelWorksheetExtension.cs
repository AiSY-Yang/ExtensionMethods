using OfficeOpenXml;

using System.Data;
using System.Linq;

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
				return new DataTable();
			}
			//获取worksheet的行数
			int rows = worksheet.Dimension.End.Row;
			//获取worksheet的列数
			int cols = worksheet.Dimension.End.Column;
			DataTable dt = new DataTable(worksheet.Name);
			for (int i = 1; i <= rows; i++)
			{
				DataRow row = dt.Rows.Add();
				for (int j = 1; j <= cols; j++)
				{
					if (firstIsHead)
					{
						//将第一行设置为datatable的标题
						if (i == 1)
							dt.Columns.Add(worksheet.Cells[i, j].Value.ToString());
						//剩下的写入datatable
						else
							row[j - 1] = worksheet.Cells[i, j].Value;
					}
					else
					{
						if (i == 1)
							dt.Columns.Add();
						row[j] = worksheet.Cells[i, j].Value;
					}
				}
			}
			return dt;
		}
	}
}
