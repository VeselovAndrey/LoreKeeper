// -----------------------------------------------------------------------
// <copyright file="UnitOfWorkBase.cs">
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
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>The implementation of common IUnitOfWork methods.</summary>
    public abstract class UnitOfWorkBase : IUnitOfWork
    {
        private readonly Queue<ICommandHandlerTransactionSupport> _transactionCommands;
        private readonly Lazy<CqrsParameterOverride[]> _paramsCache;
        protected readonly ICqrsDependencyResolver _dependencyResolver;

        protected abstract IRepository Repository { get; }

        protected abstract IDataSource DataSource { get; }

        protected UnitOfWorkBase(ICqrsDependencyResolver dependecyResolver)
        {
            Contract.Requires(dependecyResolver != null);

            this._dependencyResolver = dependecyResolver;
            this._transactionCommands = new Queue<ICommandHandlerTransactionSupport>();

            this._paramsCache = new Lazy<CqrsParameterOverride[]>(() => new[] {
                    new CqrsParameterOverride("repository", typeof(IRepository), this.Repository),
                    new CqrsParameterOverride("dataSource", typeof(IDataSource), this.DataSource)
                });
        }

        public T ResolveQuery<T>()
            where T : class, IQuery
        {
            return this._dependencyResolver.Resolve<T>(this._paramsCache.Value);
        }

        public void ExecuteCommand<T>(T command)
            where T : class, ICommand
        {
            IEnumerable<ICommandHandler<T>> commandHandlers = this._dependencyResolver
                .ResolveAll<ICommandHandler<T>>(this._paramsCache.Value);
            CancellationToken emptyCancellationToken = CancellationToken.None;

            foreach (var commandHandler in commandHandlers) {
                var syncHandler = commandHandler as ICommandHandlerSync<T>;
                if (syncHandler != null)
                    syncHandler.Execute(command);
                else {
                    var asyncHandler = commandHandler as ICommandHandlerAsync<T>;
                    if (asyncHandler != null)
                        asyncHandler.ExecuteAsync(command, emptyCancellationToken).Wait();
                    else
                        throw new InvalidOperationException(
                            $"Command handler {commandHandler.GetType().FullName} should implement ICommandHandlerSync or ICommandHandlerAsync.");
                }

                var transactionCommad = commandHandler as ICommandHandlerTransactionSupport;
                if (transactionCommad != null)
                    this._transactionCommands.Enqueue(transactionCommad);
            }
        }

        public Task ExecuteCommandAsync<T>(T command)
            where T : class, ICommand
        {
            return this.ExecuteCommandAsync(command, CancellationToken.None);
        }

        public async Task ExecuteCommandAsync<T>(T command, CancellationToken cancellationToken)
            where T : class, ICommand
        {
            IEnumerable<ICommandHandler<T>> commandHandlers = this._dependencyResolver
                .ResolveAll<ICommandHandler<T>>(this._paramsCache.Value);

            foreach (var commandHandler in commandHandlers) {
                var syncHandler = commandHandler as ICommandHandlerSync<T>;
                if (syncHandler != null)
                    syncHandler.Execute(command);
                else {
                    var asyncHandler = commandHandler as ICommandHandlerAsync<T>;
                    if (asyncHandler != null)
                        await asyncHandler.ExecuteAsync(command, cancellationToken).ConfigureAwait(false);
                    else
                        throw new InvalidOperationException(
                            $"Command handler {commandHandler.GetType().FullName} should implement ICommandHandlerSync or ICommandHandlerAsync.");
                }

                var transactionCommad = commandHandler as ICommandHandlerTransactionSupport;
                if (transactionCommad != null)
                    this._transactionCommands.Enqueue(transactionCommad);
            }
        }

        public abstract void Commit();

        public virtual Task CommitAsync()
        {
            return this.CommitAsync(CancellationToken.None);
        }

        public abstract Task CommitAsync(CancellationToken cancellationToken);

        public abstract void Rollback();

        public abstract IUnitOfWork BeginTransaction();

        public abstract IUnitOfWork BeginTransaction(bool enableInnerTransaction);

        public abstract IUnitOfWork BeginTransaction(TransactionIsolationLevel isolationLevel);

        public abstract IUnitOfWork BeginTransaction(TransactionIsolationLevel isolationLevel, TimeSpan timeout);

#if !DNXCORE50
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
#endif
        public abstract void Dispose();

        protected void CommitBaseTransactions()
        {
            foreach (var command in this._transactionCommands)
                command.OnCommitted();
        }
    }
}
