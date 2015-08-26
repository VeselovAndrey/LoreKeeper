// -----------------------------------------------------------------------
// <copyright file="ICommandHandler.cs">
// Copyright (c) 2013-2015 Andrey Veselov. All rights reserved.
// License: Apache License 2.0
// Contacts: http://andrey.moveax.com andrey@moveax.com
// </copyright>
// -----------------------------------------------------------------------

namespace LoreKeeper
{
    using System.Diagnostics.Contracts;
    using LoreKeeper.Contracts;

    /// <summary>Command handler for the command of the TCommand type.</summary>
    /// <typeparam name="TCommand">The type of the command.</typeparam>
    [ContractClass(typeof(CommandHandlerSyncContract<>))]
    public interface ICommandHandlerSync<in TCommand> : ICommandHandler<TCommand>
        where TCommand : class, ICommand
    {
        /// <summary>Executes the specified command.</summary>
        /// <param name="command">The command.</param>
        void Execute(TCommand command);
    }
}