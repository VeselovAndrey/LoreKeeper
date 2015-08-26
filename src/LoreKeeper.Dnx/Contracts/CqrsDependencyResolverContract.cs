// -----------------------------------------------------------------------
// <copyright file="CqrsDependencyResolverContract.cs">
// Copyright (c) 2013-2015 Andrey Veselov. All rights reserved.
// License: Apache License 2.0
// Contacts: http://andrey.moveax.com andrey@moveax.com
// </copyright>
// -----------------------------------------------------------------------

namespace LoreKeeper.Contracts
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;

    [ContractClassFor(typeof(ICqrsDependencyResolver))]
    internal abstract class CqrsDependencyResolverContract : ICqrsDependencyResolver
    {
        public T Resolve<T>()
            where T : class
        {
            Contract.Ensures(Contract.Result<T>() != null);

            return default(T);
        }

        public T Resolve<T>(IEnumerable<CqrsParameterOverride> ctorParams)
            where T : class
        {
            Contract.Requires(ctorParams != null && ctorParams.Any());
            Contract.Ensures(Contract.Result<T>() != null);

            return default(T);
        }

        public IEnumerable<T> ResolveAll<T>()
            where T : class
        {
            Contract.Ensures(Contract.Result<IEnumerable<T>>() != null);
            Contract.Ensures(Contract.Result<IEnumerable<T>>().Any());

            return default(IEnumerable<T>);
        }

        public IEnumerable<T> ResolveAll<T>(IEnumerable<CqrsParameterOverride> ctorParams)
            where T : class
        {
            Contract.Requires(ctorParams != null && ctorParams.Any());
            Contract.Ensures(Contract.Result<IEnumerable<T>>() != null);
            Contract.Ensures(Contract.Result<IEnumerable<T>>().Any());

            return default(IEnumerable<T>);
        }
    }
}