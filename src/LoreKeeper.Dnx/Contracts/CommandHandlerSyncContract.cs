// -----------------------------------------------------------------------
// <copyright file="CommandHandlerSyncContract.cs">
// Copyright (c) 2013-2015 Andrey Veselov. All rights reserved.
// License: Apache License 2.0
// Contacts: http://andrey.moveax.com andrey@moveax.com
// </copyright>
// -----------------------------------------------------------------------

namespace LoreKeeper.Contracts
{
    using System.Diagnostics.Contracts;

    [ContractClassFor(typeof(ICommandHandlerSync<>))]
    internal abstract class CommandHandlerSyncContract<TCommand> : ICommandHandlerSync<TCommand>
        where TCommand : class, ICommand
    {
        public void Execute(TCommand command)
        {
            Contract.Requires(command != null);
        }
    }
}