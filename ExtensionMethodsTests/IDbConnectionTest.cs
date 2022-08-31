using ExtensionMethods;

using Moq;

using MySqlX.XDevAPI.Relational;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;

using Xunit;
using Xunit.Sdk;

namespace ExtensionMethodsTests
{
	public class IDbConnectionTest
	{
		public IDbConnectionTest()
		{
			SqlConnectionFactory.Init();
		}
		public static class SqlConnectionFactory
		{
			public static List<IDbConnection> ConnectionList => new List<IDbConnection>() {
				new MySql.Data.MySqlClient.MySqlConnection("Server=localhost;Database=UnitTest;user=root;password=123456"),
			};
			public static void Init()
			{
				foreach (var conn in ConnectionList)
				{
					conn.Open();
					conn.ExecCmd(@$"
drop table if exists CountTable;
create table CountTable
(
	column_1 int null,
	column_2 int null,
	column_3 int null
);
insert into CountTable (column_1, column_2, column_3) 
SELECT 1,1,1
union all 
SELECT 1,2,2
union all 
SELECT 1,2,3;
commit;
");
				}
			}
		}

		[Fact]
		public async void ExecRowCount()
		{
			foreach (var conn in SqlConnectionFactory.ConnectionList)
			{
				conn.Open();
				Assert.Equal(3, conn.ExecRowCount(@$"select * from CountTable"));
				Assert.Equal(0, conn.ExecRowCount(@$"select * from CountTable where column_1=@p", new Dictionary<string, object>() { { "p", 0 } }));
				Assert.Equal(2, conn.ExecRowCount(@$"select * from CountTable where column_2=@p", new string[] { "p" }, new object[] { 2 }));
				Assert.Equal(2, conn.ExecRowCount(@$"select * from CountTable where column_3 in (@p)", new Dictionary<string, object>() { { "p", new List<object>() { 1, 2 } } }));

				Assert.Equal(3, await conn.ExecRowCountAsync(@$"select * from CountTable"));
				Assert.Equal(0, await conn.ExecRowCountAsync(@$"select * from CountTable where column_1=@p", new Dictionary<string, object>() { { "p", 0 } }));
				Assert.Equal(2, await conn.ExecRowCountAsync(@$"select * from CountTable where column_2=@p", new string[] { "p" }, new object[] { 2 }));
				Assert.Equal(2, await conn.ExecRowCountAsync(@$"select * from CountTable where column_3 in (@p)", new Dictionary<string, object>() { { "p", new List<object>() { 1, 2 } } }));
			}
		}
	}
}
