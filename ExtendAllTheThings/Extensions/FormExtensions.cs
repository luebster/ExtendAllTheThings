using Microsoft.AspNetCore.Http;

namespace ExtendAllTheThings.Extensions
{
	public static class FormExtensions
	{
		public static bool HasFile(this IFormFile file)
		{
			return file?.Length > 0;
		}
	}
}