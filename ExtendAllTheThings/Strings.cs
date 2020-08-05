using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ExtendAllTheThings
{
	public static class Strings
	{

		#region Common string extensions

		/// <summary>
		/// 	Determines whether the specified string is null or empty.
		/// </summary>
		/// <param name = "value">The string value to check.</param>
		public static bool IsNull(this string value)
		{
			return ((value == null) || (value.Length == 0));
		}

		/// <summary>
		/// 	Determines whether the specified string is not null or empty.
		/// </summary>
		/// <param name = "value">The string value to check.</param>
		public static bool IsNotNull(this string value)
		{
			return (value.IsNull() == false);
		}


		/// <summary>
		/// 	Trims the text to a provided maximum length.
		/// </summary>
		/// <param name = "value">The input string.</param>
		/// <param name = "maxLength">Maximum length.</param>
		/// <returns></returns>
		/// <remarks>
		/// 	Proposed by Rene Schulte
		/// </remarks>
		public static string TrimToMaxLength(this string value, int maxLength)
		{
			return (value == null || value.Length <= maxLength ? value : value.Substring(0, maxLength));
		}

		/// <summary>
		/// 	Trims the text to a provided maximum length and adds a suffix if required.
		/// </summary>
		/// <param name = "value">The input string.</param>
		/// <param name = "maxLength">Maximum length.</param>
		/// <param name = "suffix">The suffix.</param>
		/// <returns></returns>
		/// <remarks>
		/// 	Proposed by Rene Schulte
		/// </remarks>
		public static string TrimToMaxLength(this string value, int maxLength, string suffix)
		{
			return (value == null || value.Length <= maxLength ? value : string.Concat(value.Substring(0, maxLength), suffix));
		}

		/// <summary>
		/// 	Determines whether the comparison value strig is contained within the input value string
		/// </summary>
		/// <param name = "inputValue">The input value.</param>
		/// <param name = "comparisonValue">The comparison value.</param>
		/// <param name = "comparisonType">Type of the comparison to allow case sensitive or insensitive comparison.</param>
		/// <returns>
		/// 	<c>true</c> if input value contains the specified value, otherwise, <c>false</c>.
		/// </returns>
		public static bool Contains(this string inputValue, string comparisonValue, StringComparison comparisonType)
		{
			return (inputValue.IndexOf(comparisonValue, comparisonType) != -1);
		}

		/// <summary>
		/// 	Determines whether the comparison value string is contained within the input value string without any
		///     consideration about the case (<see cref="StringComparison.InvariantCultureIgnoreCase"/>).
		/// </summary>
		/// <param name = "inputValue">The input value.</param>
		/// <param name = "comparisonValue">The comparison value.  Case insensitive</param>
		/// <returns>
		/// 	<c>true</c> if input value contains the specified value (case insensitive), otherwise, <c>false</c>.
		/// </returns>
		public static bool ContainsEquivalenceTo(this string inputValue, string comparisonValue)
		{
			return BothStringsAreEmpty(inputValue, comparisonValue) || StringContainsEquivalence(inputValue, comparisonValue);
		}

		/// <summary>
		/// Centers a charters in this string, padding in both, left and right, by specified Unicode character,
		/// for a specified total lenght.
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
		/// length of width paramether.</returns>
		public static string PadBoth(this string value, int width, char padChar, bool truncate = false)
		{
			int diff = width - value.Length;
			if (diff == 0 || diff < 0 && !(truncate))
			{
				return value;
			}
			else if (diff < 0)
			{
				return value.Substring(0, width);
			}
			else
			{
				return value.PadLeft(width - diff / 2, padChar).PadRight(width, padChar);
			}
		}

		/// <summary>
		/// 	Loads the string into a LINQ to XML XDocument
		/// </summary>
		/// <param name = "xml">The XML string.</param>
		/// <returns>The XML document object model (XDocument)</returns>
		public static XDocument ToXDocument(this string xml)
		{
			return XDocument.Parse(xml);
		}

		/// <summary>
		/// 	Loads the string into a XML DOM object (XmlDocument)
		/// </summary>
		/// <param name = "xml">The XML string.</param>
		/// <returns>The XML document object model (XmlDocument)</returns>
		public static XmlDocument ToXmlDOM(this string xml)
		{
			var document = new XmlDocument();
			document.LoadXml(xml);
			return document;
		}

		/// <summary>
		/// 	Loads the string into a XML XPath DOM (XPathDocument)
		/// </summary>
		/// <param name = "xml">The XML string.</param>
		/// <returns>The XML XPath document object model (XPathNavigator)</returns>
		public static XPathNavigator ToXPath(this string xml)
		{
			var document = new XPathDocument(new StringReader(xml));
			return document.CreateNavigator();
		}

		/// <summary>
		///     Loads the string into a LINQ to XML XElement
		/// </summary>
		/// <param name = "xml">The XML string.</param>
		/// <returns>The XML element object model (XElement)</returns>
		public static XElement ToXElement(this string xml)
		{
			return XElement.Parse(xml);
		}

		/// <summary>
		/// 	Reverses / mirrors a string.
		/// </summary>
		/// <param name = "value">The string to be reversed.</param>
		/// <returns>The reversed string</returns>
		public static string Reverse(this string value)
		{
			if (value.IsEmpty() || (value.Length == 1))
				return value;

			var chars = value.ToCharArray();
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
				return new string(value[0], repeatCount);

			var sb = new StringBuilder(repeatCount * value.Length);
			while (repeatCount-- > 0)
				sb.Append(value);
			return sb.ToString();
		}

		/// <summary>
		/// 	Tests whether the contents of a string is a numeric value
		/// </summary>
		/// <param name = "value">String to check</param>
		/// <returns>
		/// 	Boolean indicating whether or not the string contents are numeric
		/// </returns>
		/// <remarks>
		/// 	Contributed by Kenneth Scott
		/// </remarks>
		public static bool IsNumeric(this string value)
		{
			float output;
			return float.TryParse(value, out output);
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
			return value.Where(Char.IsDigit).Aggregate(new StringBuilder(value.Length), (sb, c) => sb.Append(c)).ToString();
		}



		#endregion

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
		public static Guid ToGuidSave(this string value, Guid defaultValue)
		{
			if (value.IsEmpty())
				return defaultValue;

			try
			{
				return value.ToGuid();
			}
			catch { }

			return defaultValue;
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
			var xPos = value.IndexOf(x);
			return xPos == -1 ? String.Empty : value.Substring(0, xPos);
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
			var xPos = value.IndexOf(x);
			var yPos = value.LastIndexOf(y);

			if (xPos == -1 || xPos == -1)
				return String.Empty;

			var startIndex = xPos + x.Length;
			return startIndex >= yPos ? String.Empty : value.Substring(startIndex, yPos - startIndex).Trim();
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
			var xPos = value.LastIndexOf(x);

			if (xPos == -1)
				return String.Empty;

			var startIndex = xPos + x.Length;
			return startIndex >= value.Length ? String.Empty : value.Substring(startIndex).Trim();
		}

		/// <summary>
		/// 	A generic version of System.String.Join()
		/// </summary>
		/// <typeparam name = "T">
		/// 	The type of the array to join
		/// </typeparam>
		/// <param name = "separator">
		/// 	The separator to appear between each element
		/// </param>
		/// <param name = "value">
		/// 	An array of values
		/// </param>
		/// <returns>
		/// 	The join.
		/// </returns>
		/// <remarks>
		/// 	Contributed by Michael T, http://about.me/MichaelTran
		/// </remarks>
		public static string Join<T>(string separator, T[] value)
		{
			if (value == null || value.Length == 0)
				return string.Empty;
			if (separator == null)
				separator = string.Empty;
			Converter<T, string> converter = o => o.ToString();
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
		/// <remarks>
		/// 	Contributed by Michael T, http://about.me/MichaelTran
		/// </remarks>
		public static string Remove(this string value, params char[] removeCharc)
		{
			var result = value;
			if (!string.IsNullOrEmpty(result) && removeCharc != null)
				Array.ForEach(removeCharc, c => result = result.Remove(c.ToString()));

			return result;

		}

		/// <summary>
		/// Remove any instance of the given string pattern from the current string.
		/// </summary>
		/// <param name="value">The input.</param>
		/// <param name="strings">The strings.</param>
		/// <returns></returns>
		/// <remarks>
		/// Contributed by Michael T, http://about.me/MichaelTran
		/// </remarks>
		public static string Remove(this string value, params string[] strings)
		{
			return strings.Aggregate(value, (current, c) => current.Replace(c, string.Empty));
		}

		/// <summary>Finds out if the specified string contains null, empty or consists only of white-space characters</summary>
		/// <param name = "value">The input string</param>
		public static bool IsEmptyOrWhiteSpace(this string value)
		{
			return (value.IsEmpty() || value.All(t => char.IsWhiteSpace(t)));
		}

		/// <summary>Determines whether the specified string is not null, empty or consists only of white-space characters</summary>
		/// <param name = "value">The string value to check</param>
		public static bool IsNotEmptyOrWhiteSpace(this string value)
		{
			return (value.IsEmptyOrWhiteSpace() == false);
		}

		/// <summary>Checks whether the string is null, empty or consists only of white-space characters and returns a default value in case</summary>
		/// <param name = "value">The string to check</param>
		/// <param name = "defaultValue">The default value</param>
		/// <returns>Either the string or the default value</returns>
		public static string IfEmptyOrWhiteSpace(this string value, string defaultValue)
		{
			return (value.IsEmptyOrWhiteSpace() ? defaultValue : value);
		}

		/// <summary>Uppercase First Letter</summary>
		/// <param name = "value">The string value to process</param>
		public static string ToUpperFirstLetter(this string value)
		{
			if (value.IsEmptyOrWhiteSpace()) return string.Empty;

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
				throw new ArgumentNullException("value");
			if (characterCount >= value.Length)
				throw new ArgumentOutOfRangeException("characterCount", characterCount, "characterCount must be less than length of string");
			return value.Substring(0, characterCount);
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
				throw new ArgumentNullException("value");
			if (characterCount >= value.Length)
				throw new ArgumentOutOfRangeException("characterCount", characterCount, "characterCount must be less than length of string");
			return value.Substring(value.Length - characterCount);
		}

		/// <summary>Returns the right part of the string from index.</summary>
		/// <param name="value">The original value.</param>
		/// <param name="index">The start index for substringing.</param>
		/// <returns>The right part.</returns>
		public static string SubstringFrom(this string value, int index)
		{
			return index < 0 ? value : value.Substring(index, value.Length - index);
		}

		//todo: xml documentation requires
		//todo: unit test required
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
			return ToTitleCase(value, ExtensionMethodSetting.DefaultCulture);
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
			if (index > 0) return (singular.Substring(0, index)) + singular.Remove(0, index).ToPlural();

			// single Word rules
			//sibilant ending rule
			if (singular.EndsWith("sh")) return singular + "es";
			if (singular.EndsWith("ch")) return singular + "es";
			if (singular.EndsWith("us")) return singular + "es";
			if (singular.EndsWith("ss")) return singular + "es";
			//-ies rule
			if (singular.EndsWith("y")) return singular.Remove(singular.Length - 1, 1) + "ies";
			// -oes rule
			if (singular.EndsWith("o")) return singular.Remove(singular.Length - 1, 1) + "oes";
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
				return string.Empty;
			var entities = new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 28, 29, 30, 31, 34, 39, 38, 60, 62, 123, 124, 125, 126, 127, 160, 161, 162, 163, 164, 165, 166, 167, 168, 169, 170, 171, 172, 173, 174, 175, 176, 177, 178, 179, 180, 181, 182, 183, 184, 185, 186, 187, 188, 189, 190, 191, 215, 247, 192, 193, 194, 195, 196, 197, 198, 199, 200, 201, 202, 203, 204, 205, 206, 207, 208, 209, 210, 211, 212, 213, 214, 215, 216, 217, 218, 219, 220, 221, 222, 223, 224, 225, 226, 227, 228, 229, 230, 231, 232, 233, 234, 235, 236, 237, 238, 239, 240, 241, 242, 243, 244, 245, 246, 247, 248, 249, 250, 251, 252, 253, 254, 255, 256, 8704, 8706, 8707, 8709, 8711, 8712, 8713, 8715, 8719, 8721, 8722, 8727, 8730, 8733, 8734, 8736, 8743, 8744, 8745, 8746, 8747, 8756, 8764, 8773, 8776, 8800, 8801, 8804, 8805, 8834, 8835, 8836, 8838, 8839, 8853, 8855, 8869, 8901, 913, 914, 915, 916, 917, 918, 919, 920, 921, 922, 923, 924, 925, 926, 927, 928, 929, 931, 932, 933, 934, 935, 936, 937, 945, 946, 947, 948, 949, 950, 951, 952, 953, 954, 955, 956, 957, 958, 959, 960, 961, 962, 963, 964, 965, 966, 967, 968, 969, 977, 978, 982, 338, 339, 352, 353, 376, 402, 710, 732, 8194, 8195, 8201, 8204, 8205, 8206, 8207, 8211, 8212, 8216, 8217, 8218, 8220, 8221, 8222, 8224, 8225, 8226, 8230, 8240, 8242, 8243, 8249, 8250, 8254, 8364, 8482, 8592, 8593, 8594, 8595, 8596, 8629, 8968, 8969, 8970, 8971, 9674, 9824, 9827, 9829, 9830 };
			var sb = new StringBuilder();
			foreach (var c in s)
			{
				if (all || entities.Contains(c))
					sb.Append("&#" + ((int)c) + ";");
				else
					sb.Append(c);
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
		/// <remarks>
		/// 	Contributed by nagits, http://about.me/AlekseyNagovitsyn
		/// </remarks>
		public static string ReplaceAll(this string value, IEnumerable<string> oldValues, Func<string, string> replacePredicate)
		{
			var sbStr = new StringBuilder(value);
			foreach (var oldValue in oldValues)
			{
				var newValue = replacePredicate(oldValue);
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
		/// <remarks>
		/// 	Contributed by nagits, http://about.me/AlekseyNagovitsyn
		/// </remarks>
		public static string ReplaceAll(this string value, IEnumerable<string> oldValues, string newValue)
		{
			var sbStr = new StringBuilder(value);
			foreach (var oldValue in oldValues)
				sbStr.Replace(oldValue, newValue);

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
		/// <remarks>
		/// 	Contributed by nagits, http://about.me/AlekseyNagovitsyn
		/// </remarks> 
		public static string ReplaceAll(this string value, IEnumerable<string> oldValues, IEnumerable<string> newValues)
		{
			var sbStr = new StringBuilder(value);
			var newValueEnum = newValues.GetEnumerator();
			foreach (var old in oldValues)
			{
				if (!newValueEnum.MoveNext())
					throw new ArgumentOutOfRangeException("newValues", "newValues sequence is shorter than oldValues sequence");
				sbStr.Replace(old, newValueEnum.Current);
			}
			if (newValueEnum.MoveNext())
				throw new ArgumentOutOfRangeException("newValues", "newValues sequence is longer than oldValues sequence");

			return sbStr.ToString();
		}

		#endregion

		/// <summary>
		/// Generate a slug for use as a unique URL path part
		/// </summary>
		/// <param name="phrase">The string to slugify</param>
		/// <param name="maxLength">The maximum length the slug should be</param>
		/// <returns></returns>
		public static string Slugify(this string phrase, int maxLength = 50)
		{
			var str = Regex.Replace(Regex.Replace(phrase.ToLower(), "[^a-z0-9\\s-]", ""), "[\\s-]{2,}", " ").Trim();
			return Regex.Replace(str.Substring(0, str.Length <= maxLength ? str.Length : maxLength).Trim(), "\\s", "-");
		}

		/// <summary>
		/// Remove all non-digit characters from a string. Great for cleansing phone numbers.
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public static string GetNumbers(this string text)
		{
			text = text ?? string.Empty;
			return new string(text.Where(p => char.IsDigit(p)).ToArray());
		}

		/// <summary>
		/// Truncates a string containing HTML to a number of text characters, keeping whole words.
		/// The result contains HTML and any tags left open are closed.
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public static string TruncateHtml(this string html, int maxCharacters, string trailingText)
		{
			if (string.IsNullOrEmpty(html))
			{
				return html;
			}

			// find the spot to truncate
			// count the text characters and ignore tags
			var textCount = 0;
			var charCount = 0;
			var ignore = false;
			foreach (var c in html)
			{
				charCount++;
				if (c == '<')
				{
					ignore = true;
				}
				else if (!ignore)
				{
					textCount++;
				}

				if (c == '>')
				{
					ignore = false;
				}

				// stop once we hit the limit
				if (textCount >= maxCharacters)
				{
					break;
				}
			}

			// Truncate the html and keep whole words only
			var trunc = new StringBuilder(html.TruncateWords(charCount));

			// keep track of open tags and close any tags left open
			var tags = new Stack<string>();
			MatchCollection matches = Regex.Matches(trunc.ToString(),
					@"<((?<tag>[^\s/>]+)|/(?<closeTag>[^\s>]+)).*?(?<selfClose>/)?\s*>",
					RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Multiline);

			foreach (Match match in matches)
			{
				if (match.Success)
				{
					var tag = match.Groups["tag"].Value;
					var closeTag = match.Groups["closeTag"].Value;

					// push to stack if open tag and ignore it if it is self-closing, i.e. <br />
					if (!string.IsNullOrEmpty(tag) && string.IsNullOrEmpty(match.Groups["selfClose"].Value))
					{
						tags.Push(tag);
					}

					// pop from stack if close tag
					else if (!string.IsNullOrEmpty(closeTag))
					{
						// pop the tag to close it.. find the matching opening tag
						// ignore any unclosed tags
						while (tags.Pop() != closeTag && tags.Count > 0)
						{ }
					}
				}
			}

			if (html.Length > charCount)
			{
				// add the trailing text
				trunc.Append(trailingText);
			}

			// pop the rest off the stack to close remainder of tags
			while (tags.Count > 0)
			{
				trunc.Append("</");
				trunc.Append(tags.Pop());
				trunc.Append('>');
			}

			return trunc.ToString();
		}

		/// <summary>
		/// Truncates a string containing HTML to a number of text characters, keeping whole words.
		/// The result contains HTML and any tags left open are closed.
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public static string TruncateHtml(this string html, int maxCharacters)
		{
			return html.TruncateHtml(maxCharacters, null);
		}

		/// <summary>
		/// Truncates a string containing HTML to the first occurrence of a delimiter
		/// </summary>
		/// <param name="html">The HTML string to truncate</param>
		/// <param name="delimiter">The delimiter</param>
		/// <param name="comparison">The delimiter comparison type</param>
		/// <returns></returns>
		public static string TruncateHtmlByDelimiter(this string html, string delimiter, StringComparison comparison = StringComparison.Ordinal)
		{
			var index = html.IndexOf(delimiter, comparison);
			if (index <= 0)
			{
				return html;
			}

			var r = html.Substring(0, index);
			return r.TruncateHtml(r.StripHtml().Length);
		}

		/// <summary>
		/// Strips all HTML tags from a string
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public static string StripHtml(this string html)
		{
			if (string.IsNullOrEmpty(html))
			{
				return html;
			}

			return Regex.Replace(html, @"<(.|\n)*?>", string.Empty);
		}

		/// <summary>
		/// Truncates text to a number of characters
		/// </summary>
		/// <param name="text"></param>
		/// <param name="maxCharacters"></param>
		/// <param name="trailingText"></param>
		/// <returns></returns>
		public static string Truncate(this string text, int maxCharacters)
		{
			return text.Truncate(maxCharacters, null);
		}

		/// <summary>
		/// Truncates text to a number of characters and adds trailing text, i.e. elipses, to the end
		/// </summary>
		/// <param name="text"></param>
		/// <param name="maxCharacters"></param>
		/// <param name="trailingText"></param>
		/// <returns></returns>
		public static string Truncate(this string text, int maxCharacters, string trailingText)
		{
			if (string.IsNullOrEmpty(text) || maxCharacters <= 0 || text.Length <= maxCharacters)
			{
				return text;
			}
			else
			{
				return text.Substring(0, maxCharacters) + trailingText;
			}
		}

		/// <summary>
		/// Truncates text and discars any partial words left at the end
		/// </summary>
		/// <param name="text"></param>
		/// <param name="maxCharacters"></param>
		/// <param name="trailingText"></param>
		/// <returns></returns>
		public static string TruncateWords(this string text, int maxCharacters)
		{
			return text.TruncateWords(maxCharacters, null);
		}

		/// <summary>
		/// Truncates text and discars any partial words left at the end
		/// </summary>
		/// <param name="text"></param>
		/// <param name="maxCharacters"></param>
		/// <param name="trailingText"></param>
		/// <returns></returns>
		public static string TruncateWords(this string text, int maxCharacters, string trailingText)
		{
			if (string.IsNullOrEmpty(text) || maxCharacters <= 0 || text.Length <= maxCharacters)
			{
				return text;
			}

			// trunctate the text, then remove the partial word at the end
			return Regex.Replace(text.Truncate(maxCharacters),
					@"\s+[^\s]+$", string.Empty, RegexOptions.IgnoreCase | RegexOptions.Compiled) + trailingText;
		}
	}
}