using System;
using System.IO;
using System.Net;

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
		public static event Action<int> ShowDownloadPercent;
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
		/// 得到远程文件的长度
		/// </summary>
		/// <param name="url"></param>
		/// <returns></returns>
		public static long GetHttpLength(string url)
		{
			long length = 0;
			HttpWebRequest req = null;
			HttpWebResponse rsp = null;
			try
			{
				req = (HttpWebRequest)HttpWebRequest.Create(url);
				rsp = (HttpWebResponse)req.GetResponse();
				if (rsp.StatusCode == HttpStatusCode.OK)
					length = rsp.ContentLength;
			}
			catch (Exception)
			{
				throw;
			}
			finally
			{
				if (rsp != null)
					rsp.Close();
				if (req != null)
					req.Abort();
			}
			return length;
		}
		/// <summary>
		/// 从指定的网址下载文件
		/// </summary>
		/// <param name="f"></param>
		/// <param name="url"></param>
		/// <param name="cover">当文件已经存在时覆盖</param>
		/// <returns></returns>
		public static bool Download(this FileInfo f, string url, bool cover = true)
		{
			string localfileReal = f.FullName;
			string localfileWithSuffix = localfileReal + Suffix;
			try
			{
				long startPosition = 0;
				FileStream writeStream = null;
				if (string.IsNullOrEmpty(url) || string.IsNullOrEmpty(localfileReal))
					return false;

				//取得远程文件长度
				long remoteFileLength = GetHttpLength(url);
				if (remoteFileLength == 0)
					return false;
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

				HttpWebRequest req = null;
				HttpWebResponse rsp = null;
				try
				{
					req = (HttpWebRequest)HttpWebRequest.Create(url);
					if (startPosition > 0)
						req.AddRange((int)startPosition);

					rsp = (HttpWebResponse)req.GetResponse();
					using (Stream readStream = rsp.GetResponseStream())
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
					if (writeStream != null)
						writeStream.Close();
					if (rsp != null)
						rsp.Close();
					if (req != null)
						req.Abort();
					DownloadFileOk(localfileReal, localfileWithSuffix);
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
	}

}
