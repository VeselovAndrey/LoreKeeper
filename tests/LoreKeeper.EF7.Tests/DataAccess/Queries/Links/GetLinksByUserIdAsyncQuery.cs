// -----------------------------------------------------------------------
// <copyright file="GetLinksByUserIdAsyncQuery.cs">
// Copyright (c) 2013-2015 Andrey Veselov. All rights reserved.
// License: Apache License 2.0
// Contacts: http://andrey.moveax.com andrey@moveax.com
// </copyright>
// -----------------------------------------------------------------------

namespace LoreKeeper.EF7.Tests.DataAccess.Queries.Links
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using LoreKeeper.Core;
    using LoreKeeper.EF7.Tests.DataAccess.Database.Dto;
    using LoreKeeper.EF7.Tests.DataAccess.Mappers;
    using LoreKeeper.Tests.Core.Models;
    using LoreKeeper.Tests.Core.Queries.Links;
    using Microsoft.Data.Entity;

    internal class GetLinksByUserIdAsyncQuery : IGetLinksByUserIdAsyncQuery
    {
        private readonly IDataSource _dataSource;

        public GetLinksByUserIdAsyncQuery(IDataSource dataSource)
        {
            this._dataSource = dataSource;
        }

        public async Task<IEnumerable<Link>> ExecuteAsync(int userId)
        {
            var links = await this._dataSource.Set<LinkDto>()
                .Where(l => l.UserProfileId == userId)
                .ToListAsync()
                .ConfigureAwait(false);

            return links.Select(l => l.ToModel()).ToArray();
        }
    }
}