// -----------------------------------------------------------------------
// <copyright file="CreateUserCommand.cs">
// Copyright (c) 2013-2015 Andrey Veselov. All rights reserved.
// License: Apache License 2.0
// Contacts: http://andrey.moveax.com andrey@moveax.com
// </copyright>
// -----------------------------------------------------------------------

namespace LoreKeeper.Tests.Core.Commands.Users
{
    using System;
    using System.Collections.Generic;
    using LoreKeeper.Tests.Core.Models;

    public class CreateUserCommand : ICommand
    {
        public int Id { get; set; }

        public string Name { get; }

        public string Email { get; }

        public bool IsDisabled { get; }

        public IEnumerable<Link> Links { get; }

        public DateTime Created { get; set; }

        public CreateUserCommand(string name, string email, bool isDisabled, IEnumerable<Link> links)
        {
            this.Id = default(int);
            this.Name = name;
            this.Email = email;
            this.IsDisabled = isDisabled;
            this.Links = links;
        }
    }
}
