// -----------------------------------------------------------------------
// <copyright file="CreateUserCommandHandler.cs">
// Copyright (c) 2013-2015 Andrey Veselov. All rights reserved.
// License: Apache License 2.0
// Contacts: http://andrey.moveax.com andrey@moveax.com
// </copyright>
// -----------------------------------------------------------------------

namespace LoreKeeper.EF6.Tests.DataAccess.CommandHandlers.Users
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using LoreKeeper.Core;
    using LoreKeeper.EF6.Tests.DataAccess.Database.Dto;
    using LoreKeeper.Tests.Core.Commands.Users;

    internal class CreateUserCommandHandler : ICommandHandlerSync<CreateUserCommand>, ICommandHandlerTransactionSupport
    {
        private readonly IRepository _repository;
        private UserDto _user;
        private CreateUserCommand _command;

        public CreateUserCommandHandler(IRepository repository)
        {
            Contract.Requires(repository != null);

            this._repository = repository;
        }

        public void Execute(CreateUserCommand command)
        {
            this._command = command;

            this._user = new UserDto() {
                Id = default(int),
                Name = command.Name,
                Email = command.Email,
                IsDisabled = command.IsDisabled,
                Created = DateTime.UtcNow
            };

            if (command.Links != null) {
                this._user.Links = new List<LinkDto>();

                foreach (var link in command.Links) {
                    this._user.Links.Add(new LinkDto() {
                        Title = link.Title,
                        Url = link.Url
                    });
                }
            }

            this._repository.Add(this._user);
        }

        public void OnCommitted()
        {
            this._command.Id = this._user.Id;
            this._command.Created = this._user.Created;
        }
    }
}
