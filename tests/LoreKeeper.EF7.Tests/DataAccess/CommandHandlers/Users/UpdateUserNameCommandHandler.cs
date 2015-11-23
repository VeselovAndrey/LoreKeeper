// -----------------------------------------------------------------------
// <copyright file="UpdateUserNameCommandHandler.cs">
// Copyright (c) 2013-2015 Andrey Veselov. All rights reserved.
// License: Apache License 2.0
// Contacts: http://andrey.moveax.com andrey@moveax.com
// </copyright>
// -----------------------------------------------------------------------

namespace LoreKeeper.EF7.Tests.DataAccess.CommandHandlers.Users
{
    using System.Diagnostics.Contracts;
    using LoreKeeper.Core;
    using LoreKeeper.EF7.Tests.DataAccess.Database.Dto;
    using LoreKeeper.Tests.Core.Commands.Users;

    internal class UpdateUserNameCommandHandler : ICommandHandlerSync<UpdateUserNameCommand>
    {
        private readonly IRepository _repository;

        public UpdateUserNameCommandHandler(IRepository repository)
        {
            Contract.Requires(repository != null);

            this._repository = repository;
        }

        public void Execute(UpdateUserNameCommand command)
        {
            var userProfile = new UserDto() {
                Id = command.Id,
                Name = command.Name,

                // Following fields are required by EF validation engine, but should be ignored on update.
                Email = "fake@email.com"
            };

            this._repository.UpdateSpecifiedProperties(userProfile, p => p.Name);
        }
    }
}