// -----------------------------------------------------------------------
// <copyright file="AutofacDependencyResolver.cs">
// Copyright (c) 2013-2015 Andrey Veselov. All rights reserved.
// License: Apache License 2.0
// Contacts: http://andrey.moveax.com andrey@moveax.com
// </copyright>
// -----------------------------------------------------------------------

namespace LoreKeeper.Bootstrappers.Autofac
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using global::Autofac;
    using global::Autofac.Core;

    internal class AutofacDependencyResolver : ICqrsDependencyResolver
    {
        private readonly IContainer _container;

        public AutofacDependencyResolver(IContainer container)
        {
            Contract.Requires(container != null);

            this._container = container;
        }

        public T Resolve<T>()
            where T : class
        {
            return this._container.Resolve<T>();
        }

        public T Resolve<T>(IEnumerable<CqrsParameterOverride> ctorParams)
            where T : class
        {
            var typedParams = ctorParams
                .Select(p => (Parameter)new TypedParameter(p.ValueType, p.Value))
                .ToArray();

            return this._container.Resolve<T>(typedParams);
        }

        public IEnumerable<T> ResolveAll<T>()
            where T : class
        {
            return this._container.Resolve<IEnumerable<T>>();
        }

        public IEnumerable<T> ResolveAll<T>(IEnumerable<CqrsParameterOverride> ctorParams)
            where T : class
        {
            var typedParams = ctorParams
                .Select(p => (Parameter)new TypedParameter(p.ValueType, p.Value))
                .ToArray();

            return this._container.Resolve<IEnumerable<T>>(typedParams);
        }
    }
}