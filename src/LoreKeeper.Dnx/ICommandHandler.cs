// -----------------------------------------------------------------------
// <copyright file="ICommandHandler.cs">
// Copyright (c) 2013-2015 Andrey Veselov. All rights reserved.
// License: Apache License 2.0
// Contacts: http://andrey.moveax.com andrey@moveax.com
// </copyright>
// -----------------------------------------------------------------------

namespace LoreKeeper
{
    /// <summary>command handler for the command of the TCommand type.</summary>
    /// <typeparam name="TCommand">The type of the command.</typeparam>
    public interface ICommandHandler<in TCommand>
        where TCommand : class, ICommand
    {
    }
}