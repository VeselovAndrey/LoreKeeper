// -----------------------------------------------------------------------
// <copyright file="CreateLinkCommandHandler.cs">
// Copyright (c) 2014-2015 Andrey Veselov. All rights reserved.
// License: Apache License 2.0
// Contacts: http://andrey.moveax.com andrey@moveax.com
// </copyright>
// -----------------------------------------------------------------------

namespace LoreKeeper.Tests.DataAccess.CommandHandlers.Links
{
    using LoreKeeper.Tests.Core.Commands.Links;

    internal class CreateLinkCommandHandler : ICommandHandlerSync<CreateLinkCommand>, ICommandHandlerTransactionSupport
    {
        public void Execute(CreateLinkCommand command)
        {
        }

        public void OnCommitted()
        {
        }
    }
}
