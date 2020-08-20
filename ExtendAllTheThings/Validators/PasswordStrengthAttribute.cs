using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ExtendAllTheThings.Validators
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
	public sealed class PasswordStrengthAttribute : ValidationAttribute, IClientModelValidator
	{
		public int MinimumLength { get; set; } = 8;
		public int MaximumLength { get; set; } = 50;

		protected override ValidationResult IsValid(object value, ValidationContext context)
		{
			var validPassword = false;
			var reason = string.Empty;
			var password = value == null ? string.Empty : value.ToString();

			if (string.IsNullOrEmpty(password) || password.Length < MinimumLength)
			{
				reason = "Your new password must be at least " + MinimumLength + " characters long.";
			}
			else if (password.Length > MaximumLength)
			{
				reason = "Your new password must be less than " + MaximumLength + " characters long.";
			}
			else if (!password.Any(char.IsDigit))
			{
				reason = "Your new password must contain at least one number.";
			}
			else if (!password.Any(char.IsLetter))
			{
				reason = "Your new password must contain at least one letter.";
			}
			else
			{
				validPassword = true;
			}

			return validPassword ? ValidationResult.Success : new ValidationResult(reason);
		}

		public void AddValidation(ClientModelValidationContext context)
		{
			context.Attributes.Add("data-val", "true");
			context.Attributes.Add("data-val-passwordstrength", ErrorMessageString);
		}
	}
}
