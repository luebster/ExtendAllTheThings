using ExtendAllTheThings.Extensions;

using System;
using System.ComponentModel.DataAnnotations;

namespace ExtendAllTheThings.Validators
{
	[AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
	public sealed class NotInThePastValidatorAttribute : ValidationAttribute
	{
		public override bool IsValid(object value)
		{
			if (value != null)
			{
				if (DateTime.TryParse(value.ToString(), out DateTime selectedDate))
				{
					if (selectedDate < DateTime.Now.GetPreviousWeekday(DayOfWeek.Monday))
					{
						ErrorMessage = "The selected date is in the past.";
					}
					else
					{
						return true;
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