﻿using System;
using System.Linq;
using System.Security.Cryptography;

namespace ExtensionMethods.Common
{
	/// <summary>
	/// 通用组件
	/// </summary>
	public static partial class Tools
	{
		/// <summary>
		/// 验证码
		/// </summary>
		static class Captcha
		{
			/// <summary>
			/// 生成随机数字
			/// </summary>
			/// <param name="Length">验证码长度</param>
			/// <exception cref="ArgumentException"></exception>
			public static string GetNumber(int Length)
			{
				if (Length <= 0) throw new ArgumentException("The Length must be greater than 0");
				return RandomNumberGenerator.GetInt32((int)Math.Pow(10, Length)).ToString("0".Repeat(Length));
			}
			/// <summary>
			/// 生成随机大写字符验证码
			/// 已去掉易混淆的I和L
			/// </summary>
			/// <param name="Length">验证码长度</param>
			/// <returns></returns>
			/// <exception cref="ArgumentException"></exception>
			public static string GetChar(int Length)
			{
				if (Length <= 0) throw new ArgumentException("The Length must be greater than 0");
				var charList = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'J', 'K', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
				var result = new char[Length];
				for (int i = 0; i < Length; i++)
					result[i] = charList[RandomNumberGenerator.GetInt32(charList.Length)];
				return new string(result);
			}
			/// <summary>
			/// 生成随机大写字符和数字
			/// 已去掉易混淆的0,o,O,1,i,I,l,L,2,z,Z,5,s,S,6,b,8,B,9,g
			/// </summary>
			/// <param name="Length">验证码长度</param>
			/// <returns></returns>
			/// <exception cref="ArgumentException"></exception>
			public static string GetCharAndNumber(int Length)
			{
				if (Length <= 0) throw new ArgumentException("The Length must be greater than 0");
				var charList = new char[] { '3', '4', '7', 'A', 'C', 'D', 'E', 'F', 'G', 'H', 'J', 'K', 'M', 'N', 'P', 'Q', 'R', 'T', 'U', 'V', 'W', 'X', 'Y', };
				var result = new char[Length];
				for (int i = 0; i < Length; i++)
					result[i] = charList[RandomNumberGenerator.GetInt32(charList.Length)];
				return new string(result);
			}
		}
		/// <summary>
		/// 获取MIME类型,可以传入文件名或者文件扩展名
		/// </summary>
		/// <param name="Name">文件名或者扩展名</param>
		/// <returns></returns>
		public static string GetMIME(string Name)
		{
			var extensionName = System.IO.Path.GetExtension(Name);
			extensionName = extensionName.IsNullOrWhiteSpace() ? Name.StartsWith('.') ? Name[1..] : Name : extensionName[1..];
			return MIME.TryGetValue(extensionName, out string? value) ? value : "application/octet-stream";
		}

		private static readonly System.Collections.Generic.Dictionary<string, string> MIME = new System.Collections.Generic.Dictionary<string, string>
  {
	{"ai", "application/postscript"},
	{"aif", "audio/x-aiff"},
	{"aifc", "audio/x-aiff"},
	{"aiff", "audio/x-aiff"},
	{"asc", "text/plain"},
	{"atom", "application/atom+xml"},
	{"au", "audio/basic"},
	{"avi", "video/x-msvideo"},
	{"bcpio", "application/x-bcpio"},
	{"bin", "application/octet-stream"},
	{"bmp", "image/bmp"},
	{"cdf", "application/x-netcdf"},
	{"cgm", "image/cgm"},
	{"class", "application/octet-stream"},
	{"cpio", "application/x-cpio"},
	{"cpt", "application/mac-compactpro"},
	{"csh", "application/x-csh"},
	{"css", "text/css"},
	{"dcr", "application/x-director"},
	{"dif", "video/x-dv"},
	{"dir", "application/x-director"},
	{"djv", "image/vnd.djvu"},
	{"djvu", "image/vnd.djvu"},
	{"dll", "application/octet-stream"},
	{"dmg", "application/octet-stream"},
	{"dms", "application/octet-stream"},
	{"doc", "application/msword"},
	{"docx","application/vnd.openxmlformats-officedocument.wordprocessingml.document"},
	{"dotx", "application/vnd.openxmlformats-officedocument.wordprocessingml.template"},
	{"docm","application/vnd.ms-word.document.macroEnabled.12"},
	{"dotm","application/vnd.ms-word.template.macroEnabled.12"},
	{"dtd", "application/xml-dtd"},
	{"dv", "video/x-dv"},
	{"dvi", "application/x-dvi"},
	{"dxr", "application/x-director"},
	{"eps", "application/postscript"},
	{"etx", "text/x-setext"},
	{"exe", "application/octet-stream"},
	{"ez", "application/andrew-inset"},
	{"gif", "image/gif"},
	{"gram", "application/srgs"},
	{"grxml", "application/srgs+xml"},
	{"gtar", "application/x-gtar"},
	{"hdf", "application/x-hdf"},
	{"hqx", "application/mac-binhex40"},
	{"htm", "text/html"},
	{"html", "text/html"},
	{"ice", "x-conference/x-cooltalk"},
	{"ico", "image/x-icon"},
	{"ics", "text/calendar"},
	{"ief", "image/ief"},
	{"ifb", "text/calendar"},
	{"iges", "model/iges"},
	{"igs", "model/iges"},
	{"jnlp", "application/x-java-jnlp-file"},
	{"jp2", "image/jp2"},
	{"jpe", "image/jpeg"},
	{"jpeg", "image/jpeg"},
	{"jpg", "image/jpeg"},
	{"js", "application/javascript"},
	{"kar", "audio/midi"},
	{"latex", "application/x-latex"},
	{"lha", "application/octet-stream"},
	{"lzh", "application/octet-stream"},
	{"m3u", "audio/x-mpegurl"},
	{"m4a", "audio/mp4a-latm"},
	{"m4b", "audio/mp4a-latm"},
	{"m4p", "audio/mp4a-latm"},
	{"m4u", "video/vnd.mpegurl"},
	{"m4v", "video/x-m4v"},
	{"mac", "image/x-macpaint"},
	{"md", "text/markdown"},
	{"man", "application/x-troff-man"},
	{"mathml", "application/mathml+xml"},
	{"me", "application/x-troff-me"},
	{"mesh", "model/mesh"},
	{"mid", "audio/midi"},
	{"midi", "audio/midi"},
	{"mif", "application/vnd.mif"},
	{"mov", "video/quicktime"},
	{"movie", "video/x-sgi-movie"},
	{"mp2", "audio/mpeg"},
	{"mp3", "audio/mpeg"},
	{"mp4", "video/mp4"},
	{"mpe", "video/mpeg"},
	{"mpeg", "video/mpeg"},
	{"mpg", "video/mpeg"},
	{"mpga", "audio/mpeg"},
	{"ms", "application/x-troff-ms"},
	{"msh", "model/mesh"},
	{"mxu", "video/vnd.mpegurl"},
	{"nc", "application/x-netcdf"},
	{"oda", "application/oda"},
	{"ogg", "application/ogg"},
	{"pbm", "image/x-portable-bitmap"},
	{"pct", "image/pict"},
	{"pdb", "chemical/x-pdb"},
	{"pdf", "application/pdf"},
	{"pgm", "image/x-portable-graymap"},
	{"pgn", "application/x-chess-pgn"},
	{"pic", "image/pict"},
	{"pict", "image/pict"},
	{"png", "image/png"},
	{"pnm", "image/x-portable-anymap"},
	{"pnt", "image/x-macpaint"},
	{"pntg", "image/x-macpaint"},
	{"ppm", "image/x-portable-pixmap"},
	{"ppt", "application/vnd.ms-powerpoint"},
	{"pptx","application/vnd.openxmlformats-officedocument.presentationml.presentation"},
	{"potx","application/vnd.openxmlformats-officedocument.presentationml.template"},
	{"ppsx","application/vnd.openxmlformats-officedocument.presentationml.slideshow"},
	{"ppam","application/vnd.ms-powerpoint.addin.macroEnabled.12"},
	{"pptm","application/vnd.ms-powerpoint.presentation.macroEnabled.12"},
	{"potm","application/vnd.ms-powerpoint.template.macroEnabled.12"},
	{"ppsm","application/vnd.ms-powerpoint.slideshow.macroEnabled.12"},
	{"ps", "application/postscript"},
	{"qt", "video/quicktime"},
	{"qti", "image/x-quicktime"},
	{"qtif", "image/x-quicktime"},
	{"ra", "audio/x-pn-realaudio"},
	{"ram", "audio/x-pn-realaudio"},
	{"ras", "image/x-cmu-raster"},
	{"rdf", "application/rdf+xml"},
	{"rgb", "image/x-rgb"},
	{"rm", "application/vnd.rn-realmedia"},
	{"roff", "application/x-troff"},
	{"rtf", "text/rtf"},
	{"rtx", "text/richtext"},
	{"sgm", "text/sgml"},
	{"sgml", "text/sgml"},
	{"sh", "application/x-sh"},
	{"shar", "application/x-shar"},
	{"silo", "model/mesh"},
	{"sit", "application/x-stuffit"},
	{"skd", "application/x-koan"},
	{"skm", "application/x-koan"},
	{"skp", "application/x-koan"},
	{"skt", "application/x-koan"},
	{"smi", "application/smil"},
	{"smil", "application/smil"},
	{"snd", "audio/basic"},
	{"so", "application/octet-stream"},
	{"spl", "application/x-futuresplash"},
	{"src", "application/x-wais-source"},
	{"sv4cpio", "application/x-sv4cpio"},
	{"sv4crc", "application/x-sv4crc"},
	{"svg", "image/svg+xml"},
	{"swf", "application/x-shockwave-flash"},
	{"t", "application/x-troff"},
	{"tar", "application/x-tar"},
	{"tcl", "application/x-tcl"},
	{"tex", "application/x-tex"},
	{"texi", "application/x-texinfo"},
	{"texinfo", "application/x-texinfo"},
	{"tif", "image/tiff"},
	{"tiff", "image/tiff"},
	{"tr", "application/x-troff"},
	{"tsv", "text/tab-separated-values"},
	{"txt", "text/plain"},
	{"ustar", "application/x-ustar"},
	{"vcd", "application/x-cdlink"},
	{"vrml", "model/vrml"},
	{"vxml", "application/voicexml+xml"},
	{"wav", "audio/x-wav"},
	{"wbmp", "image/vnd.wap.wbmp"},
	{"wbmxl", "application/vnd.wap.wbxml"},
	{"wml", "text/vnd.wap.wml"},
	{"wmlc", "application/vnd.wap.wmlc"},
	{"wmls", "text/vnd.wap.wmlscript"},
	{"wmlsc", "application/vnd.wap.wmlscriptc"},
	{"wrl", "model/vrml"},
	{"xbm", "image/x-xbitmap"},
	{"xht", "application/xhtml+xml"},
	{"xhtml", "application/xhtml+xml"},
	{"xls", "application/vnd.ms-excel"},
	{"xml", "application/xml"},
	{"xpm", "image/x-xpixmap"},
	{"xsl", "application/xml"},
	{"xlsx","application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
	{"xltx","application/vnd.openxmlformats-officedocument.spreadsheetml.template"},
	{"xlsm","application/vnd.ms-excel.sheet.macroEnabled.12"},
	{"xltm","application/vnd.ms-excel.template.macroEnabled.12"},
	{"xlam","application/vnd.ms-excel.addin.macroEnabled.12"},
	{"xlsb","application/vnd.ms-excel.sheet.binary.macroEnabled.12"},
	{"xslt", "application/xslt+xml"},
	{"xul", "application/vnd.mozilla.xul+xml"},
	{"xwd", "image/x-xwindowdump"},
	{"xyz", "chemical/x-xyz"},
	{"zip", "application/zip"}
  };
	}
}
