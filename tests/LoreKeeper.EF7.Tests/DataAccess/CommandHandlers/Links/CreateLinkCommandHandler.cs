// -----------------------------------------------------------------------
// <copyright file="CreateLinkCommandHandler.cs">
// Copyright (c) 2014-2015 Andrey Veselov. All rights reserved.
// License: Apache License 2.0
// Contacts: http://andrey.moveax.com andrey@moveax.com
// </copyright>
// -----------------------------------------------------------------------

namespace LoreKeeper.EF7.Tests.DataAccess.CommandHandlers.Links
{
    using System.Diagnostics.Contracts;
    using LoreKeeper.Core;
    using LoreKeeper.EF7.Tests.DataAccess.Database.Dto;
    using LoreKeeper.Tests.Core.Commands.Links;

    internal class CreateLinkCommandHandler : ICommandHandlerSync<CreateLinkCommand>, ICommandHandlerTransactionSupport
    {
        private readonly IRepository _repository;
        private LinkDto _link;
        private CreateLinkCommand _command;

        public CreateLinkCommandHandler(IRepository repository)
        {
            Contract.Requires(repository != null);

            this._repository = repository;
        }

        public void Execute(CreateLinkCommand command)
        {
            this._command = command;

            this._link = new LinkDto() {
                Id = default(int),
                Title = command.Title,
                Url = command.Url,
                UserProfileId = command.UserId
            };

            this._repository.Add(this._link);
        }

        public void OnCommitted()
        {
            this._command.Id = this._link.Id;
        }
    }
}
