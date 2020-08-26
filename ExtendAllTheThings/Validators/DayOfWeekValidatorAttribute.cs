using System;
using System.ComponentModel.DataAnnotations;

namespace ExtendAllTheThings.Validators
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
	public sealed class DayOfWeekValidatorAttribute : ValidationAttribute
	{
		public DayOfWeek DayOfWeek { get; set; }

		public override bool IsValid(object value)
		{
			if (value != null)
			{
				if (DateTime.TryParse(value.ToString(), out DateTime result))
				{
					if (result.DayOfWeek.Equals(DayOfWeek))
					{
						return true;
					}
					else
					{
						ErrorMessage = "The date is not a " + DayOfWeek;
					}
				}
				else
				{
					ErrorMessage = "The supplied value is not a valid date.";
				}
			}
			else
			{
				ErrorMessage = "The value cannot be empty.";
			}

			return false;
		}
	}
}