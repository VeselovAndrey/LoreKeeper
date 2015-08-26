// -----------------------------------------------------------------------
// <copyright file="ICommandHandlerTransactionSupport.cs">
// Copyright (c) 2013-2015 Andrey Veselov. All rights reserved.
// License: Apache License 2.0
// Contacts: http://andrey.moveax.com andrey@moveax.com
// </copyright>
// -----------------------------------------------------------------------

namespace LoreKeeper
{
    /// <summary>Support for commands post-committed action.</summary>
    public interface ICommandHandlerTransactionSupport
    {
        /// <summary>This method is called after the transaction Commit() method.</summary>
        void OnCommitted();
    }
}