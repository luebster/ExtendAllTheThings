using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ExtendAllTheThings.Validators
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
	public sealed class DisabledDaysAttribute : ValidationAttribute
	{
		public DayOfWeek[] DisabledDays { get; set; }

		public DisabledDaysAttribute(DayOfWeek[] DisabledDays)
		{
			this.DisabledDays = DisabledDays;
		}

		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			if (validationContext == null)
			{
				throw new ArgumentNullException(nameof(validationContext));
			}

			var result = (DateTime)value;

			// check if this value is actually required and validate it

			if (DisabledDays.Contains(result.DayOfWeek))
			{
				return new ValidationResult(string.Format("{0} is not a valid day of the week for {1}", result.DayOfWeek, validationContext.DisplayName));
			}

			return ValidationResult.Success;
		}
	}
}
