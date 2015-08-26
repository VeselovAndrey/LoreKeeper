// -----------------------------------------------------------------------
// <copyright file="RemoveDisabledUsersCommandHandler.cs">
// Copyright (c) 2013-2015 Andrey Veselov. All rights reserved.
// License: Apache License 2.0
// Contacts: http://andrey.moveax.com andrey@moveax.com
// </copyright>
// -----------------------------------------------------------------------

namespace LoreKeeper.EF6.Tests.DataAccess.CommandHandlers.Users
{
    using System.Diagnostics.Contracts;
    using LoreKeeper.Core;
    using LoreKeeper.EF6.Tests.DataAccess.Database.Dto;
    using LoreKeeper.Tests.Core.Commands.Users;

    internal class RemoveDisabledUsersCommandHandler : ICommandHandlerSync<RemoveDisabledUsersCommand>
    {
        private readonly IRepository _repository;

        public RemoveDisabledUsersCommandHandler(IRepository repository)
        {
            Contract.Requires(repository != null);

            this._repository = repository;
        }

        public void Execute(RemoveDisabledUsersCommand command)
        {
            this._repository.Remove<UserDto>(u => u.IsDisabled);
        }
    }
}
