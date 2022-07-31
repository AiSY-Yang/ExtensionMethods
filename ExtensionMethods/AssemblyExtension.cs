using System.Linq;
using System.Reflection;


namespace ExtensionMethods
{
	/// <summary>
	/// <c>byte</c>和<c>byte[]</c>的相关扩展
	/// </summary>
	public static class AssemblyExtension
	{
		/// <summary>
		/// 程序集XML信息缓存
		/// </summary>
		public static System.Collections.Generic.Dictionary<Assembly, System.Collections.Generic.List<Member>> AssemblyXmlCache { get; } = new System.Collections.Generic.Dictionary<Assembly, System.Collections.Generic.List<Member>>();

		/// <summary>
		/// 获取程序集的xml信息 要求xml文件与程序集在同一目录下且只有文件扩展名不一样 如果未找到文件则返回空列表
		/// </summary>
		/// <param name="assembly">要读取xml的程序集</param>
		/// <returns></returns>
		public static System.Collections.Generic.List<Member> GetXMLMember(this Assembly assembly)
		{
			//先从缓存读取 如果缓存里没有则查找文件
			if (AssemblyXmlCache.ContainsKey(assembly))
			{
				return AssemblyXmlCache[assembly];
			}
			else
			{
				//如果有报错的话则返回空的列表
				try
				{
					AssemblyXmlCache.Add(assembly, new System.Collections.Generic.List<Member>());
					var xml = assembly.Location.TrimEnd("dll") + "xml";
					using var streamReader = new System.IO.StreamReader(xml);
					var xmlDocument = new System.Xml.XmlDocument();
					xmlDocument.Load(streamReader);
					var members = xmlDocument?["doc"]?["members"];
					foreach (System.Xml.XmlNode item in members!.ChildNodes)
					{
						Member member = new Member()
						{
							ID = item.Attributes?["name"]?.Value!
						};
						foreach (System.Xml.XmlNode item2 in item.ChildNodes)
						{
							ContentNode node = new ContentNode()
							{
								Type = item2.Name,
								Name = item2.Attributes?["name"]?.Value,
								Content = item2.InnerXml.Trim()
							};
							member.Content.Add(node);
						};
						AssemblyXmlCache[assembly].Add(member);
					}
					return AssemblyXmlCache[assembly];
				}
				catch (System.Exception)
				{
					return new System.Collections.Generic.List<Member>();
				}
			}
		}
		/// <summary>
		/// 成员信息
		/// </summary>
		public class Member
		{
			/// <summary>
			/// ID 格式为Type:FullName
			/// <br></br>
			/// <a href="https://docs.microsoft.com/zh-cn/dotnet/csharp/language-reference/xmldoc/#id-strings">ID格式说明</a>
			/// </summary>
			public string ID { get; internal set; } = null!;
			/// <summary>
			/// 摘要
			/// </summary>
			public string? Summary { get => Content.FirstOrDefault(x => x.Type == "summary")?.Content; }
			/// <summary>
			/// 返回值
			/// </summary>
			public string? Return { get => Content.FirstOrDefault(x => x.Type == "returns")?.Content; }
			/// <summary>
			/// XML内容
			/// </summary>
			public System.Collections.Generic.List<ContentNode> Content { get; } = new System.Collections.Generic.List<ContentNode>();
		}
		/// <summary>
		/// 内容结点
		/// </summary>
		public class ContentNode
		{
			/// <summary>
			/// 类型
			/// <example>summary</example>
			/// <example>returns</example>
			/// <example>param</example>
			/// <example>etc..</example>
			/// </summary>
			public string Type { get; internal set; } = null!;
			/// <summary>
			/// 名称
			/// </summary>
			public string? Name { get; internal set; } = null!;
			/// <summary>
			/// 内容文本
			/// </summary>
			public string Content { get; internal set; } = null!;
		}
	}
}