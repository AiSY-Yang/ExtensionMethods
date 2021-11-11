
using ExtensionMethods;
using ExtensionMethods.EPPlus;

using OfficeOpenXml;

using System;
using System.Data;
using System.IO;

using Xunit;

namespace ExtensionMethodsTests.EPPlus
{
	public class ExcelWorksheetTest
	{
		public ExcelWorksheetTest()
		{
			ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
		}

		[Fact]
		public void ToDataTable()
		{
			DateTime time = DateTime.Now.TruncateSecend(1);
			using Stream stream = new MemoryStream();
			using ExcelPackage excelPackage = new ExcelPackage();

			excelPackage.Workbook.Worksheets.Add("Sheet1");
			excelPackage.Workbook.Worksheets[0].Cells[1, 1].Value = "col1";
			excelPackage.Workbook.Worksheets[0].Cells[1, 2].Value = "col2";
			excelPackage.Workbook.Worksheets[0].Cells[1, 3].Value = "col3";
			excelPackage.Workbook.Worksheets[0].Cells[2, 1].Value = 1;
			excelPackage.Workbook.Worksheets[0].Cells[2, 2].Value = "s";
			excelPackage.Workbook.Worksheets[0].Cells[2, 3].Value = time;
			excelPackage.Workbook.Worksheets[0].Cells[3, 1].Value = 2;
			excelPackage.Workbook.Worksheets[0].Cells[3, 2].Value = "s";
			excelPackage.Workbook.Worksheets[0].Cells[3, 3].Value = time;

			DataTable dataTable1 = excelPackage.Workbook.Worksheets[0].ToDataTable();
			Assert.Equal("Sheet1", dataTable1.TableName);
			Assert.Equal(2, dataTable1.Rows.Count);
			Assert.Equal("col1", dataTable1.Columns[0].ColumnName);
			Assert.Equal("col2", dataTable1.Columns[1].ColumnName);
			Assert.Equal("col3", dataTable1.Columns[2].ColumnName);
			Assert.Equal(1, dataTable1.Rows[0][0]);
			Assert.Equal("s", dataTable1.Rows[0][1]);
			Assert.Equal(time, dataTable1.Rows[0][2]);
			Assert.Equal(2, dataTable1.Rows[1][0]);
			Assert.Equal("s", dataTable1.Rows[1][1]);
			Assert.Equal(time, dataTable1.Rows[1][2]);

			DataTable dataTable2 = excelPackage.Workbook.Worksheets[0].ToDataTable(false);
			Assert.Equal(3, dataTable2.Rows.Count);
			Assert.Equal("Column1", dataTable2.Columns[0].ColumnName);
			Assert.Equal("Column2", dataTable2.Columns[1].ColumnName);
			Assert.Equal("Column3", dataTable2.Columns[2].ColumnName);
			Assert.Equal("col1", dataTable2.Rows[0][0]);

			#region empty sheet
			excelPackage.Workbook.Worksheets.Add("Sheet2");
			Assert.Equal(0, excelPackage.Workbook.Worksheets[1].ToDataTable().Rows.Count);
			Assert.Equal("Sheet2", excelPackage.Workbook.Worksheets[1].ToDataTable().TableName);
			Assert.Equal(0, excelPackage.Workbook.Worksheets[1].ToDataTable(false).Rows.Count);
			Assert.Equal("Sheet2", excelPackage.Workbook.Worksheets[1].ToDataTable(false).TableName);
			#endregion
		}
	}
}
