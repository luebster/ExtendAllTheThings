namespace ExtendAllTheThings.Extensions
{
	public static class ObjectExtensions
	{
		public static bool IsNull(this object target)
		{
			bool ret = IsNull<object>(target);
			return ret;
		}

		public static bool IsNull<T>(this T target)
		{
			bool result = target == null;
			return result;
		}

		public static bool IsNotNull(this object target)
		{
			bool ret = IsNotNull<object>(target);
			return ret;
		}

		public static bool IsNotNull<T>(this T target)
		{
			bool result = target is object;
			return result;
		}
	}
}