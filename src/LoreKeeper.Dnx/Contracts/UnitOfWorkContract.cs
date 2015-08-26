// -----------------------------------------------------------------------
// <copyright file="UnitOfWorkContract.cs">
// Copyright (c) 2013-2015 Andrey Veselov. All rights reserved.
// License: Apache License 2.0
// Contacts: http://andrey.moveax.com andrey@moveax.com
// </copyright>
// -----------------------------------------------------------------------

namespace LoreKeeper.Contracts
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Threading;
    using System.Threading.Tasks;
    
    [ContractClassFor(typeof(IUnitOfWork))]
    internal abstract class UnitOfWorkContract : IUnitOfWork
    {
        public T ResolveQuery<T>()
            where T : class, IQuery
        {
            Contract.Ensures(Contract.Result<T>() != null);

            return default(T);
        }

        public void ExecuteCommand<T>(T command)
            where T : class, ICommand
        {
            Contract.Requires(command != null);
        }

        public Task ExecuteCommandAsync<T>(T command)
            where T : class, ICommand
        {
            Contract.Requires(command != null);
            Contract.Ensures(Contract.Result<Task>() != null);

            return default(Task);
        }

        public Task ExecuteCommandAsync<T>(T command, CancellationToken cancellationToken)
            where T : class, ICommand
        {
            Contract.Requires(command != null);
            Contract.Ensures(Contract.Result<Task>() != null);

            return default(Task);
        }

        public abstract void Commit();

        public Task CommitAsync()
        {
            Contract.Ensures(Contract.Result<Task>() != null);

            return default(Task);
        }

        public Task CommitAsync(CancellationToken cancellationToken)
        {
            Contract.Ensures(Contract.Result<Task>() != null);

            return default(Task);
        }

        public abstract void Rollback();

        public IUnitOfWork BeginTransaction()
        {
            Contract.Ensures(Contract.Result<IUnitOfWork>() != null);

            return default(IUnitOfWork);
        }

        public IUnitOfWork BeginTransaction(bool enableInnerTransaction)
        {
            Contract.Ensures(Contract.Result<IUnitOfWork>() != null);

            return default(IUnitOfWork);
        }

        public IUnitOfWork BeginTransaction(TransactionIsolationLevel isolationLevel)
        {
            Contract.Ensures(Contract.Result<IUnitOfWork>() != null);

            return default(IUnitOfWork);
        }

        public IUnitOfWork BeginTransaction(TransactionIsolationLevel isolationLevel, TimeSpan timeout)
        {
            Contract.Ensures(Contract.Result<IUnitOfWork>() != null);

            return default(IUnitOfWork);
        }

        public abstract void Dispose();
    }
}