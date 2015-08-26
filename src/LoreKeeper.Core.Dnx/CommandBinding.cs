// -----------------------------------------------------------------------
// <copyright file="CommandBinding.cs">
// Copyright (c) 2013-2015 Andrey Veselov. All rights reserved.
// License: Apache License 2.0
// Contacts: http://andrey.moveax.com andrey@moveax.com
// </copyright>
// -----------------------------------------------------------------------

namespace LoreKeeper.Core
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    /// <summary>Specifies the connection between command and it's handlers.</summary>
    public class CommandBinding
    {
        private List<Type> _handlers;

        /// <summary>Gets the type of the command.</summary>
        /// <value>The type of the command.</value>
        public Type Command { get; }

        /// <summary>Gets the binded command handlers.</summary>
        /// <value>List of handler types.</value>
        public IEnumerable<Type> Handlers => this._handlers;

        public CommandBinding(Type commandType)
        {
            this.Command = commandType;
        }

        /// <summary>Adds the handler.</summary>
        /// <param name="handler">The handler.</param>
        public void AddHandler(Type handler)
        {
            Contract.Requires(handler != null);
            //  Contract.Requires(typeof(ICommandHandler<>).IsAssignableFrom(handler));

            if (this._handlers == null)
                this._handlers = new List<Type>();

            this._handlers.Add(handler);
        }

        /// <summary>Adds command handlers.</summary>
        /// <param name="handlers">The handlers.</param>
        public void AddHandlers(IEnumerable<Type> handlers)
        {
            Contract.Requires(handlers != null);
            //  Contract.Requires(handlers.All(t => typeof(ICommandHandler<>).IsAssignableFrom(t)));

            if (this._handlers == null)
                this._handlers = new List<Type>();

            this._handlers.AddRange(handlers);
        }
    }
}