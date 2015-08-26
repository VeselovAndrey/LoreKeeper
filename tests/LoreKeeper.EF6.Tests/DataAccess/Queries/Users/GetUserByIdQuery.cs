// -----------------------------------------------------------------------
// <copyright file="GetUserByIdQuery.cs">
// Copyright (c) 2013-2015 Andrey Veselov. All rights reserved.
// License: Apache License 2.0
// Contacts: http://andrey.moveax.com andrey@moveax.com
// </copyright>
// -----------------------------------------------------------------------

namespace LoreKeeper.EF6.Tests.DataAccess.Queries.Users
{
    using System.Linq;
    using LoreKeeper.Core;
    using LoreKeeper.EF6.Tests.DataAccess.Database.Dto;
    using LoreKeeper.EF6.Tests.DataAccess.Mappers;
    using LoreKeeper.Tests.Core.Models;
    using LoreKeeper.Tests.Core.Queries.Users;

    internal class GetUserByIdQuery : IGetUserByIdQuery
    {
        private readonly IDataSource _dataSource;

        public GetUserByIdQuery(IDataSource dataSource)
        {
            this._dataSource = dataSource;
        }

        public User Execute(int userId, bool includeLinks)
        {
            var user = includeLinks ?
                this._dataSource.Set<UserDto>(p => p.Links).FirstOrDefault(p => p.Id == userId) :
                this._dataSource.Set<UserDto>().FirstOrDefault(p => p.Id == userId);

            return user.ToModel();
        }
    }
}