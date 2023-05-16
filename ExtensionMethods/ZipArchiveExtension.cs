using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Text.Json.Serialization;

namespace ExtensionMethods
{
	/// <summary>
	/// ZipArchive Extension
	/// </summary>
	public static class ZipArchiveExtension
	{
		/// <summary>
		/// 从文件或者文件夹写入内容
		/// </summary>
		/// <param name="archive"></param>
		/// <param name="sourceName"></param>
		/// <param name="entryName"></param>
		public static void CreateEntryFromAny(this ZipArchive archive, string sourceName, string entryName = "")
		{
			var fileName = Path.GetFileName(sourceName);
			if (File.GetAttributes(sourceName).HasFlag(FileAttributes.Directory))
			{
				archive.CreateEntryFromDirectory(sourceName, Path.Combine(entryName, fileName));
			}
			else
			{
				archive.CreateEntryFromFile(sourceName, Path.Combine(entryName, fileName));
			}
		}
		/// <summary>
		/// 从目录写入内容
		/// </summary>
		/// <param name="archive"></param>
		/// <param name="sourceDirName"></param>
		/// <param name="entryName"></param>
		public static void CreateEntryFromDirectory(this ZipArchive archive, string sourceDirName, string entryName = "")
		{
			string[] files = Directory.GetFiles(sourceDirName).Concat(Directory.GetDirectories(sourceDirName)).ToArray();
			foreach (var file in files)
			{
				archive.CreateEntryFromAny(file, entryName);
			}
		}
	}
}
