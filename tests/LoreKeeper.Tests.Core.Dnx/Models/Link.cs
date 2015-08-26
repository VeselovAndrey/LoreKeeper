// -----------------------------------------------------------------------
// <copyright file="Link.cs">
// Copyright (c) 2013-2015 Andrey Veselov. All rights reserved.
// License: Apache License 2.0
// Contacts: http://andrey.moveax.com andrey@moveax.com
// </copyright>
// -----------------------------------------------------------------------

namespace LoreKeeper.Tests.Core.Models
{
    public class Link
    {
        public int Id { get; set; }

        public int UserProfileId { get; set; }

        public string Title { get; set; }

        public string Url { get; set; }

        // Navigation properties
        public virtual User UserProfile { get; set; }
    }
}
