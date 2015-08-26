// -----------------------------------------------------------------------
// <copyright file="GetLinksByUserIdAsyncQuery.cs">
// Copyright (c) 2013-2015 Andrey Veselov. All rights reserved.
// License: Apache License 2.0
// Contacts: http://andrey.moveax.com andrey@moveax.com
// </copyright>
// -----------------------------------------------------------------------

namespace LoreKeeper.Tests.DataAccess.Queries.Links
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using LoreKeeper.Core;
    using LoreKeeper.Tests.Core.Models;
    using LoreKeeper.Tests.Core.Queries.Links;

    internal class GetLinksByUserIdAsyncQuery : IGetLinksByUserIdAsyncQuery
    {
        public async Task<IEnumerable<Link>> ExecuteAsync(int userId)
        {
            await Task.Delay(millisecondsDelay: 1);
            return Enumerable.Empty<Link>();
        }
    }
}