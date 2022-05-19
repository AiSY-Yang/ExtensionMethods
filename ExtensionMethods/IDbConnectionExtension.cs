using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtensionMethods
{
	/// <summary>
	/// 数据库连接,可通过begin开启事务
	/// <example>参数标识符采用@符号,in语句如下
	/// <code>
	/// sql="select * from dual where id in (@list)";</code>
	/// <code>Dictionary&lt;string,object&gt; dic={{"list",new int[]{1,2,3}}};</code>
	/// </example>
	/// </summary>
	public static class IDbConnectionExtension
	{
		/// <summary>
		/// IEnumerable参数转换为字典
		/// </summary>
		/// <param name="paramName">参数名</param>
		/// <param name="param">参数值</param>
		/// <returns></returns>
		private static Dictionary<string, object> ParamsToDictionary(IEnumerable<string> paramName, IEnumerable<object> param)
		{
			if (paramName.Count() != param.Count())
			{
				throw new ArgumentException("参数名称与参数列表数量不一致");
			}
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			var paramNameEnumerator = paramName.GetEnumerator();
			var paramEnumerator = param.GetEnumerator();
			while (paramNameEnumerator.MoveNext())
			{
				paramEnumerator.MoveNext();
				paramDict[paramNameEnumerator.Current] = paramEnumerator.Current;
			}
			return paramDict;
		}

		/// <summary>
		/// 按照SQL和参数字典生成Command对象
		/// </summary>
		/// <param name="dbConnection">数据库连接</param>
		/// <param name="sql">SQL语句</param>
		/// <param name="paramDict">参数字典</param>
		/// <returns></returns>
		public static System.Data.Common.DbCommand GenerateCmd(this IDbConnection dbConnection, string sql, Dictionary<string, object> paramDict)
		{
			using var cmd = dbConnection.CreateCommand();
			cmd.CommandText = sql;
			foreach (var item in paramDict)
			{
#if NET5_0_OR_GREATER
				if (item.Value is not string && item.Value is System.Collections.IEnumerable InList)
#else
				if (!(item.Value is string) && item.Value is System.Collections.IEnumerable InList)
#endif
				{
					var enumerator = InList.GetEnumerator();
					StringBuilder paramNameBuilder = new StringBuilder();
					int count = 0;
					while (enumerator.MoveNext())
					{
						string paramName = $"{item.Key}_{count:0000}";
						paramNameBuilder.Append(" @");
						paramNameBuilder.Append(paramName);
						paramNameBuilder.Append(',');
						var parameter = cmd.CreateParameter();
						parameter.ParameterName = paramName;
						parameter.Value = enumerator.Current;
						cmd.Parameters.Add(parameter);
						count++;
					}
					if (count == 0)
					{
						var parameter = cmd.CreateParameter();
						parameter.ParameterName = item.Key;
						parameter.Value = item.Value;
						cmd.Parameters.Add(parameter);
					}
					else
					{
						cmd.CommandText = cmd.CommandText.Replace("(@" + item.Key + ")", "(" + paramNameBuilder.ToString().TrimEnd(',') + ")");
					}
				}
				else
				{
					var parameter = cmd.CreateParameter();
					parameter.ParameterName = item.Key;
					parameter.Value = item.Value;
					cmd.Parameters.Add(parameter);
				}
			}
			return cmd as System.Data.Common.DbCommand;
		}

		#region 同步调用相关方法

		/// <inheritdoc cref="ExecRowCount(IDbConnection, string, Dictionary{string, object})"/>
		/// <param name="dbConnection">数据库连接</param>
		/// <param name="sql">SQL语句</param>
		public static long ExecRowCount(this IDbConnection dbConnection, string sql) => dbConnection.ExecRowCount(sql, new Dictionary<string, object>());

		/// <inheritdoc cref="ExecRowCount(IDbConnection, string, Dictionary{string, object})"/>
		/// <param name="dbConnection">数据库连接</param>
		/// <param name="sql">SQL语句</param>
		/// <param name="paramName">参数名称</param>
		/// <param name="param">参数值</param>
		public static long ExecRowCount(this IDbConnection dbConnection, string sql, IEnumerable<string> paramName, IEnumerable<object> param) => dbConnection.ExecRowCount(sql, ParamsToDictionary(paramName, param));

		/// <summary>
		/// 查询行数
		/// </summary>
		/// <param name="dbConnection">数据库连接</param>
		/// <param name="sql">SQL语句</param>
		/// <param name="paramDict">参数字典</param>
		/// <returns>行数</returns>
		public static long ExecRowCount(this IDbConnection dbConnection, string sql, Dictionary<string, object> paramDict)
		{
			using IDbCommand cmd = dbConnection.GenerateCmd($@"select count(*) from ({sql}) countTable", paramDict);
			return (long)cmd.ExecuteScalar();
		}

		/// <inheritdoc cref="ExecSQL(IDbConnection, string, Dictionary{string, object})"/>
		/// <param name="dbConnection">数据库连接</param>
		/// <param name="sql">SQL语句</param>
		public static DataTable ExecSQL(this IDbConnection dbConnection, string sql) => dbConnection.ExecSQL(sql, new Dictionary<string, object>());

		/// <inheritdoc cref="ExecSQL(IDbConnection, string, Dictionary{string, object})"/>
		/// <param name="dbConnection">数据库连接</param>
		/// <param name="sql">SQL语句</param>
		/// <param name="paramName">参数名称</param>
		/// <param name="param">参数值</param>
		public static DataTable ExecSQL(this IDbConnection dbConnection, string sql, IEnumerable<string> paramName, IEnumerable<object> param) => dbConnection.ExecSQL(sql, ParamsToDictionary(paramName, param));

		/// <summary>
		/// 查询数据
		/// </summary>
		/// <param name="dbConnection">数据库连接</param>
		/// <param name="sql">SQL语句</param>
		/// <param name="paramDict">参数字典</param>
		/// <returns>结果表</returns>
		public static DataTable ExecSQL(this IDbConnection dbConnection, string sql, Dictionary<string, object> paramDict)
		{
			using IDbCommand cmd = dbConnection.GenerateCmd(sql, paramDict);
			cmd.CommandTimeout = 60;
			using DataSet dataSet = new DataSet();
			dataSet.EnforceConstraints = false;
			dataSet.Tables.Add(new DataTable());
			dataSet.Tables[0].Load(cmd.ExecuteReader());
			return dataSet.Tables[0];
		}

		/// <inheritdoc cref="ExecCmd(IDbConnection, string, Dictionary{string, object})"/>
		/// <param name="dbConnection">数据库连接</param>
		/// <param name="sql">SQL语句</param>
		public static int ExecCmd(this IDbConnection dbConnection, string sql) => dbConnection.ExecCmd(sql, new Dictionary<string, object>());

		/// <inheritdoc cref="ExecCmd(IDbConnection, string, Dictionary{string, object})"/>
		/// <param name="dbConnection">数据库连接</param>
		/// <param name="sql">SQL语句</param>
		/// <param name="paramName">参数名称</param>
		/// <param name="param">参数值</param>
		public static int ExecCmd(this IDbConnection dbConnection, string sql, IEnumerable<string> paramName, IEnumerable<object> param) => dbConnection.ExecCmd(sql, ParamsToDictionary(paramName, param));

		/// <summary>
		/// 执行命令
		/// </summary>
		/// <param name="dbConnection">数据库连接</param>
		/// <param name="sql">SQL语句</param>
		/// <param name="paramDict">参数字典</param>
		/// <returns>修改行数</returns>
		public static int ExecCmd(this IDbConnection dbConnection, string sql, Dictionary<string, object> paramDict)
		{
			using IDbCommand cmd = dbConnection.GenerateCmd(sql, paramDict);
			return cmd.ExecuteNonQuery();
		}

		#endregion

		#region 异步调用相关方法

		/// <inheritdoc cref="ExecRowCountAsync(IDbConnection, string, Dictionary{string, object})"/>
		/// <param name="dbConnection">连接</param>
		/// <param name="sql">SQL语句</param>
		public static async Task<long> ExecRowCountAsync(this IDbConnection dbConnection, string sql) => await dbConnection.ExecRowCountAsync(sql, new Dictionary<string, object>());

		/// <inheritdoc cref="ExecRowCountAsync(IDbConnection, string, Dictionary{string, object})"/>
		/// <param name="dbConnection">连接</param>
		/// <param name="sql">SQL语句</param>
		/// <param name="paramName">参数名称</param>
		/// <param name="param">参数值</param>
		public static async Task<long> ExecRowCountAsync(this IDbConnection dbConnection, string sql, IEnumerable<string> paramName, IEnumerable<object> param) => await dbConnection.ExecRowCountAsync(sql, ParamsToDictionary(paramName, param));

		/// <summary>
		/// 异步查询行数
		/// </summary>
		/// <param name="dbConnection">连接</param>
		/// <param name="sql">SQL语句</param>
		/// <param name="paramDict">参数字典</param>
		/// <returns>行数</returns>
		public static async Task<long> ExecRowCountAsync(this IDbConnection dbConnection, string sql, Dictionary<string, object> paramDict)
		{
			using var cmd = dbConnection.GenerateCmd($@"select count(*) from ({sql}) countTable", paramDict);
			var result = await cmd.ExecuteScalarAsync();
			return (long)result;
		}

		/// <inheritdoc cref="ExecSQLAsync(IDbConnection, string, Dictionary{string, object})"/>
		/// <param name="dbConnection">连接</param>
		/// <param name="sql">SQL语句</param>
		public static async Task<DataTable> ExecSQLAsync(this IDbConnection dbConnection, string sql) => await dbConnection.ExecSQLAsync(sql, new Dictionary<string, object>());

		/// <inheritdoc cref="ExecSQLAsync(IDbConnection, string, Dictionary{string, object})"/>
		/// <param name="dbConnection">连接</param>
		/// <param name="sql">SQL语句</param>
		/// <param name="paramName">参数名称</param>
		/// <param name="param">参数值</param>
		public static async Task<DataTable> ExecSQLAsync(this IDbConnection dbConnection, string sql, IEnumerable<string> paramName, IEnumerable<object> param) => await dbConnection.ExecSQLAsync(sql, ParamsToDictionary(paramName, param));

		/// <summary>
		/// 异步查询数据
		/// </summary>
		/// <param name="dbConnection">连接</param>
		/// <param name="sql">SQL语句</param>
		/// <param name="paramDict">参数字典</param>
		/// <returns>结果表</returns>
		public static async Task<DataTable> ExecSQLAsync(this IDbConnection dbConnection, string sql, Dictionary<string, object> paramDict)
		{
			try
			{
				using var cmd = dbConnection.GenerateCmd(sql, paramDict);
				cmd.CommandTimeout = 60;
				DataSet dataSet = new DataSet();
				dataSet.EnforceConstraints = false;
				dataSet.Tables.Add(new DataTable());
				dataSet.Tables[0].Load(await cmd.ExecuteReaderAsync());
				return dataSet.Tables[0];
			}
			catch (Exception e)
			{
				Console.WriteLine("SQL错误" + sql + e.Message);
				throw;
			}
		}

		/// <inheritdoc cref="ExecCmdAsync(IDbConnection, string, Dictionary{string, object})"/>
		/// <param name="dbConnection">连接</param>
		/// <param name="sql">SQL语句</param>
		public static async Task<int> ExecCmdAsync(this IDbConnection dbConnection, string sql) => await dbConnection.ExecCmdAsync(sql, new Dictionary<string, object>());

		/// <inheritdoc cref="ExecCmdAsync(IDbConnection, string, Dictionary{string, object})"/>
		/// <param name="dbConnection">连接</param>
		/// <param name="sql">SQL语句</param>
		/// <param name="paramName">参数名称</param>
		/// <param name="param">参数值</param>
		public static async Task<int> ExecCmdAsync(this IDbConnection dbConnection, string sql, IEnumerable<string> paramName, IEnumerable<object> param) => await dbConnection.ExecCmdAsync(sql, ParamsToDictionary(paramName, param));

		/// <summary>
		/// 异步执行命令
		/// </summary>
		/// <param name="dbConnection">连接</param>
		/// <param name="sql">SQL语句</param>
		/// <param name="paramDict">参数字典</param>
		/// <returns>修改行数</returns>
		public static async Task<int> ExecCmdAsync(this IDbConnection dbConnection, string sql, Dictionary<string, object> paramDict)
		{
			try
			{
				using var cmd = dbConnection.GenerateCmd(sql, paramDict);
				return await cmd.ExecuteNonQueryAsync();
			}
			catch (Exception e)
			{
				Console.WriteLine("SQL错误" + sql + e.Message);
				throw;
			}
		}
		#endregion
	}
}
