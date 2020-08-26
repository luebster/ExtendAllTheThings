using System;
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

		public static IEnumerable<SomeType> PickSomeInRandomOrder<SomeType>(this IEnumerable<SomeType> someTypes, int maxCount)
		{
			Random random = new Random(DateTime.Now.Millisecond);

			Dictionary<double, SomeType> randomSortTable = new Dictionary<double, SomeType>();

			foreach (SomeType someType in someTypes)
				randomSortTable[random.NextDouble()] = someType;

			return randomSortTable.OrderBy(KVP => KVP.Key).Take(maxCount).Select(KVP => KVP.Value);
		}
	}
}
