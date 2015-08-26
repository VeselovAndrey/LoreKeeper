// -----------------------------------------------------------------------
// <copyright file="CommandHandlerAsyncContract.cs">
// Copyright (c) 2013-2015 Andrey Veselov. All rights reserved.
// License: Apache License 2.0
// Contacts: http://andrey.moveax.com andrey@moveax.com
// </copyright>
// -----------------------------------------------------------------------

namespace LoreKeeper.Contracts
{
    using System.Diagnostics.Contracts;
    using System.Threading;
    using System.Threading.Tasks;

    [ContractClassFor(typeof(ICommandHandlerAsync<>))]
    internal abstract class CommandHandlerAsyncContract<TCommand> : ICommandHandlerAsync<TCommand>
        where TCommand : class, ICommand
    {
        public Task ExecuteAsync(TCommand command, CancellationToken cancellationToken)
        {
            Contract.Requires(command != null);
            Contract.Ensures(Contract.Result<Task>() != null);

            return default(Task);
        }
    }
}