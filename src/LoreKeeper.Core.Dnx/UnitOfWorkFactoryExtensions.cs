// -----------------------------------------------------------------------
// <copyright file="UnitOfWorkFactoryExtensions.cs">
// Copyright (c) 2013-2015 Andrey Veselov. All rights reserved.
// License: Apache License 2.0
// Contacts: http://andrey.moveax.com andrey@moveax.com
// </copyright>
// -----------------------------------------------------------------------

namespace LoreKeeper.Core
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Threading.Tasks;

    /// <summary>IUnitOfWorkFactory extensions.</summary>
    public static class UnitOfWorkFactoryExtensions
    {
        /// <summary>Executes a single command.</summary>
        /// <typeparam name="TCommand">The type of the command.</typeparam>
        /// <param name="unitOfWorkFactory">The unit of work factory.</param>
        /// <param name="command">The command.</param>
        public static void ExecuteSingleCommand<TCommand>(this IUnitOfWorkFactory unitOfWorkFactory, TCommand command)
            where TCommand : class, ICommand
        {
            Contract.Requires(unitOfWorkFactory != null);
            Contract.Requires(command != null);

            using (var unitOfWork = unitOfWorkFactory.Create()) {
                unitOfWork.ExecuteCommand(command);
                unitOfWork.Commit();
            }
        }

        /// <summary>Executes a single command and commit asynchronously.</summary>
        /// <typeparam name="TCommand">The type of the command.</typeparam>
        /// <param name="unitOfWorkFactory">The unit of work factory.</param>
        /// <param name="command">The command.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static async Task ExecuteSingleCommandAsync<TCommand>(this IUnitOfWorkFactory unitOfWorkFactory, TCommand command)
            where TCommand : class, ICommand
        {
            Contract.Requires(unitOfWorkFactory != null);
            Contract.Requires(command != null);

            using (var unitOfWork = unitOfWorkFactory.Create()) {
                await unitOfWork.ExecuteCommandAsync(command).ConfigureAwait(false);
                await unitOfWork.CommitAsync().ConfigureAwait(false);
            }
        }

        /// <summary>Executes a single query.</summary>
        /// <typeparam name="TQuery">The type of the query.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="unitOfWorkFactory">The unit of work factory.</param>
        /// <param name="queryAction">The query action.</param>
        /// <returns>The result of the query.</returns>
        public static TResult ExecuteSingleQuery<TQuery, TResult>(this IUnitOfWorkFactory unitOfWorkFactory, Func<TQuery, TResult> queryAction)
            where TQuery : class, IQuery
        {
            Contract.Requires(unitOfWorkFactory != null);
            Contract.Requires(queryAction != null);

            TResult result;

            using (var unitOfWork = unitOfWorkFactory.Create()) {
                var query = unitOfWork.ResolveQuery<TQuery>();
                result = queryAction(query);
            }

            return result;
        }
    }
}