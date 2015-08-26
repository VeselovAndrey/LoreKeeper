// -----------------------------------------------------------------------
// <copyright file="RepositoryContract.cs">
// Copyright (c) 2013-2015 Andrey Veselov. All rights reserved.
// License: Apache License 2.0
// Contacts: http://andrey.moveax.com andrey@moveax.com
// </copyright>
// -----------------------------------------------------------------------

namespace LoreKeeper.Core.Contracts
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Linq.Expressions;

    [ContractClassFor(typeof(IRepository))]
    internal abstract class RepositoryContract : IRepository
    {
        public void Add<TEntity>(TEntity entity)
            where TEntity : class
        {
            Contract.Ensures(entity != null);
        }

        public void Update<TEntity>(TEntity entity)
            where TEntity : class
        {
            Contract.Ensures(entity != null);
        }

        public void Update<TEntity>(TEntity entity, Expression<Func<TEntity, bool>> match, params Expression<Func<TEntity, object>>[] properties)
            where TEntity : class
        {
            Contract.Ensures(entity != null);
        }

        public void UpdateSpecifiedProperties<TEntity>(TEntity entity, params Expression<Func<TEntity, object>>[] properties)
            where TEntity : class
        {
            Contract.Requires(entity != null);
            Contract.Requires(properties != null);
        }

        public void UpdateExcludeProperties<TEntity>(TEntity entity, params Expression<Func<TEntity, object>>[] properties)
            where TEntity : class
        {
            Contract.Requires(entity != null);
            Contract.Requires(properties != null);
        }

        public void Remove<TEntity>(TEntity entity)
            where TEntity : class
        {
            Contract.Requires(entity != null);
        }

        public void Remove<TEntity>(Expression<Func<TEntity, bool>> match)
            where TEntity : class
        {
            Contract.Requires(match != null);
        }
    }
}