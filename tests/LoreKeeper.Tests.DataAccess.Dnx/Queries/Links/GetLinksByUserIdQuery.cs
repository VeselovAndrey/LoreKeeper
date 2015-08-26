// -----------------------------------------------------------------------
// <copyright file="GetLinksByUserIdQuery.cs">
// Copyright (c) 2013-2015 Andrey Veselov. All rights reserved.
// License: Apache License 2.0
// Contacts: http://andrey.moveax.com andrey@moveax.com
// </copyright>
// -----------------------------------------------------------------------

namespace LoreKeeper.Tests.DataAccess.Queries.Links
{
    using System.Collections.Generic;
    using System.Linq;
    using LoreKeeper.Tests.Core.Models;
    using LoreKeeper.Tests.Core.Queries.Links;

    internal class GetLinksByUserIdQuery : IGetLinksByUserIdQuery
    {
        public IEnumerable<Link> Execute(int userId)
        {
            return Enumerable.Empty<Link>();
        }
    }
}