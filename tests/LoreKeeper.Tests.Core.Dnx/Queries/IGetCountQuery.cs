// -----------------------------------------------------------------------
// <copyright file="IGetCountQuery.cs">
// Copyright (c) 2013-2015 Andrey Veselov. All rights reserved.
// License: Apache License 2.0
// Contacts: http://andrey.moveax.com andrey@moveax.com
// </copyright>
// -----------------------------------------------------------------------

namespace LoreKeeper.Tests.Core.Queries
{
    using System.Diagnostics.Contracts;

    [ContractClass(typeof(GetCountQueryContract))]
    public interface IGetCountQuery : IQuery
    {
        int Execute<TEntity>()
            where TEntity : class;
    }

    [ContractClassFor(typeof(IGetCountQuery))]
    internal abstract class GetCountQueryContract : IGetCountQuery
    {
        public int Execute<TEntity>()
            where TEntity : class
        {
            Contract.Ensures(0 <= Contract.Result<int>());

            return default(int);
        }
    }
}
