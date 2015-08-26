// -----------------------------------------------------------------------
// <copyright file="IGetUserByIdQuery.cs">
// Copyright (c) 2013-2015 Andrey Veselov. All rights reserved.
// License: Apache License 2.0
// Contacts: http://andrey.moveax.com andrey@moveax.com
// </copyright>
// -----------------------------------------------------------------------

namespace LoreKeeper.Tests.Core.Queries.Users
{
    using System.Diagnostics.Contracts;
    using LoreKeeper.Tests.Core.Models;

    [ContractClass(typeof(GetUserByIdQueryContract))]
    public interface IGetUserByIdQuery : IQuery
    {
        User Execute(int userId, bool includeLinks);
    }

    [ContractClassFor(typeof(IGetUserByIdQuery))]
    internal abstract class GetUserByIdQueryContract : IGetUserByIdQuery
    {
        public User Execute(int userId, bool includeLinks)
        {
            Contract.Requires(0 < userId);

            return default(User);
        }
    }
}