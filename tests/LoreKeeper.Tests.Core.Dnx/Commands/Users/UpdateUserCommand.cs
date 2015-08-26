// -----------------------------------------------------------------------
// <copyright file="UpdateUserCommand.cs">
// Copyright (c) 2013-2015 Andrey Veselov. All rights reserved.
// License: Apache License 2.0
// Contacts: http://andrey.moveax.com andrey@moveax.com
// </copyright>
// -----------------------------------------------------------------------

namespace LoreKeeper.Tests.Core.Commands.Users
{
    public class UpdateUserCommand : ICommand
    {
        public int Id { get; }

        public string Name { get; }

        public string Email { get; }

        public bool IsDisabled { get; set; }

        public UpdateUserCommand(int userId, string name, string email, bool isDisabled)
        {
            this.Id = userId;
            this.Name = name;
            this.Email = email;
            this.IsDisabled = isDisabled;
        }
    }
}