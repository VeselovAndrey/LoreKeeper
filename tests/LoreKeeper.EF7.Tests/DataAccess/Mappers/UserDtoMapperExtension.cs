// -----------------------------------------------------------------------
// <copyright file="UserDtoMapperExtension.cs">
// Copyright (c) 2014-2015 Andrey Veselov. All rights reserved.
// License: Apache License 2.0
// Contacts: http://andrey.moveax.com andrey@moveax.com
// </copyright>
// -----------------------------------------------------------------------

namespace LoreKeeper.EF7.Tests.DataAccess.Mappers
{
    using System.Linq;
    using LoreKeeper.EF7.Tests.DataAccess.Database.Dto;
    using LoreKeeper.Tests.Core.Models;

    internal static class UserDtoMapperExtension
    {
        public static User ToModel(this UserDto source)
        {
            return source.ToModel(ignoreNavigation: false);
        }

        public static User ToModel(this UserDto source, bool ignoreNavigation)
        {
            if (source == null)
                return null;

            var user = new User() {
                // In alphabetical order without navigation properties
                Created = source.Created,
                Email = source.Email,
                Id = source.Id,
                IsDisabled = source.IsDisabled,
                Name = source.Name,
                Type = source.Type
            };

            if (source.Links != null && !ignoreNavigation)
                user.Links = source.Links.Select(l => l.ToModel(ignoreNavigation: true)).ToArray();

            return user;
        }
    }
}
