// -----------------------------------------------------------------------
// <copyright file="EntitySetNamesProvider.cs">
// Copyright (c) 2012-2015 Andrey Veselov. All rights reserved.
// License: Apache License 2.0
// Contacts: http://andrey.moveax.com andrey@moveax.com
// </copyright>
// -----------------------------------------------------------------------

namespace LoreKeeper.EF6.AdvancedFeatures
{
    using System;
    using System.Collections.Concurrent;
    using System.Data.Entity;
    using System.Data.Entity.Core.Metadata.Edm;
    using System.Data.Entity.Core.Objects;
    using System.Data.Entity.Infrastructure;
    using System.Diagnostics.Contracts;
    using System.Linq;

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

            Type type = ObjectContext.GetObjectType(entityType);
            string baseTypeName = type.BaseType?.Name;
            string typeName = type.Name;

            // Get cache for this entity set
            EntitySetNamesCache entitySetCache = this._cache.GetOrAdd(contextType, key => new EntitySetNamesCache());

            // Get entity set name
            string entitySetName = entitySetCache.GetOrAdd(entityType, key => {
                ObjectContext objContext = (context as IObjectContextAdapter).ObjectContext;

                var containers = objContext.MetadataWorkspace
                          .GetItemCollection(DataSpace.SSpace)
                          .GetItems<EntityContainer>();

                EntitySetBase entitySet =
                    containers.SelectMany(c => c.BaseEntitySets.Where(e => e.Name == typeName)).FirstOrDefault() ??
                    containers.SelectMany(c => c.BaseEntitySets.Where(e => e.Name == baseTypeName)).FirstOrDefault();

                if (entitySet == null)
                    throw new ArgumentException($"Table for {typeName} was not found");

                return entitySet.Table;
            });

            return entitySetName;
        }
    }
}