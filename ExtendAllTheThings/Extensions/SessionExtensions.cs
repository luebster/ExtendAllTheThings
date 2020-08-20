using Microsoft.AspNetCore.Http;

using System.Text.Json;

namespace ExtendAllTheThings.Extensions
{
	public static class SessionExtensions
	{
		/// <summary>
		/// Save an object in session memory
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="session"></param>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public static void Set<T>(this ISession session, string key, T value)
		{
			session.SetString(key, JsonSerializer.Serialize(value));
		}

		/// <summary>
		/// Get an object from session memory
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="session"></param>
		/// <param name="key"></param>
		/// <returns></returns>
		public static T Get<T>(this ISession session, string key)
		{
			var value = session.GetString(key);

			return value == null ? default : JsonSerializer.Deserialize<T>(value);
		}
	}
}
