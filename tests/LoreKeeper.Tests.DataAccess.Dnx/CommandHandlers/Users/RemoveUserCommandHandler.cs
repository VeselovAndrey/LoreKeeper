﻿// -----------------------------------------------------------------------
// <copyright file="RemoveUserCommandHandler.cs">
// Copyright (c) 2013-2015 Andrey Veselov. All rights reserved.
// License: Apache License 2.0
// Contacts: http://andrey.moveax.com andrey@moveax.com
// </copyright>
// -----------------------------------------------------------------------

namespace LoreKeeper.Tests.DataAccess.CommandHandlers.Users
{
    using LoreKeeper.Tests.Core.Commands.Users;

    internal class RemoveUserCommandHandler : ICommandHandlerSync<RemoveUserCommand>
    {
        public void Execute(RemoveUserCommand command)
        {
        }
    }
}
