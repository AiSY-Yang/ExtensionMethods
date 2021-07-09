
using ExtensionMethods;

using System;
using System.Data;

using Xunit;

namespace ExtensionMethodsTests
{
	public class DataTableTest
	{
		[Fact]
		public void ToHtml()
		{
			DataTable dataTable = new DataTable();
			dataTable.Columns.Add("int");
			dataTable.Columns.Add("string");
			dataTable.Columns.Add("Datetime");
			dataTable.Columns.Add("null");
			dataTable.Rows.Add(1, "s", DateTime.MinValue, null);
			Assert.Equal(@"
<html>
	<head>
	</head>
	<body>
		<table border='1px' cellpadding='3' cellspacing='0' style='font-family:Garamond; font-size:smaller'>
			<tr>
				<td>int</td>
				<td>string</td>
				<td>Datetime</td>
				<td>null</td>
			</tr>
			<tr><td>1</td><td>s</td><td>0001/1/1 0:00:00</td><td></td></tr>
		</table>
	</body>
</html>", dataTable.ToHtml());
		}
		[Fact]
		public void ToInsertSQL()
		{
			DataTable dataTable = new DataTable();
			dataTable.Columns.Add("int", typeof(int));
			dataTable.Columns.Add("string", typeof(string));
			dataTable.Columns.Add("Datetime", typeof(DateTime));
			dataTable.Columns.Add("null");
			//表名为空
			Assert.Throws<ArgumentException>(() => dataTable.ToInsertSQL(true));
			Assert.Throws<ArgumentException>(() => dataTable.ToInsertSQL(true, true));
			Assert.Throws<ArgumentException>(() => dataTable.ToInsertSQL(true, false));
			Assert.Throws<ArgumentException>(() => dataTable.ToInsertSQL(false));
			Assert.Throws<ArgumentException>(() => dataTable.ToInsertSQL(false, true));
			Assert.Throws<ArgumentException>(() => dataTable.ToInsertSQL(false, false));
			dataTable.TableName = "TableName";

			//空表插入
			Assert.Equal(@"select 1", dataTable.ToInsertSQL(true));
			Assert.Equal(@"select 1", dataTable.ToInsertSQL(true, true));
			Assert.Equal(@"select 1", dataTable.ToInsertSQL(true, false));
			Assert.Equal(@"select 1", dataTable.ToInsertSQL(false));
			Assert.Equal(@"select 1", dataTable.ToInsertSQL(false, true));
			Assert.Equal(@"select 1", dataTable.ToInsertSQL(false, false));
			//单行插入
			dataTable.Rows.Add(1, "s", DateTime.MinValue, null);
			Assert.Equal(@"insert ignore into TableName value
(1,'s','0001-01-01 00:00:00',null)", dataTable.ToInsertSQL(false));
			Assert.Equal(@"insert ignore into TableName (int,string,Datetime,null) values
(1,'s','0001-01-01 00:00:00',null)", dataTable.ToInsertSQL(false, true));
			Assert.Equal(@"replace into TableName value
(1,'s','0001-01-01 00:00:00',null)", dataTable.ToInsertSQL(true));
			Assert.Equal(@"replace into TableName (int,string,Datetime,null) values
(1,'s','0001-01-01 00:00:00',null)", dataTable.ToInsertSQL(true, true));

			//多行插入
			dataTable.Rows.Add(2, "s", DateTime.MaxValue, null);
			Assert.Equal(@"insert ignore into TableName value
(1,'s','0001-01-01 00:00:00',null),
(2,'s','9999-12-31 23:59:59',null)", dataTable.ToInsertSQL(false));
			Assert.Equal(@"insert ignore into TableName (int,string,Datetime,null) values
(1,'s','0001-01-01 00:00:00',null),
(2,'s','9999-12-31 23:59:59',null)", dataTable.ToInsertSQL(false, true));
			Assert.Equal(@"replace into TableName value
(1,'s','0001-01-01 00:00:00',null),
(2,'s','9999-12-31 23:59:59',null)", dataTable.ToInsertSQL(true));
			Assert.Equal(@"replace into TableName (int,string,Datetime,null) values
(1,'s','0001-01-01 00:00:00',null),
(2,'s','9999-12-31 23:59:59',null)", dataTable.ToInsertSQL(true, true));
		}
	}
}
