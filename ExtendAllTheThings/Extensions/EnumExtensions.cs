using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace ExtendAllTheThings.Extensions
{
	public static class EnumExtensions
	{
		/// <summary>
		/// When an enum value is decorated with a Display attribute, this method will return the Name property
		/// </summary>
		/// <param name="enumValue"></param>
		/// <returns></returns>
		public static string GetDisplayName(this Enum enumValue)
		{
			return enumValue.GetType()?
		 .GetMember(enumValue.ToString())?[0]?
		 .GetCustomAttribute<DisplayAttribute>()?
		 .Name;
		}

		public static IEnumerable<Enum> ToEnumerable(this Enum input)
		{
			foreach (Enum value in Enum.GetValues(input.GetType()))
			{
				if (input.HasFlag(value) && Convert.ToInt64(value) != 0)
				{
					yield return value;
				}
			}
		}

		public static IEnumerable<int> ToEnumerableInt(this Enum input)
		{
			return input.ToEnumerable().Select(x => Convert.ToInt32(x));
		}

		public static T ToEnum<T>(this string value, bool ignoreCase = true)
		{
			return (T)Enum.Parse(typeof(T), value, ignoreCase);
		}
	}
}
