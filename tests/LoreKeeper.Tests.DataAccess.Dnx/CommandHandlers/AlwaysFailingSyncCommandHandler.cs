// -----------------------------------------------------------------------
// <copyright file="AlwaysFailingSyncCommandHandler..cs">
// Copyright (c) 2013-2015 Andrey Veselov. All rights reserved.
// License: Apache License 2.0
// Contacts: http://andrey.moveax.com andrey@moveax.com
// </copyright>
// -----------------------------------------------------------------------

namespace LoreKeeper.Tests.DataAccess.CommandHandlers
{
    using System;
    using System.Diagnostics.Contracts;
    using LoreKeeper.Core;
    using LoreKeeper.Tests.Core.Commands;

    internal class AlwaysFailingSyncCommandHandler : ICommandHandlerSync<AlwaysFailingSyncCommand>
    {
        public AlwaysFailingSyncCommandHandler(IRepository repository)
        {
            Contract.Requires(repository != null);
        }

        public void Execute(AlwaysFailingSyncCommand command)
        {
            throw new Exception("This is AlwaysFailingAsyncCommand");
        }
    }
}