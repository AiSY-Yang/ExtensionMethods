using System;
using System.IO;
using System.Net;
using System.Net.Http;

namespace ExtensionMethods
{
	/// <summary>
	/// 文件扩展
	/// </summary>
	public static class FileInfoExtension
	{
		/// <summary>
		/// 下载文件时缓存大小
		/// </summary>
		private static readonly int ByteSize = 1024;
		/// <summary>
		/// 下载中的后缀，下载完成去掉
		/// </summary>
		private const string Suffix = ".downloading";
		/// <summary>
		/// 下载进度
		/// </summary>
		public static event Action<int>? ShowDownloadPercent;
		/// <summary>
		/// 下载完成
		/// </summary>
		private static void DownloadFileOk(string localfileReal, string localfileWithSuffix)
		{
			try
			{
				//去掉.downloading后缀
				FileInfo fi = new FileInfo(localfileWithSuffix);
				fi.MoveTo(localfileReal);
			}
			catch (Exception)
			{
				throw;
			}
			finally
			{
				//通知完成
				ShowDownloadPercent?.Invoke(100);
			}
		}
		/// <summary>
		/// 从指定的网址下载文件
		/// </summary>
		/// <param name="f"></param>
		/// <param name="url"></param>
		/// <param name="cover">当文件已经存在时覆盖</param>
		/// <returns></returns>
		public static async System.Threading.Tasks.Task<bool> DownloadAsync(this FileInfo f, string url, bool cover = true)
		{
			string localfileReal = f.FullName;
			string localfileWithSuffix = localfileReal + Suffix;
			try
			{
				long startPosition = 0;
				FileStream? writeStream = null;
				if (string.IsNullOrEmpty(url))
					throw new ArgumentNullException(nameof(url), "the url is null or empty");
				if (string.IsNullOrEmpty(localfileReal))
					throw new ArgumentNullException(nameof(f), "the FileInfo's fileName is null or empty");

				//取得远程文件长度
				using var client = new HttpClient();
				var res = await client.GetAsync(url);
				res.EnsureSuccessStatusCode();
				long remoteFileLength = res.Content.Headers.ContentLength ?? throw new WebException("file length is null");
				if (remoteFileLength == 0)
					throw new WebException("file length is 0");

				if (File.Exists(localfileReal))
					if (cover == true)
					{
						File.Delete(f.FullName);
					}
				//判断文件是否正在下载
				if (File.Exists(localfileWithSuffix))
				{
					writeStream = File.OpenWrite(localfileWithSuffix);
					startPosition = writeStream.Length;
					if (startPosition > remoteFileLength)
					{
						writeStream.Close();
						File.Delete(localfileWithSuffix);
						writeStream = new FileStream(localfileWithSuffix, FileMode.Create);
					}
					else if (startPosition == remoteFileLength)
					{
						DownloadFileOk(localfileReal, localfileWithSuffix);
						writeStream.Close();
						return true;
					}
					else
						writeStream.Seek(startPosition, SeekOrigin.Begin);
				}
				else
					writeStream = new FileStream(localfileWithSuffix, FileMode.Create);

				try
				{
					if (startPosition > 0)
						client.DefaultRequestHeaders.Range = new System.Net.Http.Headers.RangeHeaderValue(startPosition, null);
					res = await client.GetAsync(url);

					using (Stream readStream = await res.Content.ReadAsStreamAsync())
					{
						byte[] btArray = new byte[ByteSize];
						long currPostion = startPosition;
						int contentSize = 0;
						while ((contentSize = readStream.Read(btArray, 0, btArray.Length)) > 0)
						{
							writeStream.Write(btArray, 0, contentSize);
							currPostion += contentSize;

							ShowDownloadPercent?.Invoke((int)(currPostion * 100 / remoteFileLength));
						}
					}
					return true;
				}
				catch (Exception)
				{
					throw;
				}
				finally
				{
					writeStream?.Close();
					DownloadFileOk(localfileReal, localfileWithSuffix);
				}
			}
			catch (Exception)
			{
				throw;
			}
			finally
			{

			}
		}
	}
}
