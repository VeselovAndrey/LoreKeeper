// -----------------------------------------------------------------------
// <copyright file="IUnitOfWorkFactory.cs">
// Copyright (c) 2013-2015 Andrey Veselov. All rights reserved.
// License: Apache License 2.0
// Contacts: http://andrey.moveax.com andrey@moveax.com
// </copyright>
// -----------------------------------------------------------------------

namespace LoreKeeper
{
    using System;
    using System.Diagnostics.Contracts;
    using LoreKeeper.Contracts;

    /// <summary>A factory for concrete IUnitOfWork implementations.</summary>
    [ContractClass(typeof(UnitOfWorkFactoryContract))]
    public interface IUnitOfWorkFactory
    {
        /// <summary>Creates a new UnitOfWork instance with default parameters.</summary>
        /// <returns>The UnitOfWork instance.</returns>
        IUnitOfWork Create();

        /// <summary>Creates a new UnitOfWork instance with enabled or disabled transactions.</summary>
        /// <param name="enableTransactions">if set to <c>true</c> then transactions support are enabled.</param>
        /// <returns>The UnitOfWork instance.</returns>
        IUnitOfWork Create(bool enableTransactions);

        /// <summary>Creates a new UnitOfWork instance with enabled transaction, specified isolation level and transactions support.</summary>
        /// <param name="isolationLevel">The isolation level.</param>
        /// <returns>The UnitOfWork instance.</returns>
        IUnitOfWork Create(TransactionIsolationLevel isolationLevel);

        /// <summary>Creates a new UnitOfWork instance with enabled transaction, specified isolation level, connection timeout and transactions support.</summary>
        /// <param name="isolationLevel">The isolation level.</param>
        /// <param name="timeout">The timeout.</param>
        /// <returns>The UnitOfWork instance.</returns>
        IUnitOfWork Create(TransactionIsolationLevel isolationLevel, TimeSpan timeout);
    }
}