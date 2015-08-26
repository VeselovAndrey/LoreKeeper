// -----------------------------------------------------------------------
// <copyright file="UpdateLinkTitleCommand.cs">
// Copyright (c) 2013-2015 Andrey Veselov. All rights reserved.
// License: Apache License 2.0
// Contacts: http://andrey.moveax.com andrey@moveax.com
// </copyright>
// -----------------------------------------------------------------------

namespace LoreKeeper.Tests.Core.Commands.Links
{
    public class UpdateLinkTitleCommand : ICommand
    {
        public int Id { get;  }

        public string Title { get;  }

        public UpdateLinkTitleCommand(int linkId, string title)
        {
            this.Id = linkId;
            this.Title = title;
        }
    }
}