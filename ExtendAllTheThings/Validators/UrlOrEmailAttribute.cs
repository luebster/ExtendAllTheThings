using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace ExtendAllTheThings.Validators
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
	public sealed class UrlOrEmailAttribute : ValidationAttribute, IClientModelValidator
	{
		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			var isValid = false;

			const string urlRegex = @"https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{2,256}\.[a-z]{2,4}\b([-a-zA-Z0-9@:%_\+.~#?&//=]*)";
			const string emailRegex = @"([\w\.\-_]+)?\w+@[\w-_]+(\.\w+){1,}";

			var input = (string)value;

			if (input.Length > 0)
			{
				if (Regex.IsMatch(input, urlRegex) || Regex.IsMatch(input, emailRegex))
				{
					isValid = true;
				}
			}

			return isValid ? ValidationResult.Success : new ValidationResult("The link must be a complete url or an email address");
		}

		public void AddValidation(ClientModelValidationContext context)
		{
			context.Attributes.Add("data-val", "true");
			context.Attributes.Add("data-val-urloremail", ErrorMessageString);
		}
	}
}