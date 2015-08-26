// -----------------------------------------------------------------------
// <copyright file="GetUserByIdAsyncQuery.cs">
// Copyright (c) 2013-2015 Andrey Veselov. All rights reserved.
// License: Apache License 2.0
// Contacts: http://andrey.moveax.com andrey@moveax.com
// </copyright>
// -----------------------------------------------------------------------

namespace LoreKeeper.EF6.Tests.DataAccess.Queries.Users
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using LoreKeeper.Core;
    using LoreKeeper.EF6.Tests.DataAccess.Database.Dto;
    using LoreKeeper.EF6.Tests.DataAccess.Mappers;
    using LoreKeeper.Tests.Core.Models;
    using LoreKeeper.Tests.Core.Queries.Users;

    internal class GetUserByIdAsyncQuery : IGetUserByIdAsyncQuery
    {
        private readonly IDataSource _dataSource;

        public GetUserByIdAsyncQuery(IDataSource dataSource)
        {
            this._dataSource = dataSource;
        }

        public async Task<User> ExecuteAsync(int userId, bool includeLinks)
        {
            var user = includeLinks ?
                await this._dataSource.Set<UserDto>(p => p.Links)
                    .FirstOrDefaultAsync(p => p.Id == userId)
                    .ConfigureAwait(false) :
                await this._dataSource.Set<UserDto>()
                    .FirstOrDefaultAsync(p => p.Id == userId)
                    .ConfigureAwait(false);

            return user.ToModel();
        }
    }
}