// -----------------------------------------------------------------------
// <copyright file="DisableOldUsersCommand.cs">
// Copyright (c) 2013-2015 Andrey Veselov. All rights reserved.
// License: Apache License 2.0
// Contacts: http://andrey.moveax.com andrey@moveax.com
// </copyright>
// -----------------------------------------------------------------------

namespace LoreKeeper.Tests.Core.Commands.Users
{
    using System;

    public class DisableOldUsersCommand : ICommand
    {
        public DateTime MaxDate { get; }

        public DisableOldUsersCommand(DateTime maxDate)
        {
            this.MaxDate = maxDate;
        }
    }
}