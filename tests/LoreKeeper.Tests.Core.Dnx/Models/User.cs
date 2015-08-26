// -----------------------------------------------------------------------
// <copyright file="UserProfile.cs">
// Copyright (c) 2013-2015 Andrey Veselov. All rights reserved.
// License: Apache License 2.0
// Contacts: http://andrey.moveax.com andrey@moveax.com
// </copyright>
// -----------------------------------------------------------------------

namespace LoreKeeper.Tests.Core.Models
{
    using System;
    using System.Collections.Generic;

    public class User
    {
        public int Id { get; set; }

        public UserType Type { get; set; }

        public DateTime Created { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public bool IsDisabled { get; set; }

        // Navigation properties
        public virtual ICollection<Link> Links { get; set; }
    }
}