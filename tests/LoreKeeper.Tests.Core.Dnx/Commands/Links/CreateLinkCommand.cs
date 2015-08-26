// -----------------------------------------------------------------------
// <copyright file="CreateLinkCommand.cs">
// Copyright (c) 2014-2015 Andrey Veselov. All rights reserved.
// License: Apache License 2.0
// Contacts: http://andrey.moveax.com andrey@moveax.com
// </copyright>
// -----------------------------------------------------------------------

namespace LoreKeeper.Tests.Core.Commands.Links
{
    using System.Diagnostics.Contracts;

    public class CreateLinkCommand : ICommand
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string Title { get; set; }

        public string Url { get; set; }

        public CreateLinkCommand(int userId, string title, string url)
        {
            Contract.Requires(0 < userId);
            Contract.Requires(!string.IsNullOrEmpty(title));
            Contract.Requires(!string.IsNullOrEmpty(url));

            this.UserId = userId;
            this.Title = title.Trim();
            this.Url = url.Trim();
        }
    }
}
