// -----------------------------------------------------------------------
// <copyright file="ICqrsDependencyResolver.cs">
// Copyright (c) 2013-2015 Andrey Veselov. All rights reserved.
// License: Apache License 2.0
// Contacts: http://andrey.moveax.com andrey@moveax.com
// </copyright>
// -----------------------------------------------------------------------

namespace LoreKeeper
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using LoreKeeper.Contracts;

    /// <summary>Dependency resolver interface for LoreKeeper.</summary>
    [ContractClass(typeof(CqrsDependencyResolverContract))]
    public interface ICqrsDependencyResolver
    {
        /// <summary>Resolves singly registered query or command handler.</summary>
        /// <typeparam name="T">The type of the requested query or command handler.</typeparam>
        /// <returns>The requested query or command handler.</returns>
        T Resolve<T>()
            where T : class;

        /// <summary>Resolves singly registered query or command handler.</summary>
        /// <typeparam name="T">The type of the requested query or command handler.</typeparam>
        /// <param name="ctorParams">The constructor parameters.</param>
        /// <returns>The requested query or command handler.</returns>
        T Resolve<T>(IEnumerable<CqrsParameterOverride> ctorParams)
            where T : class;

        /// <summary>Resolves multiply registered queries or command handlers.</summary>
        /// <typeparam name="T">The type of the requested queries or command handlers.</typeparam>
        /// <returns>The requested queries or command handlers.</returns>
        IEnumerable<T> ResolveAll<T>()
            where T : class;

        /// <summary>Resolves multiply registered queries or command handlers.</summary>
        /// <typeparam name="T">The type of the requested queries or command handlers.</typeparam>
        /// <param name="ctorParams">The constructor parameters.</param>
        /// <returns>The requested queries or command handlers.</returns>
        IEnumerable<T> ResolveAll<T>(IEnumerable<CqrsParameterOverride> ctorParams)
            where T : class;
    }
}