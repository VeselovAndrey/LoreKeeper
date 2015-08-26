// -----------------------------------------------------------------------
// <copyright file="ICommandHandlerAsync.cs">
// Copyright (c) 2013-2015 Andrey Veselov. All rights reserved.
// License: Apache License 2.0
// Contacts: http://andrey.moveax.com andrey@moveax.com
// </copyright>
// -----------------------------------------------------------------------

namespace LoreKeeper
{
    using System.Threading;
    using System.Threading.Tasks;
    using System.Diagnostics.Contracts;
    using LoreKeeper.Contracts;

    /// <summary>Asynchronous command handler for the command of the TCommand type.</summary>
    /// <typeparam name="TCommand">The type of the command.</typeparam>
    [ContractClass(typeof(CommandHandlerAsyncContract<>))]
    public interface ICommandHandlerAsync<in TCommand> : ICommandHandler<TCommand>
        where TCommand : class, ICommand
    {
        /// <summary>Executes the specified command asynchronously.</summary>
        /// <param name="command">The command.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        Task ExecuteAsync(TCommand command, CancellationToken cancellationToken);
    }
}