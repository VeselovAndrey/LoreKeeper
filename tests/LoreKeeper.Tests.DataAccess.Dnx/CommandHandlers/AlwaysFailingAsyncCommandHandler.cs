// -----------------------------------------------------------------------
// <copyright file="AlwaysFailingAsyncCommandHandler.cs">
// Copyright (c) 2013-2015 Andrey Veselov. All rights reserved.
// License: Apache License 2.0
// Contacts: http://andrey.moveax.com andrey@moveax.com
// </copyright>
// -----------------------------------------------------------------------

namespace LoreKeeper.Tests.DataAccess.CommandHandlers
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Threading;
    using System.Threading.Tasks;
    using LoreKeeper.Core;
    using LoreKeeper.Tests.Core.Commands;

    internal class AlwaysFailingAsyncCommandHandler : ICommandHandlerAsync<AlwaysFailingAsyncCommand>
    {
        public AlwaysFailingAsyncCommandHandler(IRepository repository)
        {
            Contract.Requires(repository != null);
        }

        public Task ExecuteAsync(AlwaysFailingAsyncCommand command, CancellationToken cancellationToken)
        {
            throw new Exception("This is AlwaysFailingAsyncCommand");
        }
    }
}