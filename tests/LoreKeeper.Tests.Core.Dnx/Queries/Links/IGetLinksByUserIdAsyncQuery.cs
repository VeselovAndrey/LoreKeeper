// -----------------------------------------------------------------------
// <copyright file="IGetLinksByUserIdAsyncQuery.cs">
// Copyright (c) 2013-2015 Andrey Veselov. All rights reserved.
// License: Apache License 2.0
// Contacts: http://andrey.moveax.com andrey@moveax.com
// </copyright>
// -----------------------------------------------------------------------

namespace LoreKeeper.Tests.Core.Queries.Links
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Threading.Tasks;
    using LoreKeeper.Tests.Core.Models;

    [ContractClass(typeof(GetLinksByUserIdAsyncQueryContract))]
    public interface IGetLinksByUserIdAsyncQuery : IQuery
    {
        Task<IEnumerable<Link>> ExecuteAsync(int userId);
    }

    [ContractClassFor(typeof(IGetLinksByUserIdAsyncQuery))]
    internal abstract class GetLinksByUserIdAsyncQueryContract : IGetLinksByUserIdAsyncQuery
    {
        public Task<IEnumerable<Link>> ExecuteAsync(int userId)
        {
            Contract.Requires(0 < userId);
            Contract.Ensures(Contract.Result<Task<IEnumerable<Link>>>() != null);

            return default(Task<IEnumerable<Link>>);
        }
    }
}