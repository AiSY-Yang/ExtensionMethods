using System.Collections.Generic;

namespace ExtensionMethods
{
	/// <summary>
	/// 
	/// </summary>
	public static class DictionaryExtension
	{
		/// <summary>
		/// Add or update key to the dictionary
		/// </summary>
		/// <typeparam name="Tkey"></typeparam>
		/// <typeparam name="Tvalue"></typeparam>
		/// <param name="dict"></param>
		/// <param name="tkey"></param>
		/// <param name="tvalue"></param>
		public static void AddOrUpdate<Tkey, Tvalue>(this Dictionary<Tkey, Tvalue> dict, Tkey tkey, Tvalue tvalue)
		{
			dict[tkey] = tvalue;
		}
	}
}
