// -----------------------------------------------------------------------
// <copyright file="GetAllQuery.cs">
// Copyright (c) 2013-2015 Andrey Veselov. All rights reserved.
// License: Apache License 2.0
// Contacts: http://andrey.moveax.com andrey@moveax.com
// </copyright>
// -----------------------------------------------------------------------

namespace LoreKeeper.EF7.Tests.DataAccess.Queries
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using LoreKeeper.Core;
    using LoreKeeper.Tests.Core.Queries;

    internal class GetAllQuery : IGetAllQuery
    {
        private readonly IDataSource _dataSource;

        public GetAllQuery(IDataSource dataSource)
        {
            Contract.Requires(dataSource != null);

            this._dataSource = dataSource;
        }

        public IEnumerable<TEntity> Execute<TEntity>()
            where TEntity : class
        {
            return this._dataSource.Set<TEntity>()
                .ToList();
        }
    }
}
