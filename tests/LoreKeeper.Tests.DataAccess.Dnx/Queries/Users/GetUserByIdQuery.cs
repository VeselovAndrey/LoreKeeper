// -----------------------------------------------------------------------
// <copyright file="GetUserByIdQuery.cs">
// Copyright (c) 2013-2015 Andrey Veselov. All rights reserved.
// License: Apache License 2.0
// Contacts: http://andrey.moveax.com andrey@moveax.com
// </copyright>
// -----------------------------------------------------------------------

namespace LoreKeeper.Tests.DataAccess.Queries.Users
{
    using LoreKeeper.Tests.Core.Models;
    using LoreKeeper.Tests.Core.Queries.Users;

    internal class GetUserByIdQuery : IGetUserByIdQuery
    {
        public User Execute(int userId, bool includeLinks)
        {
            return default(User);
        }
    }
}