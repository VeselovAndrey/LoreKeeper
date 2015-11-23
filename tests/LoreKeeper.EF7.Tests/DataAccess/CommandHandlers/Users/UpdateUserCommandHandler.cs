// -----------------------------------------------------------------------
// <copyright file="UpdateUserCommandHandler.cs">
// Copyright (c) 2013-2015 Andrey Veselov. All rights reserved.
// License: Apache License 2.0
// Contacts: http://andrey.moveax.com andrey@moveax.com
// </copyright>
// -----------------------------------------------------------------------

namespace LoreKeeper.EF7.Tests.DataAccess.CommandHandlers.Users
{
    using System.Diagnostics.Contracts;
    using System.Linq;
    using LoreKeeper.Core;
    using LoreKeeper.EF7.Tests.DataAccess.Database.Dto;
    using LoreKeeper.Tests.Core.Commands.Users;

    internal class UpdateUserCommandHandler : ICommandHandlerSync<UpdateUserCommand>
    {
        private readonly IRepository _repository;
        private readonly IDataSource _dataSource;

        public UpdateUserCommandHandler(IRepository repository, IDataSource dataSource)
        {
            Contract.Requires(repository != null);
            Contract.Requires(dataSource != null);

            this._repository = repository;
            this._dataSource = dataSource;
        }

        public void Execute(UpdateUserCommand command)
        {
            var userInfo = this._dataSource
                .Set<UserDto>()
                .FirstOrDefault(u => u.Id == command.Id);

            if (userInfo != null) {
                userInfo.Name = command.Name;
                userInfo.Email = command.Email;
                userInfo.IsDisabled = command.IsDisabled;

                this._repository.Update(userInfo);
            }
        }
    }
}
