using System.IO;

using Microsoft.Extensions.FileProviders;

namespace ExtensionMethods
{
	/// <summary>
	/// PhysicalFileProvider的相关扩展
	/// </summary>
	public static class IFileProviderExtension
	{
		/// <summary>
		/// 获取指定的文件
		/// </summary>
		/// <param name="fileProvider"></param>
		/// <param name="pathAndFieName"></param>
		/// <returns></returns>
		public static IFileInfo GetFileInfo(this IFileProvider fileProvider, params string[] pathAndFieName)
		{
			return fileProvider.GetFileInfo(Path.Combine(pathAndFieName));
		}
	}
}
