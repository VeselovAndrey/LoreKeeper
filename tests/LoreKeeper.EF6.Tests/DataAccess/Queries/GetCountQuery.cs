// -----------------------------------------------------------------------
// <copyright file="GetCountQuery.cs">
// Copyright (c) 2013-2015 Andrey Veselov. All rights reserved.
// License: Apache License 2.0
// Contacts: http://andrey.moveax.com andrey@moveax.com
// </copyright>
// -----------------------------------------------------------------------

namespace LoreKeeper.EF6.Tests.DataAccess.Queries
{
    using System.Diagnostics.Contracts;
    using System.Linq;
    using LoreKeeper.Core;
    using LoreKeeper.Tests.Core.Queries;

    internal class GetCountQuery : IGetCountQuery
    {
        private readonly IDataSource _dataSource;

        public GetCountQuery(IDataSource dataSource)
        {
            Contract.Requires(dataSource != null);

            this._dataSource = dataSource;
        }

        public int Execute<TEntity>()
            where TEntity : class
        {
            return this._dataSource.Set<TEntity>()
                .Count();
        }
    }
}