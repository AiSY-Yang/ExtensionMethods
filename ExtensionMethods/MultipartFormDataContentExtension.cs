using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json.Serialization;

namespace ExtensionMethods
{
#if !NETCOREAPP3_1
	/// <summary>
	/// Form-data内容的扩展
	/// </summary>
	public static class MultipartFormDataContentExtension
	{
		/// <summary>
		/// 从json对象中填充当前form-data
		/// 支持FileStream,(string fileName,stream file),(stream file,string fileName)写入为文件
		/// </summary>
		/// <param name="content"></param>
		/// <param name="data"></param>
		public static void AddFromJsonObject(this MultipartFormDataContent content, object data)
		{
			var ps = data.GetType().GetProperties();
			foreach (var item in ps)
			{
				//排除需要忽略的属性
				var key = (item.GetCustomAttributes(typeof(JsonPropertyNameAttribute), true).FirstOrDefault() as JsonPropertyNameAttribute)?.Name;
				key ??= item.Name;
				if (item.GetCustomAttributes(typeof(JsonIgnoreAttribute), true).FirstOrDefault() is JsonIgnoreAttribute ignoreAttribute)
				{
					switch (ignoreAttribute.Condition)
					{
						case JsonIgnoreCondition.Never:
							AddContent();
							break;
						case JsonIgnoreCondition.Always:
							break;
						case JsonIgnoreCondition.WhenWritingDefault:
							if (!(Activator.CreateInstance(item.PropertyType) == item.GetValue(data)))
								AddContent();
							break;
						case JsonIgnoreCondition.WhenWritingNull:
							if (item.GetValue(data) != null)
								AddContent();
							break;
						default:
							break;
					}
				}
				else
				{
					AddContent();
				}
				async void AddContent()
				{
					byte[] buffer;
					switch (item.GetValue(data))
					{
						case null:
							content.Add(new ByteArrayContent(Array.Empty<byte>()), key);
							break;
						case byte[] array:
							content.Add(new ByteArrayContent(array), key);
							break;
						case FileStream file:
							buffer = new byte[file.Length];
							await file.ReadAsync(buffer);
							content.Add(new ByteArrayContent(buffer), key, file.Name);
							break;
						case Tuple<string, Stream> tuple:
							buffer = new byte[tuple.Item2.Length];
							await tuple.Item2.ReadAsync(buffer);
							content.Add(new ByteArrayContent(buffer), key, tuple.Item1);
							break;
						case Tuple<Stream, string> tuple:
							buffer = new byte[tuple.Item1.Length];
							await tuple.Item1.ReadAsync(buffer);
							content.Add(new ByteArrayContent(buffer), key, tuple.Item2);
							break;
						default:
							content.Add(new StringContent(item.GetValue(data)?.ToString() ?? ""), key);
							break;
					}
				}
			}
		}
	}
#endif
}
