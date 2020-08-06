using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ExtendAllTheThings.Validators
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
	public sealed class FileTypeAttribute : ValidationAttribute, IClientModelValidator
	{
		private const string _defaultErrorMessage = "Only the following file types are allowed: {0}";
		private IEnumerable<string> ValidTypes { get; }

		public FileTypeAttribute(string validTypes)
		{
			ValidTypes = validTypes.Split(',').Select(s => s.Trim());
			ErrorMessage = string.Format(_defaultErrorMessage, string.Join(" or ", ValidTypes));
		}

		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			if (value is IFormFile file && file != null && !ValidTypes.Any(e => file.FileName.EndsWith(e, StringComparison.OrdinalIgnoreCase)))
			{
				return new ValidationResult(ErrorMessageString);
			}
			return ValidationResult.Success;
		}

		public void AddValidation(ClientModelValidationContext context)
		{
			context.Attributes.Add("data-val", "true");
			context.Attributes.Add("data-val-filetype", ErrorMessageString);
			context.Attributes.Add("data-val-filetype-validtypes", string.Join(",", ValidTypes));
		}
	}
}
