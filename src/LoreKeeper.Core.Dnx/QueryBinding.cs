// -----------------------------------------------------------------------
// <copyright file="QueryBinding.cs">
// Copyright (c) 2013-2015 Andrey Veselov. All rights reserved.
// License: Apache License 2.0
// Contacts: http://andrey.moveax.com andrey@moveax.com
// </copyright>
// -----------------------------------------------------------------------

namespace LoreKeeper.Core
{
    using System;
    using System.Diagnostics.Contracts;

    /// <summary>Specifies the connection between query interface and it's implementation.</summary>
    public class QueryBinding
    {
        private readonly Type _interface;
        private Type _implementation = null;

        /// <summary>Gets the type of the query interface.</summary>
        /// <value>The type of the query interface.</value>
        public Type Interface => this._interface;

        /// <summary>Gets or sets the query implementation.</summary>
        /// <value>The type of the implementation.</value>
        /// <exception cref="System.InvalidOperationException">A implementation was already specified.</exception>
        public Type Implementation
        {
            get { return this._implementation; }
            set
            {
                Contract.Requires(value != null);

                if (this._implementation != null)
                    throw new InvalidOperationException(
                        $"Class {value}: Interface {this._interface.FullName} implementation was registered already by {this._implementation.FullName}.");

                this._implementation = value;
            }
        }

        public QueryBinding(Type interfaceType)
        {
            Contract.Requires(interfaceType != null);
            //TODO: Contract.Requires(typeof(IQuery).IsAssignableFrom(interfaceType));

            this._interface = interfaceType;
        }
    }
}