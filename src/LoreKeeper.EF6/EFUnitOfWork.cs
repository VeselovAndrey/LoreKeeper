// -----------------------------------------------------------------------
// <copyright file="EFUnitOfWork.cs">
// Copyright (c) 2013 Andrey Veselov. All rights reserved.
// License: Apache License 2.0
// Contacts: http://andrey.moveax.com andrey@moveax.com
// </copyright>
// -----------------------------------------------------------------------

namespace LoreKeeper.EF6
{
    using System;
    using System.Data.Entity;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Transactions;
    using LoreKeeper.Core;

    internal class EFUnitOfWork<TDbContext> : UnitOfWorkBase, IUnitOfWork
        where TDbContext : DbContext, new()
    {
        private readonly DbContext _context;
        private readonly Lazy<IDataSource> _dataSource;
        private readonly Lazy<IRepository> _repository;

        private readonly TransactionOptions _transactionOptions;
        private readonly TransactionScope _transactionScope;

        private bool _isDisposed = false;

        protected override IRepository Repository => this._repository.Value;

        protected override IDataSource DataSource => this._dataSource.Value;

        #region Constructors

        public EFUnitOfWork(ICqrsDependencyResolver dependecyResolver)
            : base(dependecyResolver)
        {
            // Disable transaction
            this._transactionScope = null;

            // Create data context
            this._context = new TDbContext();
            this._dataSource = new Lazy<IDataSource>(() => new EFDataSource(this._context));
            this._repository = new Lazy<IRepository>(() => (IRepository)new EFRepository(this._context));
        }

        public EFUnitOfWork(ICqrsDependencyResolver dependecyResolver, bool enableTransactions)
            : base(dependecyResolver)
        {
            // Enable transaction if required
            if (enableTransactions) {
                this._transactionOptions = new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted };
                this._transactionScope = new TransactionScope(TransactionScopeOption.RequiresNew, this._transactionOptions);
            }
            else
                this._transactionScope = null;

            // Create data context
            this._context = new TDbContext();
            this._dataSource = new Lazy<IDataSource>(() => new EFDataSource(this._context));
            this._repository = new Lazy<IRepository>(() => (IRepository)new EFRepository(this._context));
        }

        public EFUnitOfWork(ICqrsDependencyResolver dependecyResolver, TransactionOptions transactionOptions)
            : base(dependecyResolver)
        {
            // Enable transaction
            this._transactionOptions = transactionOptions;
            this._transactionScope = new TransactionScope(TransactionScopeOption.RequiresNew, this._transactionOptions);

            // Create data context
            this._context = new TDbContext();
            this._dataSource = new Lazy<IDataSource>(() => new EFDataSource(this._context));
            this._repository = new Lazy<IRepository>(() => (IRepository)new EFRepository(this._context));
        }

        #endregion

        ~EFUnitOfWork()
        {
            this.Disposing();
        }

        #region IUnitOfWork Members

        public override IUnitOfWork BeginTransaction()
        {
            if (this._transactionScope == null)
                throw new InvalidOperationException("Transaction not initialized.");

            return new EFUnitOfWork<TDbContext>(this._dependencyResolver);
        }

        public override IUnitOfWork BeginTransaction(bool enableInnerTransaction)
        {
            if (this._transactionScope == null)
                throw new InvalidOperationException("Transaction not initialized.");

            return new EFUnitOfWork<TDbContext>(this._dependencyResolver, this._transactionOptions);
        }

        public override IUnitOfWork BeginTransaction(TransactionIsolationLevel isolationLevel)
        {
            if (this._transactionScope == null)
                throw new InvalidOperationException("Transaction not initialized.");

            var transactionOptions = new TransactionOptions() {
                IsolationLevel = (IsolationLevel)isolationLevel
            };

            return new EFUnitOfWork<TDbContext>(this._dependencyResolver, transactionOptions);
        }

        public override IUnitOfWork BeginTransaction(TransactionIsolationLevel isolationLevel, TimeSpan timeout)
        {
            if (this._transactionScope == null)
                throw new InvalidOperationException("Transaction not initialized.");

            var transactionOptions = new TransactionOptions() {
                IsolationLevel = (IsolationLevel)isolationLevel,
                Timeout = timeout
            };

            return new EFUnitOfWork<TDbContext>(this._dependencyResolver, transactionOptions);
        }

        public override void Commit()
        {
            this._context.SaveChanges();

            if (this._repository.IsValueCreated) {
                var efRepositiry = this._repository.Value as IEFRepository;

                efRepositiry?.Commit();
            }

            this._transactionScope?.Complete();

            base.CommitBaseTransactions();
        }

        public override async Task CommitAsync(CancellationToken cancellationToken)
        {
            await this._context.SaveChangesAsync(cancellationToken);

            if (this._repository.IsValueCreated) {
                var efRepositiry = this._repository.Value as IEFRepository;

                efRepositiry?.Commit();
            }

            this._transactionScope?.Complete();

            base.CommitBaseTransactions();
        }

        public override void Rollback()
        {
            if (this._repository.IsValueCreated) {
                var efRepositiry = this._repository.Value as IEFRepository;

                efRepositiry?.Rollback();
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

            this._transactionScope?.Dispose();
            this._context.Dispose();

            this._isDisposed = true;
        }
    }
}
