// -----------------------------------------------------------------------
// <copyright file="LinkDtoMapperExtension.cs">
// Copyright (c) 2014-2015 Andrey Veselov. All rights reserved.
// License: Apache License 2.0
// Contacts: http://andrey.moveax.com andrey@moveax.com
// </copyright>
// -----------------------------------------------------------------------

namespace LoreKeeper.EF7.Tests.DataAccess.Mappers
{
    using LoreKeeper.EF7.Tests.DataAccess.Database.Dto;
    using LoreKeeper.Tests.Core.Models;

    internal static class LinkDtoMapperExtension
    {
        public static Link ToModel(this LinkDto source)
        {
            return source.ToModel(ignoreNavigation: false);
        }

        public static Link ToModel(this LinkDto source, bool ignoreNavigation)
        {
            if (source == null)
                return null;

            var link = new Link() {
                // In alphabetical order without navigation properties
                Id = source.Id,
                Title = source.Title,
                Url = source.Url,
                UserProfileId = source.UserProfileId
            };

            if (source.UserProfile != null && !ignoreNavigation)
                link.UserProfile = source.UserProfile.ToModel(ignoreNavigation: true);

            return link;
        }
    }
}