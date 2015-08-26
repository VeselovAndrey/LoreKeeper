// -----------------------------------------------------------------------
// <copyright file="TransactionIsolationLevel.cs">
// Copyright (c) 2013-2015 Andrey Veselov. All rights reserved.
// License: Apache License 2.0
// Contacts: http://andrey.moveax.com andrey@moveax.com
// </copyright>
// -----------------------------------------------------------------------

namespace LoreKeeper
{
    /// <summary>Specifies the isolation level of a transaction.</summary>
    public enum TransactionIsolationLevel
    {
        /// <summary>Volatile data can be read but not modified, and no new data can be added during the transaction.</summary>
        Serializable = 0,

        /// <summary>Volatile data can be read but not modified during the transaction. New data can be added during the transaction.</summary>
        RepeatableRead = 1,

        /// <summary>Volatile data cannot be read during the transaction, but can be modified.</summary>
        ReadCommitted = 2,

        /// <summary> Volatile data can be read and modified during the transaction.</summary>
        ReadUncommitted = 3,

        /// <summary>Volatile data can be read. Before a transaction modifies data, it verifies
        /// if another transaction has changed the data after it was initially read.
        /// If the data has been updated, an error is raised. This allows a transaction
        /// to get to the previously committed value of the data.</summary>
        Snapshot = 4,

        /// <summary>The pending changes from more highly isolated transactions cannot be overwritten.</summary>
        Chaos = 5,

        /// <summary>A different isolation level than the one specified is being used, but the 
        /// level cannot be determined. An exception is thrown if this value is set.</summary>
        Unspecified = 6
    }
}