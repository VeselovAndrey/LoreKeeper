// -----------------------------------------------------------------------
// <copyright file="GetUserByIdAsyncQuery.cs">
// Copyright (c) 2013-2015 Andrey Veselov. All rights reserved.
// License: Apache License 2.0
// Contacts: http://andrey.moveax.com andrey@moveax.com
// </copyright>
// -----------------------------------------------------------------------

namespace LoreKeeper.Tests.DataAccess.Queries.Users
{
    using System.Threading.Tasks;
    using LoreKeeper.Tests.Core.Models;
    using LoreKeeper.Tests.Core.Queries.Users;

    internal class GetUserByIdAsyncQuery : IGetUserByIdAsyncQuery
    {
        public async Task<User> ExecuteAsync(int userId, bool includeLinks)
        {
            await Task.Delay(millisecondsDelay: 1);
            return default(User);
        }
    }
}