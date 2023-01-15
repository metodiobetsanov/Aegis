namespace Aegis.Persistence.Repositories
{
	using System;
	using System.Collections.Generic;
	using System.Linq.Expressions;

	using Aegis.Persistence.Contracts;

	using Microsoft.EntityFrameworkCore;

	/// <summary>
	/// Repository
	/// </summary>
	/// <typeparam name="TEntity">The type of the entity.</typeparam>
	/// <seealso cref="Aegis.Persistence.Contracts.IRepository&lt;TEntity&gt;" />
	public sealed class Repository<TEntity> : IRepository<TEntity>
		where TEntity : class
	{
		/// <summary>
		/// The context
		/// </summary>
		private readonly DbContext _context;

		/// <summary>
		/// Initializes a new instance of the <see cref="Repository{TEntity}"/> class.
		/// </summary>
		/// <param name="context">The context.</param>
		public Repository(DbContext context)
		{
			_context = context;
		}

		/// <summary>
		/// Gets the entities.
		/// </summary>
		/// <returns></returns>
		/// <exception cref="System.NotImplementedException"></exception>
		public IEnumerable<TEntity> GetEntities()
			=> _context.Set<TEntity>();

		/// <summary>
		/// Finds the by condition.
		/// </summary>
		/// <param name="expression">The expression.</param>
		/// <returns></returns>
		/// <exception cref="System.NotImplementedException"></exception>
		public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> expression)
			=> _context.Set<TEntity>().Where(expression);

		/// <summary>
		/// Creates the specified entity.
		/// </summary>
		/// <param name="entity">The entity.</param>
		/// <exception cref="System.NotImplementedException"></exception>
		public void Create(TEntity entity)
			=> _context.Set<TEntity>().Add(entity);

		/// <summary>
		/// Updates the specified entity.
		/// </summary>
		/// <param name="entity">The entity.</param>
		/// <exception cref="System.NotImplementedException"></exception>
		public void Update(TEntity entity)
			=> _context.Set<TEntity>().Update(entity);

		/// <summary>
		/// Deletes the specified entity.
		/// </summary>
		/// <param name="entity">The entity.</param>
		/// <exception cref="System.NotImplementedException"></exception>
		public void Delete(TEntity entity)
			=> _context.Set<TEntity>().Remove(entity);
	}
}
