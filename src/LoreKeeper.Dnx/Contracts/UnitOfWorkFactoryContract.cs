// -----------------------------------------------------------------------
// <copyright file="UnitOfWorkFactoryContract.cs">
// Copyright (c) 2013-2015 Andrey Veselov. All rights reserved.
// License: Apache License 2.0
// Contacts: http://andrey.moveax.com andrey@moveax.com
// </copyright>
// -----------------------------------------------------------------------

namespace LoreKeeper.Contracts
{
    using System;
    using System.Diagnostics.Contracts;
    
    [ContractClassFor(typeof(IUnitOfWorkFactory))]
    internal abstract class UnitOfWorkFactoryContract : IUnitOfWorkFactory
    {
        public IUnitOfWork Create()
        {
            Contract.Ensures(Contract.Result<IUnitOfWork>() != null);

            return default(IUnitOfWork);
        }

        public IUnitOfWork Create(bool enableTransactions)
        {
            Contract.Ensures(Contract.Result<IUnitOfWork>() != null);

            return default(IUnitOfWork);
        }

        public IUnitOfWork Create(TransactionIsolationLevel isolationLevel)
        {
            Contract.Ensures(Contract.Result<IUnitOfWork>() != null);

            return default(IUnitOfWork);
        }

        public IUnitOfWork Create(TransactionIsolationLevel isolationLevel, TimeSpan timeout)
        {
            Contract.Ensures(Contract.Result<IUnitOfWork>() != null);

            return default(IUnitOfWork);
        }
    }
}