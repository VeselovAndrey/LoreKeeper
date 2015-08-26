// -----------------------------------------------------------------------
// <copyright file="RemoveUserCommand.cs">
// Copyright (c) 2013-2015 Andrey Veselov. All rights reserved.
// License: Apache License 2.0
// Contacts: http://andrey.moveax.com andrey@moveax.com
// </copyright>
// -----------------------------------------------------------------------

namespace LoreKeeper.Tests.Core.Commands.Users
{
    public class RemoveUserCommand : ICommand
    {
        public int UserId { get; }

        public RemoveUserCommand(int userId)
        {
            this.UserId = userId;
        }
    }
}