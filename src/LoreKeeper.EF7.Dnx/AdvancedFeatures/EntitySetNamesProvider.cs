// -----------------------------------------------------------------------
// <copyright file="EntitySetNamesProvider.cs">
// Copyright (c) 2012-2015 Andrey Veselov. All rights reserved.
// License:  Microsoft Public License (MS-PL)
// Contacts: http://andrey.moveax.com  andrey@moveax.com
// </copyright>
// -----------------------------------------------------------------------

namespace LoreKeeper.EF7.AdvancedFeatures
{
    using System;
    using System.Collections.Concurrent;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using Microsoft.Data.Entity;

    internal class EntitySetNamesProvider
    {
        private class EntitySetNamesCache : ConcurrentDictionary<Type, string>
        {
        }

        private readonly ConcurrentDictionary<Type, EntitySetNamesCache> _cache
            = new ConcurrentDictionary<Type, EntitySetNamesCache>();

        public string GetEntitySetName(DbContext context, Type entityType)
        {
            Contract.Requires(context != null);
            Contract.Requires(entityType != null);

            var contextType = context.GetType();

            // get cache for this entity set
            EntitySetNamesCache entitySetCache = this._cache.GetOrAdd(contextType, key => new EntitySetNamesCache());

            // get entity set name
            string entitySetName = entitySetCache.GetOrAdd(entityType, key => context.Model.FindEntityType(entityType).SqlServer().TableName);

            return entitySetName;
        }
    }
}