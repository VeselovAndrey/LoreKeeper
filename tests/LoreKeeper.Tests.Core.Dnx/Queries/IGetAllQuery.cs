// -----------------------------------------------------------------------
// <copyright file="IGetAllQuery.cs">
// Copyright (c) 2013-2015 Andrey Veselov. All rights reserved.
// License: Apache License 2.0
// Contacts: http://andrey.moveax.com andrey@moveax.com
// </copyright>
// -----------------------------------------------------------------------

namespace LoreKeeper.Tests.Core.Queries
{
    using System.Collections.Generic;

    public interface IGetAllQuery : IQuery
    {
        IEnumerable<TEntity> Execute<TEntity>()
            where TEntity : class;
    }
}
