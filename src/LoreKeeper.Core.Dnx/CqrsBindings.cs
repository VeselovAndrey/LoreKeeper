// -----------------------------------------------------------------------
// <copyright file="CqrsBindings.cs">
// Copyright (c) 2013-2015 Andrey Veselov. All rights reserved.
// License: Apache License 2.0
// Contacts: http://andrey.moveax.com andrey@moveax.com
// </copyright>
// -----------------------------------------------------------------------

namespace LoreKeeper.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Diagnostics.Contracts;

    /// <summary>Container for commands and query bindings.</summary>
    public class CqrsBindings
    {
        private readonly Type _iQueryType = typeof(IQuery);
        private readonly Type _iCommandType = typeof(ICommand);
        private readonly Type _iCommandHandlerType = typeof(ICommandHandler<>);

        private readonly ICollection<QueryBinding> _queryBindings = new List<QueryBinding>();
        private readonly ICollection<CommandBinding> _commandBindings = new List<CommandBinding>();

        /// <summary>Gets all collected query bindings.</summary>
        public IEnumerable<QueryBinding> QueryBindings => this._queryBindings;

        /// <summary>Gets all collected command bindings.</summary>
        public IEnumerable<CommandBinding> CommandBindings => this._commandBindings;

        /// <summary>Collects all query interfaces from specified assembly.</summary>
        /// <param name="assembly">The assembly.</param>
        public void CollectQueryInterfaces(Assembly assembly)
        {
            Contract.Requires(assembly != null);

            var queryInterfaces = assembly.GetTypes()
                .Where(t => this.IsInterface(t) && this._iQueryType.IsAssignableFrom(t) && t != this._iQueryType);

            foreach (var queryInterface in queryInterfaces)
                this._queryBindings.Add(new QueryBinding(queryInterface));
        }

        /// <summary>Collects all query implementations from specified assembly and bind them to interfaces.</summary>
        /// <param name="assembly">The assembly.</param>
        public void BindQueries(Assembly assembly)
        {
            Contract.Requires(assembly != null);

            Dictionary<Type, Type> queries = assembly.GetTypes()
                .Where(t => this.IsNotInterfaceAndNotAbstract(t) && this._iQueryType.IsAssignableFrom(t))
                .ToDictionary(t => t.GetInterfaces().First(i => i != this._iQueryType && this._iQueryType.IsAssignableFrom(i)), t => t);

            foreach (var query in queries) {
                var binding = this._queryBindings.FirstOrDefault(b => b.Interface == query.Key);

                if (binding == null)
                    throw new InvalidOperationException($"Query {query.Value.FullName}: Interface {query.Key.FullName} was not defined.");

                binding.Implementation = query.Value;
            }
        }

        /// <summary>Collects all commands from specified assembly.</summary>
        /// <param name="assembly">The assembly.</param>
        public void CollectCommands(Assembly assembly)
        {
            Contract.Requires(assembly != null);

            IEnumerable<Type> commands = assembly.GetTypes()
                .Where(t => this.IsNotInterfaceAndNotAbstract(t) && this._iCommandType.IsAssignableFrom(t));

            foreach (var command in commands)
                this._commandBindings.Add(new CommandBinding(command));
        }

        /// <summary>Collects all command handlers from specified assembly and bind them to commands.</summary>
        /// <param name="assembly">The assembly.</param>
        public void BindCommandHandlers(Assembly assembly)
        {
            Contract.Requires(assembly != null);

            var allTypes = assembly.GetTypes();

            foreach (var commandBinding in this._commandBindings) {
                var genericCommandHandler = this._iCommandHandlerType.MakeGenericType(commandBinding.Command);

                var handlers = allTypes
                    .Where(t => this.IsNotInterfaceAndNotAbstract(t) && genericCommandHandler.IsAssignableFrom(t));

                if (handlers.Any())
                    commandBinding.AddHandlers(handlers);
            }
        }

        /// <summary>Validate all bindings.</summary>
        /// <returns>Returns: true if validation succeeds, false otherwise.</returns>
        public bool Validate()
        {
            return this._queryBindings.All(p => p.Implementation != null) && this._commandBindings.All(p => p.Handlers != null);
        }

#if DNXCORE50
        private bool IsInterface(Type t)
        {
            return t.GetTypeInfo().IsInterface;
        }

        private bool IsNotInterfaceAndNotAbstract(Type t)
        {
            var tInfo = t.GetTypeInfo();
            return !tInfo.IsInterface && !tInfo.IsAbstract;
        }
#else
        private bool IsInterface(Type t)
        {
            return t.IsInterface;
        }

        private bool IsNotInterfaceAndNotAbstract(Type t)
        {
            return !t.IsInterface && !t.IsAbstract;
        }
#endif
    }
}