// -----------------------------------------------------------------------
// <copyright file="DataSourceContract.cs">
// Copyright (c) 2013-2015 Andrey Veselov. All rights reserved.
// License: Apache License 2.0
// Contacts: http://andrey.moveax.com andrey@moveax.com
// </copyright>
// -----------------------------------------------------------------------

namespace LoreKeeper.Core.Contracts
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Linq.Expressions;

    [ContractClassFor(typeof(IDataSource))]
    internal abstract class DataSourceContract : IDataSource
    {
        public IQueryable<TEntity> Set<TEntity>() where TEntity : class
        {
            Contract.Ensures(Contract.Result<IQueryable<TEntity>>() != null);

            return default(IQueryable<TEntity>);
        }

        public IQueryable<TEntity> Set<TEntity>(params Expression<Func<TEntity, object>>[] include)
            where TEntity : class
        {
            Contract.Ensures(Contract.Result<IQueryable<TEntity>>() != null);

            return default(IQueryable<TEntity>);
        }
    }
}