// -----------------------------------------------------------------------
// <copyright file="EFDataSource.cs">
// Copyright (c) 2013-2015 Andrey Veselov. All rights reserved.
// License: Apache License 2.0
// Contacts: http://andrey.moveax.com andrey@moveax.com
// </copyright>
// -----------------------------------------------------------------------

namespace LoreKeeper.EF6
{
    using System;
    using System.Data.Entity;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Linq.Expressions;
    using LoreKeeper.Core;

    internal class EFDataSource : IDataSource
    {
        private readonly DbContext _context;

        public EFDataSource(DbContext context)
        {
            Contract.Requires(context != null);

            this._context = context;
        }

        public IQueryable<TEntity> Set<TEntity>() where TEntity : class
        {
            return this._context.Set<TEntity>().AsNoTracking();
        }

        public IQueryable<TEntity> Set<TEntity>(params Expression<Func<TEntity, object>>[] include) where TEntity : class
        {
            IQueryable<TEntity> setQuery = this._context.Set<TEntity>();
            setQuery = include.Aggregate(setQuery, (current, property) => current.Include(property));

            return setQuery.AsNoTracking();
        }
    }
}