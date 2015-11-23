// -----------------------------------------------------------------------
// <copyright file="UserDto.cs">
// Copyright (c) 2012-2015 Andrey Veselov. All rights reserved.
// License: Apache License 2.0
// Contacts: http://andrey.moveax.com andrey@moveax.com
// </copyright>
// -----------------------------------------------------------------------

namespace LoreKeeper.EF7.Tests.DataAccess.Database.Dto
{
    using System;
    using System.Collections.Generic;
    using LoreKeeper.Tests.Core.Models;

    internal class UserDto
    {
        public int Id { get; set; }

        public UserType Type { get; set; }

        public DateTime Created { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public bool IsDisabled { get; set; }

        // Navigation properties
        public virtual ICollection<LinkDto> Links { get; set; }
    }
}
