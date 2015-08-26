// -----------------------------------------------------------------------
// <copyright file="IDataSource.cs">
// Copyright (c) 2013-2015 Andrey Veselov. All rights reserved.
// License: Apache License 2.0
// Contacts: http://andrey.moveax.com andrey@moveax.com
// </copyright>
// -----------------------------------------------------------------------

namespace LoreKeeper.Core
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Diagnostics.Contracts;
    using LoreKeeper.Core.Contracts;

    /// <summary>Allows query underlying data source.</summary>
    [ContractClass(typeof(DataSourceContract))]
    public interface IDataSource
    {
        /// <summary>Returns IQueryable interface for the specified type.</summary>
        /// <typeparam name="TEntity">The entity type for which IQueryable interface should be returned.</typeparam>
        /// <returns>IQueryable interface for specified object type.</returns>
        IQueryable<TEntity> Set<TEntity>() where TEntity : class;


        /// <summary>Returns IQueryable interface for the specified type.</summary>
        /// <typeparam name="TEntity">The entity type for which IQueryable interface should be returned.</typeparam>
        /// <param name="include">The list of navigation properties to include in result.</param>
        /// <returns>IQueryable interface for specified object type.</returns>
        IQueryable<TEntity> Set<TEntity>(params Expression<Func<TEntity, object>>[] include) where TEntity : class;
    }
}