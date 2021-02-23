using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace ExtendAllTheThings.Extensions
{
	public static class StringExtensions
	{
		#region Common string extensions

		/// <summary>
		/// 	Determines whether the specified string is null or empty.
		/// </summary>
		/// <param name = "value">The string value to check.</param>
		public static bool IsNull(this string value)
		{
			return string.IsNullOrEmpty(value);
		}

		/// <summary>
		/// 	Determines whether the specified string is not null or empty.
		/// </summary>
		/// <param name = "value">The string value to check.</param>
		public static bool IsNotNull(this string value)
		{
			return !value.IsNull();
		}

		/// <summary>
		/// Get the first not null/empty string
		/// </summary>
		/// <param name="str"></param>
		/// <param name="strings"></param>
		/// <returns></returns>
		public static string Coalesce(this string str, params string[] strings)
		{
			return (new[] { str })
					.Concat(strings)
					.FirstOrDefault(s => !string.IsNullOrEmpty(s));
		}

		/// <summary>
		/// 	Formats the value with the parameters using string.Format.
		/// </summary>
		/// <param name = "value">The input string.</param>
		/// <param name = "parameters">The parameters.</param>
		/// <returns></returns>
		public static string FormatWith(this string value, params object[] parameters)
		{
			return string.Format(value, parameters);
		}

		/// <summary>
		/// 	Trims the text to a provided maximum length.
		/// </summary>
		/// <param name = "value">The input string.</param>
		/// <param name = "maxLength">Maximum length.</param>
		/// <returns></returns>
		public static string TrimToMaxLength(this string value, int maxLength)
		{
			return value == null || value.Length <= maxLength ? value : value[0..];
		}

		/// <summary>
		/// 	Trims the text to a provided maximum length and adds a suffix if required.
		/// </summary>
		/// <param name = "value">The input string.</param>
		/// <param name = "maxLength">Maximum length.</param>
		/// <param name = "suffix">The suffix.</param>
		/// <returns></returns>
		public static string TrimToMaxLength(this string value, int maxLength, string suffix)
		{
			return value == null || value.Length <= maxLength ? value : string.Concat(value[0..], suffix);
		}

		/// <summary>
		/// 	Determines whether the comparison value string is contained within the input value string
		/// </summary>
		/// <param name = "inputValue">The input value.</param>
		/// <param name = "comparisonValue">The comparison value.</param>
		/// <param name = "comparisonType">Type of the comparison to allow case sensitive or insensitive comparison.</param>
		/// <returns>
		/// 	<c>true</c> if input value contains the specified value, otherwise, <c>false</c>.
		/// </returns>
		public static bool Contains(this string inputValue, string comparisonValue, StringComparison comparisonType)
		{
			return inputValue.IndexOf(comparisonValue, comparisonType) != -1;
		}

		/// <summary>
		/// Centers a charters in this string, padding in both, left and right, by specified Unicode character,
		/// for a specified total length.
		/// </summary>
		/// <param name="value">Instance value.</param>
		/// <param name="width">The number of characters in the resulting string,
		/// equal to the number of original characters plus any additional padding characters.
		/// </param>
		/// <param name="padChar">A Unicode padding character.</param>
		/// <param name="truncate">Should get only the substring of specified width if string width is
		/// more than the specified width.</param>
		/// <returns>A new string that is equivalent to this instance,
		/// but center-aligned with as many paddingChar characters as needed to create a
		/// length of width parameter.</returns>
		public static string PadBoth(this string value, int width, char padChar, bool truncate = false)
		{
			int diff = width - value.Length;
			if (diff == 0 || (diff < 0 && !truncate))
			{
				return value;
			}
			else if (diff < 0)
			{
				return value[0..];
			}
			else
			{
				return value.PadLeft(width - (diff / 2), padChar).PadRight(width, padChar);
			}
		}

		/// <summary>
		/// 	Reverses / mirrors a string.
		/// </summary>
		/// <param name = "value">The string to be reversed.</param>
		/// <returns>The reversed string</returns>
		public static string Reverse(this string value)
		{
			if (value.IsNull() || (value.Length == 1))
			{
				return value;
			}

			char[] chars = value.ToCharArray();
			Array.Reverse(chars);
			return new string(chars);
		}

		/// <summary>
		/// 	Ensures that a string starts with a given prefix.
		/// </summary>
		/// <param name = "value">The string value to check.</param>
		/// <param name = "prefix">The prefix value to check for.</param>
		/// <returns>The string value including the prefix</returns>
		/// <example>
		/// 	<code>
		/// 		var extension = "txt";
		/// 		var fileName = string.Concat(file.Name, extension.EnsureStartsWith("."));
		/// 	</code>
		/// </example>
		public static string EnsureStartsWith(this string value, string prefix)
		{
			return value.StartsWith(prefix) ? value : string.Concat(prefix, value);
		}

		/// <summary>
		/// 	Ensures that a string ends with a given suffix.
		/// </summary>
		/// <param name = "value">The string value to check.</param>
		/// <param name = "suffix">The suffix value to check for.</param>
		/// <returns>The string value including the suffix</returns>
		/// <example>
		/// 	<code>
		/// 		var url = "http://www.pgk.de";
		/// 		url = url.EnsureEndsWith("/"));
		/// 	</code>
		/// </example>
		public static string EnsureEndsWith(this string value, string suffix)
		{
			return value.EndsWith(suffix) ? value : string.Concat(value, suffix);
		}

		/// <summary>
		/// 	Repeats the specified string value as provided by the repeat count.
		/// </summary>
		/// <param name = "value">The original string.</param>
		/// <param name = "repeatCount">The repeat count.</param>
		/// <returns>The repeated string</returns>
		public static string Repeat(this string value, int repeatCount)
		{
			if (value.Length == 1)
			{
				return new string(value[0], repeatCount);
			}

			StringBuilder sb = new StringBuilder(repeatCount * value.Length);
			while (repeatCount-- > 0)
			{
				sb.Append(value);
			}

			return sb.ToString();
		}

		/// <summary>
		/// 	Tests whether the contents of a string is a numeric value
		/// </summary>
		/// <param name = "value">String to check</param>
		/// <returns>
		/// 	Boolean indicating whether or not the string contents are numeric
		/// </returns>
		public static bool IsNumeric(this string value)
		{
			return float.TryParse(value, out _);
		}

		#region Extract

		/// <summary>
		/// 	Extracts all digits from a string.
		/// </summary>
		/// <param name = "value">String containing digits to extract</param>
		/// <returns>
		/// 	All digits contained within the input string
		/// </returns>
		/// <remarks>
		/// 	Contributed by Kenneth Scott
		/// </remarks>
		public static string ExtractDigits(this string value)
		{
			return new string(value.Where(p => char.IsDigit(p)).ToArray());
		}

		#endregion Extract

		/// <summary>
		/// 	Concatenates the specified string value with the passed additional strings.
		/// </summary>
		/// <param name = "value">The original value.</param>
		/// <param name = "values">The additional string values to be concatenated.</param>
		/// <returns>The concatenated string.</returns>
		public static string ConcatWith(this string value, params string[] values)
		{
			return string.Concat(value, string.Concat(values));
		}

		/// <summary>
		/// 	Convert the provided string to a Guid value.
		/// </summary>
		/// <param name = "value">The original string value.</param>
		/// <returns>The Guid</returns>
		public static Guid ToGuid(this string value)
		{
			return new Guid(value);
		}

		/// <summary>
		/// 	Convert the provided string to a Guid value and returns Guid.Empty if conversion fails.
		/// </summary>
		/// <param name = "value">The original string value.</param>
		/// <returns>The Guid</returns>
		public static Guid ToGuidSave(this string value)
		{
			return value.ToGuidSave(Guid.Empty);
		}

		/// <summary>
		/// 	Convert the provided string to a Guid value and returns the provided default value if the conversion fails.
		/// </summary>
		/// <param name = "value">The original string value.</param>
		/// <param name = "defaultValue">The default value.</param>
		/// <returns>The Guid</returns>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "<Pending>")]
		public static Guid ToGuidSave(this string value, Guid defaultValue)
		{
			if (value.IsNull())
			{
				return defaultValue;
			}

			try
			{
				return value.ToGuid();
			}
			catch { return defaultValue; }
		}

		/// <summary>
		/// 	Gets the string before the given string parameter.
		/// </summary>
		/// <param name = "value">The default value.</param>
		/// <param name = "x">The given string parameter.</param>
		/// <returns></returns>
		/// <remarks>Unlike GetBetween and GetAfter, this does not Trim the result.</remarks>
		public static string GetBefore(this string value, string x)
		{
			int xPos = value.IndexOf(x);
			return xPos == -1 ? string.Empty : value[0..];
		}

		/// <summary>
		/// 	Gets the string between the given string parameters.
		/// </summary>
		/// <param name = "value">The source value.</param>
		/// <param name = "x">The left string sentinel.</param>
		/// <param name = "y">The right string sentinel</param>
		/// <returns></returns>
		/// <remarks>Unlike GetBefore, this method trims the result</remarks>
		public static string GetBetween(this string value, string x, string y)
		{
			int xPos = value.IndexOf(x);
			int yPos = value.LastIndexOf(y);

			if (xPos == -1 || xPos == -1)
			{
				return string.Empty;
			}

			int startIndex = xPos + x.Length;
			return startIndex >= yPos ? string.Empty : value[startIndex..yPos].Trim();
		}

		/// <summary>
		/// 	Gets the string after the given string parameter.
		/// </summary>
		/// <param name = "value">The default value.</param>
		/// <param name = "x">The given string parameter.</param>
		/// <returns></returns>
		/// <remarks>Unlike GetBefore, this method trims the result</remarks>
		public static string GetAfter(this string value, string x)
		{
			int xPos = value.LastIndexOf(x);

			if (xPos == -1)
			{
				return string.Empty;
			}

			int startIndex = xPos + x.Length;
			return startIndex >= value.Length ? string.Empty : value[startIndex..].Trim();
		}

		/// <summary>
		/// 	A generic version of System.String.Join()
		/// </summary>
		/// <typeparam name = "T">
		/// 	The type of the array to join
		/// </typeparam>
		/// <param name = "value">
		/// 	An array of values
		/// </param>
		/// <param name = "separator">
		/// 	The separator to appear between each element
		/// </param>
		/// <returns>
		/// 	The join.
		/// </returns>
		public static string Join<T>(this T[] value, string separator)
		{
			if (value == null || value.Length == 0)
			{
				return string.Empty;
			}

			if (separator == null)
			{
				separator = string.Empty;
			}

			static string converter(T o) => o.ToString();
			return string.Join(separator, Array.ConvertAll(value, converter));
		}

		/// <summary>
		/// 	Remove any instance of the given character from the current string.
		/// </summary>
		/// <param name = "value">
		/// 	The input.
		/// </param>
		/// <param name = "removeCharc">
		/// 	The remove char.
		/// </param>
		public static string Remove(this string value, params char[] removeCharc)
		{
			string result = value;
			if (!string.IsNullOrEmpty(result) && removeCharc != null)
			{
				Array.ForEach(removeCharc, c => result = result.Remove(c.ToString()));
			}

			return result;
		}

		/// <summary>
		/// Remove any instance of the given string pattern from the current string.
		/// </summary>
		/// <param name="value">The input.</param>
		/// <param name="strings">The strings.</param>
		/// <returns></returns>
		public static string Remove(this string value, params string[] strings)
		{
			return strings.Aggregate(value, (current, c) => current.Replace(c, string.Empty));
		}

		/// <summary>Finds out if the specified string contains null, empty or consists only of white-space characters</summary>
		/// <param name = "value">The input string</param>
		public static bool IsEmptyOrWhiteSpace(this string value)
		{
			return value.IsNull() || value.All(t => char.IsWhiteSpace(t));
		}

		/// <summary>Determines whether the specified string is not null, empty or consists only of white-space characters</summary>
		/// <param name = "value">The string value to check</param>
		public static bool IsNotEmptyOrWhiteSpace(this string value)
		{
			return !value.IsEmptyOrWhiteSpace();
		}

		/// <summary>Checks whether the string is null, empty or consists only of white-space characters and returns a default value in case</summary>
		/// <param name = "value">The string to check</param>
		/// <param name = "defaultValue">The default value</param>
		/// <returns>Either the string or the default value</returns>
		public static string IfEmptyOrWhiteSpace(this string value, string defaultValue)
		{
			return value.IsEmptyOrWhiteSpace() ? defaultValue : value;
		}

		/// <summary>Uppercase First Letter</summary>
		/// <param name = "value">The string value to process</param>
		public static string ToUpperFirstLetter(this string value)
		{
			if (value.IsEmptyOrWhiteSpace())
			{
				return string.Empty;
			}

			char[] valueChars = value.ToCharArray();
			valueChars[0] = char.ToUpper(valueChars[0]);

			return new string(valueChars);
		}

		/// <summary>
		/// Returns the left part of the string.
		/// </summary>
		/// <param name="value">The original string.</param>
		/// <param name="characterCount">The character count to be returned.</param>
		/// <returns>The left part</returns>
		public static string Left(this string value, int characterCount)
		{
			if (value == null)
			{
				throw new ArgumentNullException(nameof(value));
			}

			if (characterCount >= value.Length)
			{
				throw new ArgumentOutOfRangeException(nameof(characterCount), characterCount, "characterCount must be less than length of string");
			}

			return value[0..];
		}

		/// <summary>
		/// Returns the Right part of the string.
		/// </summary>
		/// <param name="value">The original string.</param>
		/// <param name="characterCount">The character count to be returned.</param>
		/// <returns>The right part</returns>
		public static string Right(this string value, int characterCount)
		{
			if (value == null)
			{
				throw new ArgumentNullException(nameof(value));
			}

			if (characterCount >= value.Length)
			{
				throw new ArgumentOutOfRangeException(nameof(characterCount), characterCount, "characterCount must be less than length of string");
			}

			return value[^characterCount..];
		}

		/// <summary>Returns the right part of the string from index.</summary>
		/// <param name="value">The original value.</param>
		/// <param name="index">The start index for substringing.</param>
		/// <returns>The right part.</returns>
		public static string SubstringFrom(this string value, int index)
		{
			return index < 0 ? value : value[index..];
		}

		public static byte[] GetBytes(this string data)
		{
			return Encoding.Default.GetBytes(data);
		}

		public static byte[] GetBytes(this string data, Encoding encoding)
		{
			return encoding.GetBytes(data);
		}

		/// <summary>Convert text's case to a title case</summary>
		/// <remarks>UppperCase characters is the source string after the first of each word are lowered, unless the word is exactly 2 characters</remarks>
		public static string ToTitleCase(this string value)
		{
			return ToTitleCase(value, CultureInfo.CurrentCulture);
		}

		/// <summary>Convert text's case to a title case</summary>
		/// <remarks>UppperCase characters is the source string after the first of each word are lowered, unless the word is exactly 2 characters</remarks>
		public static string ToTitleCase(this string value, CultureInfo culture)
		{
			return culture.TextInfo.ToTitleCase(value);
		}

		public static string ToPlural(this string singular)
		{
			// Multiple words in the form A of B : Apply the plural to the first word only (A)
			int index = singular.LastIndexOf(" of ");
			if (index > 0)
			{
				return singular[0..] + singular.Remove(0, index).ToPlural();
			}

			// single Word rules
			//sibilant ending rule
			if (singular.EndsWith("sh"))
			{
				return singular + "es";
			}

			if (singular.EndsWith("ch"))
			{
				return singular + "es";
			}

			if (singular.EndsWith("us"))
			{
				return singular + "es";
			}

			if (singular.EndsWith("ss"))
			{
				return singular + "es";
			}
			//-ies rule
			if (singular.EndsWith("y"))
			{
				return singular.Remove(singular.Length - 1, 1) + "ies";
			}
			// -oes rule
			if (singular.EndsWith("o"))
			{
				return singular.Remove(singular.Length - 1, 1) + "oes";
			}
			// -s suffix rule
			return singular + "s";
		}

		/// <summary>
		/// Makes the current instance HTML safe.
		/// </summary>
		/// <param name="s">The current instance.</param>
		/// <returns>An HTML safe string.</returns>
		public static string ToHtmlSafe(this string s)
		{
			return s.ToHtmlSafe(false, false);
		}

		/// <summary>
		/// Makes the current instance HTML safe.
		/// </summary>
		/// <param name="s">The current instance.</param>
		/// <param name="all">Whether to make all characters entities or just those needed.</param>
		/// <returns>An HTML safe string.</returns>
		public static string ToHtmlSafe(this string s, bool all)
		{
			return s.ToHtmlSafe(all, false);
		}

		/// <summary>
		/// Makes the current instance HTML safe.
		/// </summary>
		/// <param name="s">The current instance.</param>
		/// <param name="all">Whether to make all characters entities or just those needed.</param>
		/// <param name="replace">Whether or not to encode spaces and line breaks.</param>
		/// <returns>An HTML safe string.</returns>
		public static string ToHtmlSafe(this string s, bool all, bool replace)
		{
			if (s.IsEmptyOrWhiteSpace())
			{
				return string.Empty;
			}

			int[] entities = new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 28, 29, 30, 31, 34, 39, 38, 60, 62, 123, 124, 125, 126, 127, 160, 161, 162, 163, 164, 165, 166, 167, 168, 169, 170, 171, 172, 173, 174, 175, 176, 177, 178, 179, 180, 181, 182, 183, 184, 185, 186, 187, 188, 189, 190, 191, 215, 247, 192, 193, 194, 195, 196, 197, 198, 199, 200, 201, 202, 203, 204, 205, 206, 207, 208, 209, 210, 211, 212, 213, 214, 215, 216, 217, 218, 219, 220, 221, 222, 223, 224, 225, 226, 227, 228, 229, 230, 231, 232, 233, 234, 235, 236, 237, 238, 239, 240, 241, 242, 243, 244, 245, 246, 247, 248, 249, 250, 251, 252, 253, 254, 255, 256, 8704, 8706, 8707, 8709, 8711, 8712, 8713, 8715, 8719, 8721, 8722, 8727, 8730, 8733, 8734, 8736, 8743, 8744, 8745, 8746, 8747, 8756, 8764, 8773, 8776, 8800, 8801, 8804, 8805, 8834, 8835, 8836, 8838, 8839, 8853, 8855, 8869, 8901, 913, 914, 915, 916, 917, 918, 919, 920, 921, 922, 923, 924, 925, 926, 927, 928, 929, 931, 932, 933, 934, 935, 936, 937, 945, 946, 947, 948, 949, 950, 951, 952, 953, 954, 955, 956, 957, 958, 959, 960, 961, 962, 963, 964, 965, 966, 967, 968, 969, 977, 978, 982, 338, 339, 352, 353, 376, 402, 710, 732, 8194, 8195, 8201, 8204, 8205, 8206, 8207, 8211, 8212, 8216, 8217, 8218, 8220, 8221, 8222, 8224, 8225, 8226, 8230, 8240, 8242, 8243, 8249, 8250, 8254, 8364, 8482, 8592, 8593, 8594, 8595, 8596, 8629, 8968, 8969, 8970, 8971, 9674, 9824, 9827, 9829, 9830 };
			StringBuilder sb = new StringBuilder();
			foreach (char c in s)
			{
				if (all || entities.Contains(c))
				{
					sb.Append("&#").Append((int)c).Append(';');
				}
				else
				{
					sb.Append(c);
				}
			}

			return replace ? sb.Replace("", "<br />").Replace("\n", "<br />").Replace(" ", "&nbsp;").ToString() : sb.ToString();
		}

		/// <summary>
		/// Returns true if strings are equals, without consideration to case (<see cref="StringComparison.InvariantCultureIgnoreCase"/>)
		/// </summary>
		public static bool EquivalentTo(this string s, string whateverCaseString)
		{
			return string.Equals(s, whateverCaseString, StringComparison.InvariantCultureIgnoreCase);
		}

		/// <summary>
		/// Replace all values in string
		/// </summary>
		/// <param name="value">The input string.</param>
		/// <param name="oldValues">List of old values, which must be replaced</param>
		/// <param name="replacePredicate">Function for replacement old values</param>
		/// <returns>Returns new string with the replaced values</returns>
		/// <example>
		/// 	<code>
		///         var str = "White Red Blue Green Yellow Black Gray";
		///         var achromaticColors = new[] {"White", "Black", "Gray"};
		///         str = str.ReplaceAll(achromaticColors, v => "[" + v + "]");
		///         // str == "[White] Red Blue Green Yellow [Black] [Gray]"
		/// 	</code>
		/// </example>
		public static string ReplaceAll(this string value, IEnumerable<string> oldValues, Func<string, string> replacePredicate)
		{
			StringBuilder sbStr = new StringBuilder(value);
			foreach (string oldValue in oldValues)
			{
				string newValue = replacePredicate(oldValue);
				sbStr.Replace(oldValue, newValue);
			}

			return sbStr.ToString();
		}

		/// <summary>
		/// Replace all values in string
		/// </summary>
		/// <param name="value">The input string.</param>
		/// <param name="oldValues">List of old values, which must be replaced</param>
		/// <param name="newValue">New value for all old values</param>
		/// <returns>Returns new string with the replaced values</returns>
		/// <example>
		/// 	<code>
		///         var str = "White Red Blue Green Yellow Black Gray";
		///         var achromaticColors = new[] {"White", "Black", "Gray"};
		///         str = str.ReplaceAll(achromaticColors, "[AchromaticColor]");
		///         // str == "[AchromaticColor] Red Blue Green Yellow [AchromaticColor] [AchromaticColor]"
		/// 	</code>
		/// </example>
		public static string ReplaceAll(this string value, IEnumerable<string> oldValues, string newValue)
		{
			StringBuilder sbStr = new StringBuilder(value);
			foreach (string oldValue in oldValues)
			{
				sbStr.Replace(oldValue, newValue);
			}

			return sbStr.ToString();
		}

		/// <summary>
		/// Replace all values in string
		/// </summary>
		/// <param name="value">The input string.</param>
		/// <param name="oldValues">List of old values, which must be replaced</param>
		/// <param name="newValues">List of new values</param>
		/// <returns>Returns new string with the replaced values</returns>
		/// <example>
		/// 	<code>
		///         var str = "White Red Blue Green Yellow Black Gray";
		///         var achromaticColors = new[] {"White", "Black", "Gray"};
		///         var exquisiteColors = new[] {"FloralWhite", "Bistre", "DavyGrey"};
		///         str = str.ReplaceAll(achromaticColors, exquisiteColors);
		///         // str == "FloralWhite Red Blue Green Yellow Bistre DavyGrey"
		/// 	</code>
		/// </example>
		public static string ReplaceAll(this string value, IEnumerable<string> oldValues, IEnumerable<string> newValues)
		{
			StringBuilder sbStr = new StringBuilder(value);
			IEnumerator<string> newValueEnum = newValues.GetEnumerator();
			foreach (string old in oldValues)
			{
				if (!newValueEnum.MoveNext())
				{
					throw new ArgumentOutOfRangeException(nameof(newValues), "newValues sequence is shorter than oldValues sequence");
				}

				sbStr.Replace(old, newValueEnum.Current);
			}
			if (newValueEnum.MoveNext())
			{
				throw new ArgumentOutOfRangeException(nameof(newValues), "newValues sequence is longer than oldValues sequence");
			}

			return sbStr.ToString();
		}

		#endregion Common string extensions

		#region Regex based extension methods

		/// <summary>
		/// 	Uses regular expressions to determine if the string matches to a given regex pattern.
		/// </summary>
		/// <param name = "value">The input string.</param>
		/// <param name = "regexPattern">The regular expression pattern.</param>
		/// <returns>
		/// 	<c>true</c> if the value is matching to the specified pattern; otherwise, <c>false</c>.
		/// </returns>
		/// <example>
		/// 	<code>
		/// 		var s = "12345";
		/// 		var isMatching = s.IsMatchingTo(@"^\d+$");
		/// 	</code>
		/// </example>
		public static bool IsMatchingTo(this string value, string regexPattern)
		{
			return IsMatchingTo(value, regexPattern, RegexOptions.None);
		}

		/// <summary>
		/// 	Uses regular expressions to determine if the string matches to a given regex pattern.
		/// </summary>
		/// <param name = "value">The input string.</param>
		/// <param name = "regexPattern">The regular expression pattern.</param>
		/// <param name = "options">The regular expression options.</param>
		/// <returns>
		/// 	<c>true</c> if the value is matching to the specified pattern; otherwise, <c>false</c>.
		/// </returns>
		/// <example>
		/// 	<code>
		/// 		var s = "12345";
		/// 		var isMatching = s.IsMatchingTo(@"^\d+$");
		/// 	</code>
		/// </example>
		public static bool IsMatchingTo(this string value, string regexPattern, RegexOptions options)
		{
			return Regex.IsMatch(value, regexPattern, options);
		}

		/// <summary>
		/// 	Uses regular expressions to replace parts of a string.
		/// </summary>
		/// <param name = "value">The input string.</param>
		/// <param name = "regexPattern">The regular expression pattern.</param>
		/// <param name = "replaceValue">The replacement value.</param>
		/// <returns>The newly created string</returns>
		/// <example>
		/// 	<code>
		/// 		var s = "12345";
		/// 		var replaced = s.ReplaceWith(@"\d", m => string.Concat(" -", m.Value, "- "));
		/// 	</code>
		/// </example>
		public static string ReplaceWith(this string value, string regexPattern, string replaceValue)
		{
			return ReplaceWith(value, regexPattern, replaceValue, RegexOptions.None);
		}

		/// <summary>
		/// 	Uses regular expressions to replace parts of a string.
		/// </summary>
		/// <param name = "value">The input string.</param>
		/// <param name = "regexPattern">The regular expression pattern.</param>
		/// <param name = "replaceValue">The replacement value.</param>
		/// <param name = "options">The regular expression options.</param>
		/// <returns>The newly created string</returns>
		/// <example>
		/// 	<code>
		/// 		var s = "12345";
		/// 		var replaced = s.ReplaceWith(@"\d", m => string.Concat(" -", m.Value, "- "));
		/// 	</code>
		/// </example>
		public static string ReplaceWith(this string value, string regexPattern, string replaceValue, RegexOptions options)
		{
			return Regex.Replace(value, regexPattern, replaceValue, options);
		}

		/// <summary>
		/// 	Uses regular expressions to replace parts of a string.
		/// </summary>
		/// <param name = "value">The input string.</param>
		/// <param name = "regexPattern">The regular expression pattern.</param>
		/// <param name = "evaluator">The replacement method / lambda expression.</param>
		/// <returns>The newly created string</returns>
		/// <example>
		/// 	<code>
		/// 		var s = "12345";
		/// 		var replaced = s.ReplaceWith(@"\d", m => string.Concat(" -", m.Value, "- "));
		/// 	</code>
		/// </example>
		public static string ReplaceWith(this string value, string regexPattern, MatchEvaluator evaluator)
		{
			return ReplaceWith(value, regexPattern, RegexOptions.None, evaluator);
		}

		/// <summary>
		/// 	Uses regular expressions to replace parts of a string.
		/// </summary>
		/// <param name = "value">The input string.</param>
		/// <param name = "regexPattern">The regular expression pattern.</param>
		/// <param name = "options">The regular expression options.</param>
		/// <param name = "evaluator">The replacement method / lambda expression.</param>
		/// <returns>The newly created string</returns>
		/// <example>
		/// 	<code>
		/// 		var s = "12345";
		/// 		var replaced = s.ReplaceWith(@"\d", m => string.Concat(" -", m.Value, "- "));
		/// 	</code>
		/// </example>
		public static string ReplaceWith(this string value, string regexPattern, RegexOptions options, MatchEvaluator evaluator)
		{
			return Regex.Replace(value, regexPattern, evaluator, options);
		}

		/// <summary>
		/// 	Uses regular expressions to determine all matches of a given regex pattern.
		/// </summary>
		/// <param name = "value">The input string.</param>
		/// <param name = "regexPattern">The regular expression pattern.</param>
		/// <returns>A collection of all matches</returns>
		public static MatchCollection GetMatches(this string value, string regexPattern)
		{
			return GetMatches(value, regexPattern, RegexOptions.None);
		}

		/// <summary>
		/// 	Uses regular expressions to determine all matches of a given regex pattern.
		/// </summary>
		/// <param name = "value">The input string.</param>
		/// <param name = "regexPattern">The regular expression pattern.</param>
		/// <param name = "options">The regular expression options.</param>
		/// <returns>A collection of all matches</returns>
		public static MatchCollection GetMatches(this string value, string regexPattern, RegexOptions options)
		{
			return Regex.Matches(value, regexPattern, options);
		}

		/// <summary>
		/// 	Uses regular expressions to determine all matches of a given regex pattern and returns them as string enumeration.
		/// </summary>
		/// <param name = "value">The input string.</param>
		/// <param name = "regexPattern">The regular expression pattern.</param>
		/// <returns>An enumeration of matching strings</returns>
		/// <example>
		/// 	<code>
		/// 		var s = "12345";
		/// 		foreach(var number in s.GetMatchingValues(@"\d")) {
		/// 		Console.WriteLine(number);
		/// 		}
		/// 	</code>
		/// </example>
		public static IEnumerable<string> GetMatchingValues(this string value, string regexPattern)
		{
			return GetMatchingValues(value, regexPattern, RegexOptions.None);
		}

		/// <summary>
		/// 	Uses regular expressions to determine all matches of a given regex pattern and returns them as string enumeration.
		/// </summary>
		/// <param name = "value">The input string.</param>
		/// <param name = "regexPattern">The regular expression pattern.</param>
		/// <param name = "options">The regular expression options.</param>
		/// <returns>An enumeration of matching strings</returns>
		/// <example>
		/// 	<code>
		/// 		var s = "12345";
		/// 		foreach(var number in s.GetMatchingValues(@"\d")) {
		/// 		Console.WriteLine(number);
		/// 		}
		/// 	</code>
		/// </example>
		public static IEnumerable<string> GetMatchingValues(this string value, string regexPattern, RegexOptions options)
		{
			foreach (Match match in GetMatches(value, regexPattern, options))
			{
				if (match.Success)
				{
					yield return match.Value;
				}
			}
		}

		/// <summary>
		/// 	Uses regular expressions to split a string into parts.
		/// </summary>
		/// <param name = "value">The input string.</param>
		/// <param name = "regexPattern">The regular expression pattern.</param>
		/// <returns>The splitted string array</returns>
		public static string[] Split(this string value, string regexPattern)
		{
			return value.Split(regexPattern, RegexOptions.None);
		}

		/// <summary>
		/// 	Uses regular expressions to split a string into parts.
		/// </summary>
		/// <param name = "value">The input string.</param>
		/// <param name = "regexPattern">The regular expression pattern.</param>
		/// <param name = "options">The regular expression options.</param>
		/// <returns>The splitted string array</returns>
		public static string[] Split(this string value, string regexPattern, RegexOptions options)
		{
			return Regex.Split(value, regexPattern, options);
		}

		/// <summary>
		/// 	Splits the given string into words and returns a string array.
		/// </summary>
		/// <param name = "value">The input string.</param>
		/// <returns>The splitted string array</returns>
		public static string[] GetWords(this string value)
		{
			return value.Split(@"\W");
		}

		/// <summary>
		/// 	Gets the nth "word" of a given string, where "words" are substrings separated by a given separator
		/// </summary>
		/// <param name = "value">The string from which the word should be retrieved.</param>
		/// <param name = "index">Index of the word (0-based).</param>
		/// <returns>
		/// 	The word at position n of the string.
		/// 	Trying to retrieve a word at a position lower than 0 or at a position where no word exists results in an exception.
		/// </returns>
		/// <remarks>
		/// 	Originally contributed by MMathews
		/// </remarks>
		public static string GetWordByIndex(this string value, int index)
		{
			string[] words = value.GetWords();

			if ((index < 0) || (index > words.Length - 1))
			{
				throw new IndexOutOfRangeException("The word number is out of range.");
			}

			return words[index];
		}

		/// <summary>
		/// Removed all special characters from the string.
		/// </summary>
		/// <param name="value">The input string.</param>
		/// <returns>The adjusted string.</returns>
		[Obsolete("Please use RemoveAllSpecialCharacters instead")]
		public static string AdjustInput(this string value)
		{
			return string.Join(null, Regex.Split(value, "[^a-zA-Z0-9]"));
		}

		/// <summary>
		/// Removed all special characters from the string.
		/// </summary>
		/// <param name="value">The input string.</param>
		/// <returns>The adjusted string.</returns>
		public static string RemoveAllSpecialCharacters(this string value)
		{
			StringBuilder sb = new StringBuilder(value.Length);
			foreach (char c in value.Where(c => char.IsLetterOrDigit(c)))
			{
				sb.Append(c);
			}

			return sb.ToString();
		}

		/// <summary>
		/// Add space on every upper character
		/// </summary>
		/// <param name="value">The input string.</param>
		/// <returns>The adjusted string.</returns>
		public static string SpaceOnUpper(this string value)
		{
			return Regex.Replace(value, "([A-Z])(?=[a-z])|(?<=[a-z])([A-Z]|[0-9]+)", " $1$2").TrimStart();
		}

		#region ExtractArguments extension

		/// <summary>
		/// Options to match the template with the original string
		/// </summary>
		public enum ComparsionTemplateOptions
		{
			/// <summary>
			/// Free template comparison
			/// </summary>
			Default,

			/// <summary>
			/// Template compared from beginning of input string
			/// </summary>
			FromStart,

			/// <summary>
			/// Template compared with the end of input string
			/// </summary>
			AtTheEnd,

			/// <summary>
			/// Template compared whole with input string
			/// </summary>
			Whole,
		}

		private const RegexOptions _defaultRegexOptions = RegexOptions.None;
		private const ComparsionTemplateOptions _defaultComparsionTemplateOptions = ComparsionTemplateOptions.Default;
		private static readonly string[] _reservedRegexOperators = new[] { @"\", "^", "$", "*", "+", "?", ".", "(", ")" };

		private static string GetRegexPattern(string template, ComparsionTemplateOptions compareTemplateOptions)
		{
			template = template.ReplaceAll(_reservedRegexOperators, v => @"\" + v);

			bool comparingFromStart = compareTemplateOptions == ComparsionTemplateOptions.FromStart ||
										compareTemplateOptions == ComparsionTemplateOptions.Whole;
			bool comparingAtTheEnd = compareTemplateOptions == ComparsionTemplateOptions.AtTheEnd ||
									 compareTemplateOptions == ComparsionTemplateOptions.Whole;
			StringBuilder pattern = new StringBuilder();

			if (comparingFromStart)
			{
				pattern.Append("^");
			}

			pattern.Append(Regex.Replace(template, @"\{[0-9]+\}",
											(Match m) =>
											{
												string argNum = m.ToString().Replace("{", "").Replace("}", "");
												return string.Format("(?<{0}>.*?)", int.Parse(argNum) + 1);
											}
							));

			if (comparingAtTheEnd || (template.LastOrDefault() == '}' && compareTemplateOptions == ComparsionTemplateOptions.Default))
			{
				pattern.Append("$");
			}

			return pattern.ToString();
		}

		/// <summary>
		/// Extract arguments from string by template
		/// </summary>
		/// <param name="value">The input string. For example, "My name is Aleksey".</param>
		/// <param name="template">Template with arguments in the format {№}. For example, "My name is {0} {1}.".</param>
		/// <param name="compareTemplateOptions">Template options for compare with input string.</param>
		/// <param name="regexOptions">Regex options.</param>
		/// <returns>Returns parsed arguments.</returns>
		/// <example>
		/// 	<code>
		/// 		var str = "My name is Aleksey Nagovitsyn. I'm from Russia.";
		/// 		var args = str.ExtractArguments(@"My name is {1} {0}. I'm from {2}.");
		///         // args[i] is [Nagovitsyn, Aleksey, Russia]
		/// 	</code>
		/// </example>
		public static IEnumerable<string> ExtractArguments(this string value, string template,
															 ComparsionTemplateOptions compareTemplateOptions = _defaultComparsionTemplateOptions,
															 RegexOptions regexOptions = _defaultRegexOptions)
		{
			return ExtractGroupArguments(value, template, compareTemplateOptions, regexOptions).Select(g => g.Value);
		}

		/// <summary>
		/// Extract arguments as regex groups from string by template
		/// </summary>
		/// <param name="value">The input string. For example, "My name is Aleksey".</param>
		/// <param name="template">Template with arguments in the format {№}. For example, "My name is {0} {1}.".</param>
		/// <param name="compareTemplateOptions">Template options for compare with input string.</param>
		/// <param name="regexOptions">Regex options.</param>
		/// <returns>Returns parsed arguments as regex groups.</returns>
		/// <example>
		/// 	<code>
		/// 		var str = "My name is Aleksey Nagovitsyn. I'm from Russia.";
		/// 		var groupArgs = str.ExtractGroupArguments(@"My name is {1} {0}. I'm from {2}.");
		///         // groupArgs[i].Value is [Nagovitsyn, Aleksey, Russia]
		/// 	</code>
		/// </example>
		public static IEnumerable<Group> ExtractGroupArguments(this string value, string template,
																 ComparsionTemplateOptions compareTemplateOptions = _defaultComparsionTemplateOptions,
																 RegexOptions regexOptions = _defaultRegexOptions)
		{
			string pattern = GetRegexPattern(template, compareTemplateOptions);
			Regex regex = new Regex(pattern, regexOptions);
			Match match = regex.Match(value);

			return match.Groups.Cast<Group>().Skip(1);
		}

		#endregion ExtractArguments extension

		#endregion Regex based extension methods

		#region String to Enum

		/// <summary>
		///     Parse a string to a enum item if that string exists in the enum otherwise return the default enum item.
		/// </summary>
		/// <typeparam name="TEnum">The Enum type.</typeparam>
		/// <param name="dataToMatch">The data will use to convert into give enum</param>
		/// <param name="ignorecase">Whether the enum parser will ignore the given data's case or not.</param>
		/// <returns>Converted enum.</returns>
		/// <example>
		/// 	<code>
		/// 		public enum EnumTwo {  None, One,}
		/// 		object[] items = new object[] { "One".ParseStringToEnum<EnumTwo>(), "Two".ParseStringToEnum<EnumTwo>() };
		/// 	</code>
		/// </example>
		public static TEnum ParseStringToEnum<TEnum>(this string dataToMatch)
				where TEnum : struct
		{
			return dataToMatch.IsItemInEnum<TEnum>()() ? default : (TEnum)Enum.Parse(typeof(TEnum), dataToMatch, default);
		}

		/// <summary>
		///     To check whether the data is defined in the given enum.
		/// </summary>
		/// <typeparam name="TEnum">The enum will use to check, the data defined.</typeparam>
		/// <param name="dataToCheck">To match against enum.</param>
		/// <returns>Anonymous method for the condition.</returns>
		/// <remarks>
		/// 	Contributed by Mohammad Rahman, http://mohammad-rahman.blogspot.com/
		/// </remarks>
		public static Func<bool> IsItemInEnum<TEnum>(this string dataToCheck)
			where TEnum : struct
		{
			return () => string.IsNullOrEmpty(dataToCheck) || !Enum.IsDefined(typeof(TEnum), dataToCheck);
		}

		#endregion String to Enum

		/// <summary>
		/// Encodes the email address so that the link is still valid, but the email address is useless for email harvesters.
		/// </summary>
		/// <param name="emailAddress">The email address.</param>
		/// <returns></returns>
		public static string EncodeEmailAddress(this string emailAddress)
		{
			int i;
			string repl;
			string tempHtmlEncode = emailAddress;
			for (i = tempHtmlEncode.Length; i >= 1; i--)
			{
				int acode = Convert.ToInt32(tempHtmlEncode[i - 1]);
				if (acode == 32)
				{
					repl = " ";
				}
				else if (acode == 34)
				{
					repl = "\"";
				}
				else if (acode == 38)
				{
					repl = "&";
				}
				else if (acode == 60)
				{
					repl = "<";
				}
				else if (acode == 62)
				{
					repl = ">";
				}
				else if (acode >= 32 && acode <= 127)
				{
					repl = "&#" + Convert.ToString(acode) + ";";
				}
				else
				{
					repl = "&#" + Convert.ToString(acode) + ";";
				}
				if (repl.Length > 0)
				{
					tempHtmlEncode = tempHtmlEncode[0..] +
													 repl + tempHtmlEncode[i..];
				}
			}
			return tempHtmlEncode;
		}

		/// <summary>
		/// Truncates a string with optional Ellipses added
		/// </summary>
		/// <param name="this"></param>
		/// <param name="length"></param>
		/// <param name="useElipses"></param>
		/// <returns></returns>
		public static string Truncate(this string @this, int length, bool useElipses = false)
		{
			int e = useElipses ? 3 : 0;
			if (length - e <= 0)
			{
				throw new InvalidOperationException(string.Format("Length must be greater than {0}.", e));
			}

			if (string.IsNullOrEmpty(@this) || @this.Length <= length)
			{
				return @this;
			}

			return @this[0..] + new string('.', e);
		}

		public static string Raw(this string str)
		{
			return HttpUtility.HtmlEncode(str);
		}

		public static string PhoneNumberFormatter(this string value)
		{
			value = new Regex(@"\D").Replace(value, string.Empty);
			value = value.TrimStart('1');

			if (value.Length == 0)
			{
				return string.Empty;
			}
			else if (value.Length < 3)
			{
				return string.Format("({0})", value[0..]);
			}
			else if (value.Length < 7)
			{
				return string.Format("({0}) {1}", value[0..], value[3..]);
			}
			else if (value.Length < 11)
			{
				return string.Format("({0}) {1}-{2}", value[0..], value[3..], value[6..]);
			}
			else if (value.Length > 10)
			{
				value = value.Remove(value.Length - 1, 1);
				return string.Format("{0}.{1}.{2}", value[0..], value[3..], value[6..]);
			}
			return value;
		}

		public static string GenerateSlug(this string phrase, int maxLength = 50)
		{
			string str = Regex.Replace(Regex.Replace(phrase.ToLower(), "[^a-z0-9\\s-]", ""), "[\\s-]{2,}", " ").Trim();
			return Regex.Replace(str.Substring(0, str.Length <= maxLength ? str.Length : maxLength).Trim(), "\\s", "-");
		}
	}
}