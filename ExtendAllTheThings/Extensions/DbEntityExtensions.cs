using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ExtendAllTheThings.Extensions
{
	public static class DbEntityExtensions
	{
		/// <summary>
		/// Remove a collection of items from a DbSet using a lambda expression
		/// </summary>
		/// <typeparam name="TEntity"></typeparam>
		/// <param name="entities"></param>
		/// <param name="predicate"></param>
		/// <example>
		/// This sample shows how to call the <see cref="RemoveRange"/> method.
		/// <code>
		/// class TestClass
		/// {
		///     static void Main(DbContext context)
		///     {
		///         context.Users.RemoveRange(x => x.LastName == "Davis");
		///     }
		/// }
		/// </code>
		/// </example>
		public static void RemoveRange<TEntity>(this DbSet<TEntity> entities, Expression<Func<TEntity, bool>> predicate) where TEntity : class
		{
			List<TEntity> records = entities
					.Where(predicate)
					.ToList();

			if (records.Count > 0)
			{
				entities.RemoveRange(records);
			}
		}
	}
}
