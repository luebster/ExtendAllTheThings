using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace ExtendAllTheThings.Validators
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public sealed class RequireAtLeastOnePropertyAttribute : ValidationAttribute
	{
		private string[] PropertyList { get; }

		public RequireAtLeastOnePropertyAttribute(params string[] propertyList)
		{
			PropertyList = propertyList;
		}

		// See	http://stackoverflow.com/a/1365669
		//			https://stackoverflow.com/a/26424791/256885
		public override object TypeId => this;

		public override bool IsValid(object value)
		{
			PropertyInfo propertyInfo;
			foreach (var propertyName in PropertyList)
			{
				propertyInfo = value.GetType().GetProperty(propertyName);

				if (propertyInfo?.GetValue(value, null) != null)
				{
					return true;
				}
			}

			return false;
		}
	}
}
