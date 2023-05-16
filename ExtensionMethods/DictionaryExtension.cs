using System.Collections.Generic;
using System.Linq;

namespace ExtensionMethods
{
	/// <summary>
	/// Dictionary's extension method
	/// </summary>
	public static class DictionaryExtension
	{
		/// <summary>
		/// Add or update key to the dictionary
		/// </summary>
		/// <typeparam name="Tkey"></typeparam>
		/// <typeparam name="Tvalue"></typeparam>
		/// <param name="dictionary"></param>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public static void AddOrUpdate<Tkey, Tvalue>(this Dictionary<Tkey, Tvalue> dictionary, Tkey key, Tvalue value) where Tkey : notnull => dictionary[key] = value;
		/// <summary>
		/// Clear the key reserve value and comparer of the dictionary
		/// </summary>
		/// <typeparam name="Tkey"></typeparam>
		/// <typeparam name="Tvalue"></typeparam>
		/// <param name="dictionary"></param>
		/// <returns></returns>
		public static Dictionary<Tkey, Tvalue> ClearValue<Tkey, Tvalue>(this Dictionary<Tkey, Tvalue> dictionary) where Tkey : notnull => dictionary.ToDictionary(x => x.Key, x => default(Tvalue), dictionary.Comparer)!;
	}
}
