// -----------------------------------------------------------------------
// <copyright file="EFUnitOfWorkFactory.cs">
// Copyright (c) 2013-2015 Andrey Veselov. All rights reserved.
// License:  Microsoft Public License (MS-PL)
// Contacts: http://andrey.moveax.com  andrey@moveax.com
// </copyright>
// -----------------------------------------------------------------------

namespace LoreKeeper.EF7
{
    using System;
    using System.Diagnostics.Contracts;
    using LoreKeeper;
    using Microsoft.Data.Entity;

    /// <summary>IUnitOfWork factory implementation that supports EntityFramework. </summary>
    /// <typeparam name="TDbContext">The type of the database context.</typeparam>
    public class EFUnitOfWorkFactory<TDbContext> : IUnitOfWorkFactory
        where TDbContext : DbContext, new()
    {
        private readonly ICqrsDependencyResolver _dependencyResolver;

        /// <summary>Initializes a new instance of the <see cref="EFUnitOfWorkFactory{TDbContext}"/> class.</summary>
        /// <param name="dependencyResolver">The CQRS dependency resolver.</param>
        public EFUnitOfWorkFactory(ICqrsDependencyResolver dependencyResolver)
        {
            Contract.Requires(dependencyResolver != null);

            this._dependencyResolver = dependencyResolver;
        }

        /// <summary>Creates a new UnitOfWork instance with default parameters.</summary>
        /// <returns>The UnitOfWork instance.</returns>
        public IUnitOfWork Create()
        {
            return this.Create(enableTransactions: false);
        }

        /// <summary>Creates a new UnitOfWork instance with enabled or disabled transactions.</summary>
        /// <param name="enableTransactions">if set to <c>true</c> then transactions support are enabled.</param>
        /// <returns>The UnitOfWork instance.</returns>
        public IUnitOfWork Create(bool enableTransactions)
        {
            return new EFUnitOfWork<TDbContext>(this._dependencyResolver, enableTransactions);
        }

        /// <summary>Creates a new UnitOfWork instance with enabled transaction, specified isolation level and transactions support.</summary>
        /// <param name="isolationLevel">The isolation level.</param>
        /// <returns>The UnitOfWork instance.</returns>
        public IUnitOfWork Create(TransactionIsolationLevel isolationLevel)
        {
            return new EFUnitOfWork<TDbContext>(this._dependencyResolver, isolationLevel);
        }

        /// <summary>Creates a new UnitOfWork instance with enabled transaction, specified isolation level, connection timeout and transactions support.</summary>
        /// <param name="isolationLevel">The isolation level.</param>
        /// <param name="timeout">The timeout.</param>
        /// <returns>The UnitOfWork instance.</returns>
        public IUnitOfWork Create(TransactionIsolationLevel isolationLevel, TimeSpan timeout)
        {
            return new EFUnitOfWork<TDbContext>(this._dependencyResolver, isolationLevel);
        }
    }
}