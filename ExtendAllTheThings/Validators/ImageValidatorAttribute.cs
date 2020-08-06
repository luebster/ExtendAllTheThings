using Microsoft.AspNetCore.Http;

using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ExtendAllTheThings.CustomValidators
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
	public sealed class ImageValidatorAttribute : ValidationAttribute
	{
		private static readonly string[] _contentTypes = { "image/jpeg", "image/jpg", "image/png", "image/gif" };

		public bool Required { get; set; }

		public override bool IsValid(object value)
		{
			if (value == null)
			{
				if (!Required)
				{
					return true;
				}

				ErrorMessage = "You must upload an image.";
				return false;
			}

			IFormFile image = (IFormFile)value;
			const int size = 1024 * 1024 * 8; // 8 MB

			bool retlval = FileIsWebFriendlyImage(image, size);
			return retlval;
		}

		public static bool FileIsWebFriendlyImage(IFormFile image)
		{
			try
			{
				return _contentTypes.Contains(image.ContentType);
			}
			catch (Exception)
			{
				throw;
			}
		}

		public static bool FileIsWebFriendlyImage(IFormFile image, long size)
		{
			bool retval = image.Length <= size && FileIsWebFriendlyImage(image);
			return retval;
		}
	}
}