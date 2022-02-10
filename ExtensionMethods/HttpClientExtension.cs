using System.Net.Http;
using System.Web;

namespace ExtensionMethods
{
	/// <summary>
	/// HttpClient扩展函数
	/// </summary>
	public static class HttpClientExtension
	{
		/// <summary>
		/// <inheritdoc cref="HttpClient.GetAsync(string)"/>
		/// </summary>
		/// <param name="httpClient"></param>
		/// <param name="requestUri"></param>
		/// <param name="collection"></param>
		/// <returns></returns>
		public static async System.Threading.Tasks.Task<HttpResponseMessage> GetAsync(this HttpClient httpClient, string requestUri, System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<string, string>> collection)
		{
			var query = HttpUtility.ParseQueryString("");
			foreach (var item in collection)
			{
				query[item.Key] = item.Value;
			}
			return await httpClient.GetAsync($"{requestUri}?{query}");
		}

	}
}
