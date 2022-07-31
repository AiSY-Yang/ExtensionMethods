using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ExtensionMethods
{
	/// <summary>
	/// Http客户端的扩展
	/// </summary>
	public static class IHttpClientBuilderExtension
	{
		/// <summary>
		/// 添加日志记录
		/// </summary>
		/// <param name="builder"></param>
		/// <param name="shouldLog"></param>
		/// <returns></returns>
		public static IHttpClientBuilder AddTraceLogHandler(this IHttpClientBuilder builder, Func<HttpResponseMessage, bool> shouldLog)
		{
			return builder.AddHttpMessageHandler((services) => new TraceLogHandler(services.GetRequiredService<IServiceProvider>(), shouldLog));
		}
		/// <summary>
		/// 添加日志记录
		/// </summary>
		/// <param name="builder"></param>
		/// <returns></returns>
		public static IHttpClientBuilder AddTraceLogHandler(this IHttpClientBuilder builder)
		{
			return builder.AddHttpMessageHandler((services) => new TraceLogHandler(services.GetRequiredService<IServiceProvider>(), (HttpResponseMessage) => { return true; }));
		}
		/// <summary>
		/// 记录日志的Handler
		/// </summary>
		internal class TraceLogHandler : DelegatingHandler
		{
			private readonly IServiceProvider serviceProvider;
			private readonly Func<HttpResponseMessage, bool> shouldLog;

			public TraceLogHandler(IServiceProvider serviceProvider, Func<HttpResponseMessage, bool> shouldLog)
			{
				this.serviceProvider = serviceProvider;
				this.shouldLog = shouldLog;
			}
			protected override async System.Threading.Tasks.Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
			{
				bool logPayloads = false;
				if (request.Headers.Contains("trace"))
				{
					logPayloads = true;
				}
				Dictionary<string, object> scope = new Dictionary<string, object>();
				scope.TryAdd("request_headers", request);
				if (request.Content != null)
				{
#if NET5_0_OR_GREATER
					scope.Add("request_body", await request.Content.ReadAsStringAsync(cancellationToken));
#else
					scope.Add("request_body", await request.Content.ReadAsStringAsync());
#endif
				}
				try
				{
					HttpResponseMessage response = await base.SendAsync(request, cancellationToken);
					scope.TryAdd("response_headers", response);
					if (response.Content != null)
					{
#if NET5_0_OR_GREATER
						scope.Add("response_body", await response.Content.ReadAsStringAsync(cancellationToken));
#else
						scope.Add("response_body", await response.Content.ReadAsStringAsync());
#endif
					}

					// We run the ShouldLog function that calculates, based on HttpResponseMessage, if we should log HttpClient request/response.
					logPayloads = logPayloads || shouldLog(response);
					return response;
				}
				catch (Exception)
				{
					// We want to log HttpClient request/response when some exception occurs, so we can reproduce what caused it.
					logPayloads = true;
					throw;
				}
				finally
				{
					// Finally, we check if we decided to log HttpClient request/response or not.
					// Only if we want to, we will have some allocations for the logger and try to read headers and contents.
					if (logPayloads)
					{
						var logger = serviceProvider.GetRequiredService<ILogger<TraceLogHandler>>();
						using (logger.BeginScope(scope))
						{
							logger.LogInformation("[HttpClientTrace]");
						}
					}
				}
			}
		}
	}
}