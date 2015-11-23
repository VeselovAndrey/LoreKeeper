// -----------------------------------------------------------------------
// <copyright file="UpdateUserEmailCommandHandler.cs">
// Copyright (c) 2013-2015 Andrey Veselov. All rights reserved.
// License: Apache License 2.0
// Contacts: http://andrey.moveax.com andrey@moveax.com
// </copyright>
// -----------------------------------------------------------------------

namespace LoreKeeper.EF7.Tests.DataAccess.CommandHandlers.Users
{
    using System.Diagnostics.Contracts;
    using System.Threading;
    using System.Threading.Tasks;
    using LoreKeeper.Core;
    using LoreKeeper.EF7.Tests.DataAccess.Database.Dto;
    using LoreKeeper.Tests.Core.Commands.Users;
    using Microsoft.Data.Entity;

    internal class UpdateUserEmailCommandHandler : ICommandHandlerAsync<UpdateUserEmailCommand>
    {
        private readonly IRepository _repository;
        private readonly IDataSource _dataSource;

        public UpdateUserEmailCommandHandler(IRepository repository, IDataSource dataSource)
        {
            Contract.Requires(repository != null);
            Contract.Requires(dataSource != null);

            this._repository = repository;
            this._dataSource = dataSource;
        }

        public async Task ExecuteAsync(UpdateUserEmailCommand command, CancellationToken cancellationToken)
        {
            var user = await this._dataSource.Set<UserDto>()
                .FirstOrDefaultAsync(u => u.Id == command.Id, cancellationToken)
                .ConfigureAwait(false);

            if (user != null) {
                user.Email = command.Email;
                this._repository.Update(user);
            }
        }
    }
}