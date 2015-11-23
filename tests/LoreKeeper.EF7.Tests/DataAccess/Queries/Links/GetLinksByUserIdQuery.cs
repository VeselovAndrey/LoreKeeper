// -----------------------------------------------------------------------
// <copyright file="GetLinksByUserIdQuery.cs">
// Copyright (c) 2013-2015 Andrey Veselov. All rights reserved.
// License: Apache License 2.0
// Contacts: http://andrey.moveax.com andrey@moveax.com
// </copyright>
// -----------------------------------------------------------------------

namespace LoreKeeper.EF7.Tests.DataAccess.Queries.Links
{
    using System.Collections.Generic;
    using System.Linq;
    using LoreKeeper.Core;
    using LoreKeeper.EF7.Tests.DataAccess.Database.Dto;
    using LoreKeeper.EF7.Tests.DataAccess.Mappers;
    using LoreKeeper.Tests.Core.Models;
    using LoreKeeper.Tests.Core.Queries.Links;

    internal class GetLinksByUserIdQuery : IGetLinksByUserIdQuery
    {
        private readonly IDataSource _dataSource;

        public GetLinksByUserIdQuery(IDataSource dataSource)
        {
            this._dataSource = dataSource;
        }

        public IEnumerable<Link> Execute(int userId)
        {
            var links = this._dataSource.Set<LinkDto>()
                .Where(l => l.UserProfileId == userId)
                .ToList();

            return links.Select(l => l.ToModel()).ToArray();
        }
    }
}