// -----------------------------------------------------------------------
// <copyright file="EFRepository.cs">
// Copyright (c) 2013-2015 Andrey Veselov. All rights reserved.
// License:  Microsoft Public License (MS-PL)
// Contacts: http://andrey.moveax.com  andrey@moveax.com
// </copyright>
// -----------------------------------------------------------------------

namespace LoreKeeper.EF7
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Linq.Expressions;
    using LoreKeeper.EF7.AdvancedFeatures;
    using Microsoft.Data.Entity;
    using Microsoft.Data.Entity.ChangeTracking;
    using Microsoft.Data.Entity.Metadata;

    internal class EFRepository : IEFRepository
    {
        private readonly DbContext _context;
        private readonly AdvancedRequestsContainer _requestsContainer;

        public EFRepository(DbContext context)
        {
            Contract.Requires(context != null);

            this._context = context;
            this._requestsContainer = new AdvancedRequestsContainer(this._context);
        }

        public void Add<TEntity>(TEntity newEntity)
            where TEntity : class
        {
            this._context.Set<TEntity>().Add(newEntity);
        }

        public void Update<TEntity>(TEntity entity)
            where TEntity : class
        {
            this._context.Set<TEntity>().Attach(entity);
            this._context.Entry(entity).State = EntityState.Modified;
        }

        public void Update<TEntity>(TEntity entity, Expression<Func<TEntity, bool>> match, params Expression<Func<TEntity, object>>[] properties)
            where TEntity : class
        {
            this._requestsContainer.Update(entity, match, properties);
        }

        public void UpdateSpecifiedProperties<TEntity>(TEntity entity, params Expression<Func<TEntity, object>>[] properties)
            where TEntity : class
        {
            this._context.Set<TEntity>().Attach(entity);
            var attachedEntry = this._context.Entry(entity);

            foreach (var property in properties)
                attachedEntry.Property(this.GetPropertyName(property)).IsModified = true;
        }

        public void UpdateExcludeProperties<TEntity>(TEntity entity, params Expression<Func<TEntity, object>>[] properties)
            where TEntity : class
        {
            ICollection<string> excludedProperties = properties
                            .Select(this.GetPropertyName)
                            .ToList();

            this._context.Set<TEntity>().Attach(entity);
            EntityEntry<TEntity> attachedEntry = this._context.Entry(entity);

            IEnumerable<string> propertiesList = attachedEntry.Metadata.GetProperties()
                .Where(p => !p.IsPrimaryKey() && !excludedProperties.Any(name => string.Equals(name, p.Name, StringComparison.Ordinal)))
                .Select(p => p.Name)
                .ToArray();

            foreach (var propertyName in propertiesList)
                attachedEntry.Property(propertyName).IsModified = true;
        }

        public void Remove<TEntity>(TEntity entity)
            where TEntity : class
        {
            this._context.Set<TEntity>().Attach(entity);
            this._context.Set<TEntity>().Remove(entity);
        }

        public void Remove<TEntity>(Expression<Func<TEntity, bool>> match)
            where TEntity : class
        {
            this._requestsContainer.Remove(match);
        }

        public void Commit()
        {
            this._requestsContainer.Execute();
        }

        public void Rollback()
        {
            this._requestsContainer.Clear();
        }

        private string GetPropertyName<TEntity, TProperty>(Expression<Func<TEntity, TProperty>> selector)
        {
            switch (selector.Body.NodeType) {
                case ExpressionType.Convert:
                    var body = (UnaryExpression)selector.Body;
                    var memberExp1 = body.Operand as MemberExpression;
                    if (memberExp1 != null)
                        return memberExp1.Member.Name;
                    break;

                case ExpressionType.MemberAccess:
                    var memberExp2 = selector.Body as MemberExpression;
                    if (memberExp2 != null)
                        return memberExp2.Member.Name;
                    break;
            }

            throw new ArgumentException("MemberExpression expected.");
        }
    }
}