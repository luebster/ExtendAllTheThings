using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ExtendAllTheThings.Validators
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
	public sealed class CheckBoxRequiredAttribute : ValidationAttribute, IClientModelValidator
	{
		public override bool IsValid(object value)
		{
			if (value is bool x)
			{
				return x;
			}

			return false;
		}

		public void AddValidation(ClientModelValidationContext context)
		{
			MergeAttribute(context.Attributes, "data-val", "true");
			var errorMessage = FormatErrorMessage(context.ModelMetadata.GetDisplayName());
			MergeAttribute(context.Attributes, "data-val-checkboxrequired", errorMessage);
		}

		private bool MergeAttribute(IDictionary<string, string> attributes, string key, string value)
		{
			if (attributes.ContainsKey(key))
			{
				return false;
			}
			attributes.Add(key, value);
			return true;
		}
	}
}
