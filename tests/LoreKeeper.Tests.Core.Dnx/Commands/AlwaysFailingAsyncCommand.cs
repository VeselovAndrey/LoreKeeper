// -----------------------------------------------------------------------
// <copyright file="AlwaysFailingAsyncCommand.cs">
// Copyright (c) 2013-2015 Andrey Veselov. All rights reserved.
// License: Apache License 2.0
// Contacts: http://andrey.moveax.com andrey@moveax.com
// </copyright>
// -----------------------------------------------------------------------

namespace LoreKeeper.Tests.Core.Commands
{
    /// <summary>Handler for this command should be asynchronous and always throws an exception.</summary>
    public class AlwaysFailingAsyncCommand : ICommand
    {
    }
}
