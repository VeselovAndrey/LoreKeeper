// -----------------------------------------------------------------------
// <copyright file="UpdateLinkTitleCommandHandler.cs">
// Copyright (c) 2013-2015 Andrey Veselov. All rights reserved.
// License: Apache License 2.0
// Contacts: http://andrey.moveax.com andrey@moveax.com
// </copyright>
// -----------------------------------------------------------------------

namespace LoreKeeper.EF6.Tests.DataAccess.CommandHandlers.Links
{
    using System.Diagnostics.Contracts;
    using LoreKeeper.Core;
    using LoreKeeper.EF6.Tests.DataAccess.Database.Dto;
    using LoreKeeper.Tests.Core.Commands.Links;

    internal class UpdateLinkTitleCommandHandler : ICommandHandlerSync<UpdateLinkTitleCommand>
    {
        private readonly IRepository _repository;

        public UpdateLinkTitleCommandHandler(IRepository repository)
        {
            Contract.Requires(repository != null);

            this._repository = repository;
        }

        public void Execute(UpdateLinkTitleCommand command)
        {
            var link = new LinkDto() {
                Id = command.Id,
                Title = command.Title,
                Url = "http://somesite.com" // value for EF validation engine, but it should be ignored on update
            };

            this._repository.UpdateExcludeProperties(link,
                l => l.Url,
                l => l.UserProfileId);
        }
    }
}
