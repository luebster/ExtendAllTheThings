namespace ExtendAllTheThings.Extensions
{
	public static class ObjectExtensions
	{
		public static bool IsNull(this object target) => IsNull<object>(target);

		public static bool IsNull<T>(this T target) => target == null;

		public static bool IsNotNull(this object target) => IsNotNull<object>(target);

		public static bool IsNotNull<T>(this T target) => target is object;
	}
}