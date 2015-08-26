// -----------------------------------------------------------------------
// <copyright file="IUnitOfWork.cs">
// Copyright (c) 2013-2015 Andrey Veselov. All rights reserved.
// License: Apache License 2.0
// Contacts: http://andrey.moveax.com andrey@moveax.com
// </copyright>
// -----------------------------------------------------------------------

namespace LoreKeeper
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Diagnostics.Contracts;
    using LoreKeeper.Contracts;

    /// <summary>Unit of Work interface.</summary>
    [ContractClass(typeof(UnitOfWorkContract))]
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>Resolves and returns the query.</summary>
        /// <typeparam name="T">The query type.</typeparam>
        /// <returns>The query instance.</returns>
        T ResolveQuery<T>()
            where T : class, IQuery;

        /// <summary>Adds command to execution queue.</summary>
        /// <typeparam name="T">The command type.</typeparam>
        /// <param name="command">The command.</param>
        void ExecuteCommand<T>(T command)
            where T : class, ICommand;

        /// <summary>Adds command to execution queue.</summary>
        /// <typeparam name="T">The command type.</typeparam>
        /// <param name="command">The command.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        Task ExecuteCommandAsync<T>(T command)
            where T : class, ICommand;

        /// <summary>Adds command to execution queue asynchronously.</summary>
        /// <typeparam name="T">The command type.</typeparam>
        /// <param name="command">The command.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        Task ExecuteCommandAsync<T>(T command, CancellationToken cancellationToken)
            where T : class, ICommand;

        /// <summary>Saves all changes made in this UnitOfWork to the underlaying data source.</summary>
        void Commit();

        /// <summary>Asynchronously saves all changes made in this UnitOfWork to the underlaying data source.</summary>
        /// <returns>The task object representing the asynchronous operation.</returns>
        Task CommitAsync();

        /// <summary>Asynchronously saves all changes made in this UnitOfWork to the underlaying data source.</summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        Task CommitAsync(CancellationToken cancellationToken);

        /// <summary>Dismiss all changes.</summary>
        void Rollback();

        /// <summary>Begins the inner transaction.</summary>
        /// <returns>The Unit of Work for inner transaction.</returns>
        IUnitOfWork BeginTransaction();

        /// <summary>Begins the inner transaction.</summary>
        /// <param name="enableInnerTransaction">if set to <c>true</c> then transactions support are enabled.</param>
        /// <returns>The Unit of Work for inner transaction.</returns>
        IUnitOfWork BeginTransaction(bool enableInnerTransaction);

        /// <summary>Begins the inner transaction.</summary>
        /// <param name="isolationLevel">The transaction isolation level.</param>
        /// <returns>The Unit of Work for inner transaction.</returns>
        IUnitOfWork BeginTransaction(TransactionIsolationLevel isolationLevel);

        /// <summary>Begins the inner transaction.</summary>
        /// <param name="isolationLevel">The transaction isolation level.</param>
        /// <param name="timeout">The command execution timeout.</param>
        /// <returns>The Unit of Work for inner transaction.</returns>
        IUnitOfWork BeginTransaction(TransactionIsolationLevel isolationLevel, TimeSpan timeout);
    }
}