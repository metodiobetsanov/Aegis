#region copyright
//----------------------------------------------------------------------
// Copyright 2023 MNB Software
// Licensed under the Apache License, Version 2.0
// You may obtain a copy at http://www.apache.org/licenses/LICENSE-2.0
//----------------------------------------------------------------------
#endregion
namespace Aegis.Persistence.Contracts
{
	using System.Linq.Expressions;

	using Aegis.Persistence.Entities;

	/// <summary>
	/// Aegis Repository Interface
	/// </summary>
	/// <typeparam name="TEntity">The type of the entity.</typeparam>
	public interface IRepository<TEntity>
		where TEntity : class
	{
		/// <summary>
		/// Gets the entities.
		/// </summary>
		/// <returns></returns>
		IEnumerable<TEntity> GetEntities();

		/// <summary>
		/// Finds the by condition.
		/// </summary>
		/// <param name="expression">The expression.</param>
		/// <returns></returns>
		IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> expression);

		/// <summary>
		/// Creates the specified entity.
		/// </summary>
		/// <param name="entity">The entity.</param>
		void Create(TEntity entity);

		/// <summary>
		/// Updates the specified entity.
		/// </summary>
		/// <param name="entity">The entity.</param>
		void Update(TEntity entity);

		/// <summary>
		/// Deletes the specified entity.
		/// </summary>
		/// <param name="entity">The entity.</param>
		void Delete(TEntity entity);
	}
}
