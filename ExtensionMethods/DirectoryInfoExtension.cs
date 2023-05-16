namespace ExtensionMethods
{
	/// <summary>
	/// DirectoryInfoExtension
	/// </summary>
	public static class DirectoryInfoExtension
	{
		/// <summary>
		/// 获取目录大小
		/// <a href="https://stackoverflow.com/questions/468119/whats-the-best-way-to-calculate-the-size-of-a-directory-in-net"/>
		/// </summary>
		/// <param name="directoryInfo"></param>
		/// <param name="recursive"></param>
		/// <returns></returns>
		public static long GetSize(this System.IO.DirectoryInfo directoryInfo, bool recursive = true)
		{
			var startDirectorySize = default(long);
			if (directoryInfo == null || !directoryInfo.Exists)
				return startDirectorySize; //Return 0 while Directory does not exist.

			//Add size of files in the Current Directory to main size.
			foreach (var fileInfo in directoryInfo.GetFiles())
				System.Threading.Interlocked.Add(ref startDirectorySize, fileInfo.Length);

			if (recursive) //Loop on Sub Direcotries in the Current Directory and Calculate it's files size.
				System.Threading.Tasks.Parallel.ForEach(directoryInfo.GetDirectories(), (subDirectory) => System.Threading.Interlocked.Add(ref startDirectorySize, GetSize(subDirectory, recursive)));

			return startDirectorySize;  //Return full Size of this Directory.
		}
	}
}
