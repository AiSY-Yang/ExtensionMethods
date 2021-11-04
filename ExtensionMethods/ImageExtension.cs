using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace ExtensionMethods
{
	/// <summary>
	/// Image扩展
	/// </summary>
	public static class ImageExtension
	{

		/// <summary>
		/// 转换到base64编码
		/// </summary>
		/// <param name="image"></param>
		/// <returns></returns>
		public static string ToBase64(this Bitmap image)
		{
			System.IO.MemoryStream ms = new System.IO.MemoryStream();
			image.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
			byte[] arr = new byte[ms.Length];
			ms.Position = 0;
			ms.Read(arr, 0, (int)ms.Length);
			ms.Close();
			return Convert.ToBase64String(arr);
		}
		/// <summary>
		/// 转换到base64编码
		/// </summary>
		/// <param name="image"></param>
		/// <param name="format">图片格式</param>
		/// <returns></returns>
		public static string ToBase64(this Image image, ImageFormat format)
		{
			System.IO.MemoryStream ms = new System.IO.MemoryStream();
			image.Save(ms, format);
			byte[] arr = new byte[ms.Length];
			ms.Position = 0;
			ms.Read(arr, 0, (int)ms.Length);
			ms.Close();
			return Convert.ToBase64String(arr);
		}
		/// <summary>
		/// base64 转换为一个Image并替换当前对象
		/// </summary>
		/// <param name="image"></param>
		/// <param name="base64"></param>
		/// <returns>当前对象</returns>
		public static Image Base64ToImage(this Image image, string base64)
		{
			base64 = base64
				.Replace("data:image/png;base64,", "")
				.Replace("data:image/jpg;base64,", "")
				.Replace("data:image/jpeg;base64,", "");//将base64头部信息替换
			byte[] bytes = Convert.FromBase64String(base64);
			System.IO.MemoryStream memStream = new System.IO.MemoryStream(bytes);
			image = Image.FromStream(memStream);
			return image;
		}
	}

}