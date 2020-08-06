using Microsoft.AspNetCore.Http;

namespace ExtendAllTheThings.Extensions
{
	public static class HttpRequestExtensions
	{
		public static bool HasFile(this IFormFile file)
		{
			return file?.Length > 0;
		}
	}
}
