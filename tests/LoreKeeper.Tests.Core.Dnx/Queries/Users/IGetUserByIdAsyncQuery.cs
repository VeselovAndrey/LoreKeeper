// -----------------------------------------------------------------------
// <copyright file="IGetUserByIdAsyncQuery.cs">
// Copyright (c) 2013-2015 Andrey Veselov. All rights reserved.
// License: Apache License 2.0
// Contacts: http://andrey.moveax.com andrey@moveax.com
// </copyright>
// -----------------------------------------------------------------------

namespace LoreKeeper.Tests.Core.Queries.Users
{
    using System.Diagnostics.Contracts;
    using System.Threading.Tasks;
    using LoreKeeper.Tests.Core.Models;

    [ContractClass(typeof(GetUserByIdAsyncQueryContract))]
    public interface IGetUserByIdAsyncQuery : IQuery
    {
        Task<User> ExecuteAsync(int userId, bool includeLinks);
    }

    [ContractClassFor(typeof(IGetUserByIdAsyncQuery))]
    internal abstract class GetUserByIdAsyncQueryContract : IGetUserByIdAsyncQuery
    {
        public Task<User> ExecuteAsync(int userId, bool includeLinks)
        {
            Contract.Requires(0 < userId);
            Contract.Ensures(Contract.Result<Task<User>>() != null);

            return default(Task<User>);
        }
    }
}