using System;
using System.Collections.Generic;
using System.Data;
using System.Text;


namespace ExtensionMethods
{
	/// <summary>
	/// 数据表扩展
	/// </summary>
	public static class DataTableExtension
	{
		/// <summary>
		/// 转换为简单的HTML,用于一些需要格式化输出的地方
		/// </summary>
		/// <param name="dataTable"></param>
		/// <returns></returns>
		public static string ToHtml(this DataTable dataTable)
		{
			StringBuilder strHTMLBuilder = new StringBuilder();

			strHTMLBuilder.Append("\r\n");
			strHTMLBuilder.Append("<html>\r\n");
			strHTMLBuilder.Append("\t<head>\r\n");
			strHTMLBuilder.Append("\t</head>\r\n");
			strHTMLBuilder.Append("\t<body>\r\n");
			strHTMLBuilder.Append("\t\t<table border='1px' cellpadding='3' cellspacing='0' style='font-family:Garamond; font-size:smaller'>\r\n");

			strHTMLBuilder.Append("\t\t\t<tr>\r\n");
			foreach (DataColumn? myColumn in dataTable.Columns)
			{
				strHTMLBuilder.Append("\t\t\t\t<td>");
				strHTMLBuilder.Append(myColumn!.ColumnName);
				strHTMLBuilder.Append("</td>\r\n");
			}
			strHTMLBuilder.Append("\t\t\t</tr>\r\n");
			foreach (DataRow? myRow in dataTable.Rows)
			{
				strHTMLBuilder.Append("\t\t\t<tr>");
				foreach (DataColumn? myColumn in dataTable.Columns)
				{
					strHTMLBuilder.Append("<td>");
					strHTMLBuilder.Append(myRow![myColumn!.ColumnName].ToString());
					strHTMLBuilder.Append("</td>");
				}
				strHTMLBuilder.Append("</tr>\r\n");
			}
			strHTMLBuilder.Append("\t\t</table>\r\n");
			strHTMLBuilder.Append("\t</body>\r\n");
			strHTMLBuilder.Append("</html>");
			string Htmltext = strHTMLBuilder.ToString();
			return Htmltext;
		}
		/// <summary>
		/// 返回MySQL的insert into语句,表名为DataTable的TableName字段,当表行数为0的时候返回"select 1"
		/// </summary>
		/// <param name="dataTable"></param>
		/// <param name="Replace"> <code>true</code>replace into DataTable.TableName<code>false</code>insert ignore into DataTable.TableName</param>
		/// <param name="IncludeColumnName">是否指定列名</param>
		/// <returns></returns>
		public static string ToInsertSQL(this DataTable dataTable, bool Replace, bool IncludeColumnName = false)
		{
			if (string.IsNullOrWhiteSpace(dataTable.TableName))
			{
				throw new ArgumentException("表名为空");
			}

			List<string> rows = new List<string>();
			if (dataTable.Rows.Count == 0)
			{
				return "select 1";
			}
#if NETCOREAPP3_1
			foreach (DataRow? row in dataTable.Rows)
			{
				if (row == null)
				{
					throw new ArgumentNullException();
				}
#else
			foreach (DataRow row in dataTable.Rows)
			{
#endif
				List<string> fields = new List<string>();
				foreach (var field in row.ItemArray)
				{
					switch (field)
					{
						case string str: fields.Add("'" + str + "'"); break;
						case DateTime time: fields.Add("'" + time.ToString("yyyy-MM-dd HH:mm:ss") + "'"); break;
						case DBNull _: fields.Add("null"); break;
						case null: fields.Add("null"); break;
						default: fields.Add(field.ToString() ?? "null"); break;
					}
				}
				rows.Add("(" + string.Join(",", fields) + ")");
			}
			if (IncludeColumnName)
			{
				List<string> cols = new List<string>();
#if NETCOREAPP3_1
				foreach (DataColumn? item in dataTable.Columns)
				{
					if (item == null)
					{
						throw new ArgumentNullException("datetable's column is null");
					}
					cols.Add(item.ColumnName);
				}
#else
				foreach (DataColumn item in dataTable.Columns)
				{
					cols.Add(item.ColumnName);
				}
#endif
				return $@"{(Replace ? "replace" : "insert ignore")} into {dataTable.TableName} ({string.Join(",", cols)}) values
{string.Join(",\r\n", rows)}";
			}
			else
			{
				return $@"{(Replace ? "replace" : "insert ignore")} into {dataTable.TableName} value
{string.Join(",\r\n", rows)}";
			}
		}
	}
}