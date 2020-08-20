using System.Collections.Generic;
using System.Linq;

namespace ExtendAllTheThings.Extensions
{
	public static class EnumerableExtensions
	{
		/// <summary>
		/// Usage: foreach(var item in myObject.MyNullableCollection.EmptyIfNull())
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source"></param>
		/// <returns></returns>
		public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T> source) => source ?? Enumerable.Empty<T>();
	}
}
