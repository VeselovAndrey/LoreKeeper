// -----------------------------------------------------------------------
// <copyright file="UpdateUserEmailCommand.cs">
// Copyright (c) 2013-2015 Andrey Veselov. All rights reserved.
// License: Apache License 2.0
// Contacts: http://andrey.moveax.com andrey@moveax.com
// </copyright>
// -----------------------------------------------------------------------

namespace LoreKeeper.Tests.Core.Commands.Users
{
    public class UpdateUserEmailCommand : ICommand
    {
        public int Id { get; }

        public string Email { get; }

        public UpdateUserEmailCommand(int userId, string email)
        {
            this.Id = userId;
            this.Email = email;
        }
    }
}