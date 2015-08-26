// -----------------------------------------------------------------------
// <copyright file="CqrsDependencyResolverBuilder.cs">
// Copyright (c) 2013-2015 Andrey Veselov. All rights reserved.
// License: Apache License 2.0
// Contacts: http://andrey.moveax.com andrey@moveax.com
// </copyright>
// -----------------------------------------------------------------------

namespace LoreKeeper.Bootstrappers.Autofac
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Reflection;
    using global::Autofac;
    using LoreKeeper.Core;

    /// <summary>Builds the CQRS dependency resolver.</summary>
    public class CqrsDependencyResolverBuilder
    {
        private readonly CqrsBindings _bindings = new CqrsBindings();

        /// <summary>Collects all query interfaces from specified assembly.</summary>
        /// <param name="assembly">The assembly.</param>
        public void CollectQueryInterfaces(Assembly assembly)
        {
            Contract.Requires(assembly != null);

            this._bindings.CollectQueryInterfaces(assembly);
        }

        /// <summary>Collects all query implementations from specified assembly and bind them to interfaces.</summary>
        /// <param name="assembly">The assembly.</param>
        public void BindQueries(Assembly assembly)
        {
            Contract.Requires(assembly != null);

            this._bindings.BindQueries(assembly);
        }

        /// <summary>Collects all commands from specified assembly.</summary>
        /// <param name="assembly">The assembly.</param>
        public void CollectCommands(Assembly assembly)
        {
            Contract.Requires(assembly != null);

            this._bindings.CollectCommands(assembly);
        }

        /// <summary>Collects all command handlers from specified assembly and bind them to commands.</summary>
        /// <param name="assembly">The assembly.</param>
        public void BindCommandHandlers(Assembly assembly)
        {
            Contract.Requires(assembly != null);

            this._bindings.BindCommandHandlers(assembly);
        }

        /// <summary>Builds the CQRS dependency resolver.</summary>
        /// <returns>The CQRS dependency resolver instance.</returns>
        /// <exception cref="System.InvalidOperationException">Invalid bindings was found.</exception>
        public ICqrsDependencyResolver Build()
        {
            if (!this._bindings.Validate())
                throw new InvalidOperationException("Invalid bindings was found.");

            var builder = new ContainerBuilder();

            foreach (var binding in this._bindings.QueryBindings)
                builder.RegisterType(binding.Implementation).As(binding.Interface);

            var interfaceType = typeof(ICommandHandler<>);

            foreach (var binding in this._bindings.CommandBindings) {
                var cmdType = interfaceType.MakeGenericType(binding.Command);

                foreach (var handler in binding.Handlers)
                    builder.RegisterType(handler).As(cmdType);
            }

            return new AutofacDependencyResolver(builder.Build());
        }
    }
}