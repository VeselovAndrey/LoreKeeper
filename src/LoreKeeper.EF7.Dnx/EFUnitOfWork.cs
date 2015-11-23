// -----------------------------------------------------------------------
// <copyright file="EFUnitOfWork.cs">
// Copyright (c) 2013 Andrey Veselov. All rights reserved.
// License:  Microsoft Public License (MS-PL)
// Contacts: http://andrey.moveax.com  andrey@moveax.com
// </copyright>
// -----------------------------------------------------------------------

namespace LoreKeeper.EF7
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Threading;
    using System.Threading.Tasks;
    using LoreKeeper.Core;
    using Microsoft.Data.Entity;
    using Microsoft.Data.Entity.Storage;

    internal class EFUnitOfWork<TDbContext> : UnitOfWorkBase, IUnitOfWork
        where TDbContext : DbContext, new()
    {
        private readonly DbContext _context;
        private readonly Lazy<IDataSource> _dataSource;
        private readonly Lazy<IRepository> _repository;

        private readonly bool _transactionsEnabled = false;
        private readonly IRelationalTransaction _transaction = null;
        private readonly TransactionIsolationLevel _isolationLevel = TransactionIsolationLevel.ReadUncommitted;

        private bool _isDisposed = false;

        protected override IRepository Repository => this._repository.Value;

        protected override IDataSource DataSource => this._dataSource.Value;

        #region Constructors

        public EFUnitOfWork(ICqrsDependencyResolver dependecyResolver)
            : this(dependecyResolver, enableTransactions: false)
        {
        }

        public EFUnitOfWork(ICqrsDependencyResolver dependecyResolver, bool enableTransactions)
            : base(dependecyResolver)
        {
            // Create data context
            this._context = new TDbContext();
            this._dataSource = new Lazy<IDataSource>(() => new EFDataSource(this._context));
            this._repository = new Lazy<IRepository>(() => (IRepository)new EFRepository(this._context));

            // Enable transaction if required
            if (enableTransactions) {
                this._transactionsEnabled = true;
                this._isolationLevel = TransactionIsolationLevel.ReadUncommitted;
                this._transaction = this._context.Database.BeginTransaction(this.ToEFIsolationLevel(this._isolationLevel));
            }
            else {
                this._transactionsEnabled = false;
            }
        }

        public EFUnitOfWork(ICqrsDependencyResolver dependecyResolver, TransactionIsolationLevel transactionIsolationLevel)
            : base(dependecyResolver)
        {
            // Create data context
            this._context = new TDbContext();
            this._dataSource = new Lazy<IDataSource>(() => new EFDataSource(this._context));
            this._repository = new Lazy<IRepository>(() => (IRepository)new EFRepository(this._context));

            // Enable transaction
            this._transactionsEnabled = true;
            this._isolationLevel = transactionIsolationLevel;
            this._transaction = this._context.Database.BeginTransaction(this.ToEFIsolationLevel(this._isolationLevel));
        }

        #endregion

        ~EFUnitOfWork()
        {
            this.Disposing();
        }

        #region IUnitOfWork Members

        public override IUnitOfWork BeginTransaction()
        {
            if (!this._transactionsEnabled)
                throw new InvalidOperationException("Transaction not initialized.");

            return new EFUnitOfWork<TDbContext>(this._dependencyResolver);
        }

        public override IUnitOfWork BeginTransaction(bool enableInnerTransaction)
        {
            if (!this._transactionsEnabled)
                throw new InvalidOperationException("Transaction not initialized.");

            return new EFUnitOfWork<TDbContext>(this._dependencyResolver, this._isolationLevel);
        }

        public override IUnitOfWork BeginTransaction(TransactionIsolationLevel isolationLevel)
        {
            if (!this._transactionsEnabled)
                throw new InvalidOperationException("Transaction not initialized.");

            return new EFUnitOfWork<TDbContext>(this._dependencyResolver, isolationLevel);
        }

        public override IUnitOfWork BeginTransaction(TransactionIsolationLevel isolationLevel, TimeSpan timeout)
        {
            if (!this._transactionsEnabled)
                throw new InvalidOperationException("Transaction not initialized.");

            return new EFUnitOfWork<TDbContext>(this._dependencyResolver, isolationLevel);
        }

        public override void Commit()
        {
            this._context.SaveChanges();

            if (this._repository.IsValueCreated) {
                var efRepositiry = this._repository.Value as IEFRepository;

                efRepositiry?.Commit();
            }

            this._transaction?.Commit();

            base.CommitBaseTransactions();
        }

        public override async Task CommitAsync(CancellationToken cancellationToken)
        {
            await this._context.SaveChangesAsync(cancellationToken);

            if (this._repository.IsValueCreated) {
                var efRepositiry = this._repository.Value as IEFRepository;

                efRepositiry?.Commit();
            }

            this._transaction?.Commit();

            base.CommitBaseTransactions();
        }

        public override void Rollback()
        {
            if (this._repository.IsValueCreated) {
                var efRepositiry = this._repository.Value as IEFRepository;

                efRepositiry?.Rollback();
                this._transaction?.Rollback();
            }

            this.Disposing();
        }

        #endregion

        #region IDisposable Members

        public override void Dispose()
        {
            GC.SuppressFinalize(this);

            this.Disposing();
        }

        #endregion IDisposable Members

        private void Disposing()
        {
            if (this._isDisposed)
                return;

            this._transaction?.Dispose();
            this._context.Dispose();

            this._isDisposed = true;
        }

        private IsolationLevel ToEFIsolationLevel(TransactionIsolationLevel level)
        {
            var isolationLevel = new Dictionary<TransactionIsolationLevel, IsolationLevel>() {
                [TransactionIsolationLevel.Serializable] = IsolationLevel.Serializable,
                [TransactionIsolationLevel.RepeatableRead] = IsolationLevel.RepeatableRead,
                [TransactionIsolationLevel.ReadCommitted] = IsolationLevel.ReadCommitted,
                [TransactionIsolationLevel.ReadUncommitted] = IsolationLevel.ReadUncommitted,
                [TransactionIsolationLevel.Snapshot] = IsolationLevel.Snapshot,
                [TransactionIsolationLevel.Chaos] = IsolationLevel.Chaos,
                [TransactionIsolationLevel.Unspecified] = IsolationLevel.Unspecified
            };

            return isolationLevel[level];
        }
    }
}
