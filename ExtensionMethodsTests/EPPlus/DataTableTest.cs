
using ExtensionMethods;
using ExtensionMethods.EPPlus;

using OfficeOpenXml;

using System;
using System.Data;
using System.IO;

using Xunit;

namespace ExtensionMethodsTests.EPPlus
{
	public class DataTableTest
	{
		public DataTableTest()
		{
			ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
		}

		[Fact]
		public void InsertExcel()
		{
			DataTable dataTable = new DataTable();
			dataTable.Columns.Add("index", typeof(double));
			dataTable.Columns.Add("value", typeof(string));
			dataTable.Columns.Add("date", typeof(DateTime));
			DateTime time = DateTime.Now.TruncateSecend(1);
			dataTable.Rows.Add(1, "s", time);
			dataTable.Rows.Add(2, "s", time);
			using Stream stream = new MemoryStream();

			using ExcelPackage excelPackage = new ExcelPackage();

			dataTable.InsertExcel(excelPackage);
			dataTable.TableName = "TableName";
			dataTable.InsertExcel(excelPackage);
			dataTable.InsertExcel(excelPackage, "NamedSheet");
			stream.Seek(0, SeekOrigin.Begin);
			double totalRow = 2000000;
			for (int i = 3; i <= totalRow; i++)
			{
				dataTable.Rows.Add(i, "s", time);
			}
			dataTable.InsertExcel(excelPackage, "Big");

			Assert.Equal("Sheet1", excelPackage.Workbook.Worksheets[0].Name);
			Assert.Equal("index", excelPackage.Workbook.Worksheets[0].Cells[1, 1].Value);
			Assert.Equal("value", excelPackage.Workbook.Worksheets[0].Cells[1, 2].Value);
			Assert.Equal("date", excelPackage.Workbook.Worksheets[0].Cells[1, 3].Value);
			Assert.Equal(1D, excelPackage.Workbook.Worksheets[0].Cells[2, 1].Value);
			Assert.Equal("s", excelPackage.Workbook.Worksheets[0].Cells[2, 2].Value);
			Assert.Equal(time, excelPackage.Workbook.Worksheets[0].Cells[2, 3].GetValue<DateTime>());

			Assert.Equal("TableName1", excelPackage.Workbook.Worksheets[1].Name);
			Assert.Equal("index", excelPackage.Workbook.Worksheets[1].Cells[1, 1].Value);
			Assert.Equal("value", excelPackage.Workbook.Worksheets[1].Cells[1, 2].Value);
			Assert.Equal("date", excelPackage.Workbook.Worksheets[1].Cells[1, 3].Value);
			Assert.Equal(1D, excelPackage.Workbook.Worksheets[1].Cells[2, 1].Value);
			Assert.Equal("s", excelPackage.Workbook.Worksheets[1].Cells[2, 2].Value);
			Assert.Equal(time, excelPackage.Workbook.Worksheets[1].Cells[2, 3].GetValue<DateTime>());

			Assert.Equal("NamedSheet1", excelPackage.Workbook.Worksheets[2].Name);
			Assert.Equal("index", excelPackage.Workbook.Worksheets[2].Cells[1, 1].Value);
			Assert.Equal("value", excelPackage.Workbook.Worksheets[2].Cells[1, 2].Value);
			Assert.Equal("date", excelPackage.Workbook.Worksheets[2].Cells[1, 3].Value);
			Assert.Equal(1D, excelPackage.Workbook.Worksheets[2].Cells[2, 1].Value);
			Assert.Equal("s", excelPackage.Workbook.Worksheets[2].Cells[2, 2].Value);
			Assert.Equal(time, excelPackage.Workbook.Worksheets[2].Cells[2, 3].GetValue<DateTime>());

			Assert.Equal("Big1", excelPackage.Workbook.Worksheets[3].Name);
			Assert.Equal("index", excelPackage.Workbook.Worksheets[2].Cells[1, 1].Value);
			Assert.Equal("value", excelPackage.Workbook.Worksheets[2].Cells[1, 2].Value);
			Assert.Equal("date", excelPackage.Workbook.Worksheets[2].Cells[1, 3].Value);
			Assert.Equal(1D, excelPackage.Workbook.Worksheets[2].Cells[2, 1].Value);
			Assert.Equal("s", excelPackage.Workbook.Worksheets[2].Cells[2, 2].Value);
			Assert.Equal(time, excelPackage.Workbook.Worksheets[2].Cells[2, 3].GetValue<DateTime>());

			Assert.Equal("Big2", excelPackage.Workbook.Worksheets[4].Name);
			Assert.Equal("index", excelPackage.Workbook.Worksheets[4].Cells[1, 1].Value);
			Assert.Equal("value", excelPackage.Workbook.Worksheets[4].Cells[1, 2].Value);
			Assert.Equal("date", excelPackage.Workbook.Worksheets[4].Cells[1, 3].Value);
			Assert.Equal(1048576D, excelPackage.Workbook.Worksheets[4].Cells[2, 1].Value);
			Assert.Equal("s", excelPackage.Workbook.Worksheets[4].Cells[2, 2].Value);
			Assert.Equal(time, excelPackage.Workbook.Worksheets[4].Cells[2, 3].GetValue<DateTime>());
			Assert.Equal(totalRow, excelPackage.Workbook.Worksheets[4].Cells[(int)totalRow + 1 - 1048575, 1].Value);
			Assert.Null(excelPackage.Workbook.Worksheets[4].Cells[(int)totalRow + 2 - 1048575, 1].Value);
		}
	}
}
