// -----------------------------------------------------------------------
// <copyright file="AlwaysSuccessAsyncCommandHandler.cs">
// Copyright (c) 2013-2015 Andrey Veselov. All rights reserved.
// License: Apache License 2.0
// Contacts: http://andrey.moveax.com andrey@moveax.com
// </copyright>
// -----------------------------------------------------------------------

namespace LoreKeeper.EF6.Tests.DataAccess.CommandHandlers
{
    using System.Diagnostics.Contracts;
    using System.Threading;
    using System.Threading.Tasks;
    using LoreKeeper;
    using LoreKeeper.Core;
    using LoreKeeper.Tests.Core.Commands;

    internal class AlwaysSuccessAsyncCommandHandler : ICommandHandlerAsync<AlwaysSuccessAsyncCommand>
    {
        public AlwaysSuccessAsyncCommandHandler(IRepository repository)
        {
            Contract.Requires(repository != null);
        }

        public Task ExecuteAsync(AlwaysSuccessAsyncCommand command, CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<bool>();
            tcs.SetResult(true);

            return tcs.Task;
        }
    }
}