// -----------------------------------------------------------------------
// <copyright file="IRepository.cs">
// Copyright (c) 2013-2015 Andrey Veselov. All rights reserved.
// License: Apache License 2.0
// Contacts: http://andrey.moveax.com andrey@moveax.com
// </copyright>
// -----------------------------------------------------------------------

namespace LoreKeeper.Core
{
    using System;
    using System.Linq.Expressions;
    using System.Diagnostics.Contracts;
    using LoreKeeper.Core.Contracts;

    /// <summary>The repository interface.</summary>
    [ContractClass(typeof(RepositoryContract))]
    public interface IRepository
    {
        /// <summary>Adds the TEntity object to the repository.</summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="entity">The object to add.</param>
        void Add<TEntity>(TEntity entity)
            where TEntity : class;

        /// <summary>Updates the specified entity.</summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="entity">The object to update.</param>
        void Update<TEntity>(TEntity entity)
            where TEntity : class;

        /// <summary>Updates the specified entities.</summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="entity">The object that contains new values.</param>
        /// <param name="match">The query that describes objects to updated.</param>
        /// <param name="properties">Properties to update.</param>
        void Update<TEntity>(TEntity entity, Expression<Func<TEntity, bool>> match, params Expression<Func<TEntity, object>>[] properties)
            where TEntity : class;

        /// <summary>Updates specified properties from the specified entity.</summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="entity">The object to update.</param>
        /// <param name="properties">Properties to update.</param>
        void UpdateSpecifiedProperties<TEntity>(TEntity entity, params Expression<Func<TEntity, object>>[] properties)
            where TEntity : class;

        /// <summary>Updates the specified entity excluding selected properties.</summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="entity">The object to update.</param>
        /// <param name="properties">Properties to ignore.</param>
        void UpdateExcludeProperties<TEntity>(TEntity entity, params Expression<Func<TEntity, object>>[] properties)
            where TEntity : class;

        /// <summary>Removes the object from the repository.</summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="entity">The object to delete.</param>
        void Remove<TEntity>(TEntity entity)
            where TEntity : class;

        /// <summary>Removes specified objects from the repository.</summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="match">The query that describes objects to deleted.</param>
        void Remove<TEntity>(Expression<Func<TEntity, bool>> match)
            where TEntity : class;
    }
}