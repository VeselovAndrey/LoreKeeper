// -----------------------------------------------------------------------
// <copyright file="AdvancedRequestsContainer.cs">
// Copyright (c) 2013-2015 Andrey Veselov. All rights reserved.
// License: Apache License 2.0
// Contacts: http://andrey.moveax.com andrey@moveax.com
// </copyright>
// -----------------------------------------------------------------------

namespace LoreKeeper.EF6.AdvancedFeatures
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Linq.Expressions;

    internal class AdvancedRequestsContainer
    {
        private enum RequestType { Update, Delete }

        private class Request
        {
            public RequestType RequestType { get; set; }

            public string TableName { get; set; }

            public string WhereClause { get; set; }

            public string Values { get; set; }
        }

        private const string _updateSqlTemplate = "UPDATE {0} SET {1} WHERE {2}";
        private const string _deleteSqlTemplate = "DELETE FROM {0} WHERE {1}";

        private static readonly EntitySetNamesProvider _entitySetNamesProvider = new EntitySetNamesProvider();

        private readonly ICollection<Request> _requests = new List<Request>();
        private readonly SqlClauseBuilder _sqlClauseBuilder = new SqlClauseBuilder();
        private readonly DbContext _context;

        public AdvancedRequestsContainer(DbContext context)
        {
            Contract.Requires(context != null);

            this._context = context;
        }

        public void Update<TEntity>(TEntity entity, Expression<Func<TEntity, bool>> match, params Expression<Func<TEntity, object>>[] properties)
            where TEntity : class
        {
            Contract.Requires(entity != null);
            Contract.Requires(match != null);
            Contract.Requires(properties != null);
            Contract.Requires(properties.Any());

            var entityType = typeof(TEntity);
            var values = new List<string>();

            foreach (var property in properties) {
                string propertyName = this.GetPropertyName(property);
                object value = entityType
                    .GetProperty(propertyName)
                    .GetValue(entity);

                values.Add(string.Format("{0} = {1}", propertyName, this.ObjectToSqlStringValue(value)));
            }

            var request = new Request() {
                RequestType = RequestType.Update,
                TableName = _entitySetNamesProvider.GetEntitySetName(this._context, typeof(TEntity)),
                WhereClause = this._sqlClauseBuilder.Build(match.Body),
                Values = string.Join(",", values)
            };

            this._requests.Add(request);
        }

        public void Remove<TEntity>(Expression<Func<TEntity, bool>> match)
            where TEntity : class
        {
            Contract.Requires(match != null);

            var request = new Request {
                RequestType = RequestType.Delete,
                TableName = _entitySetNamesProvider.GetEntitySetName(this._context, typeof(TEntity)),
                WhereClause = this._sqlClauseBuilder.Build(match.Body),
                Values = null
            };

            this._requests.Add(request);
        }

        public void Execute()
        {
            foreach (var request in this._requests) {
                var queryString = request.RequestType == RequestType.Update ?
                    string.Format(_updateSqlTemplate, request.TableName, request.Values, request.WhereClause) :
                    string.Format(_deleteSqlTemplate, request.TableName, request.WhereClause);

                this._context.Database.ExecuteSqlCommand(queryString);
            }

            this._requests.Clear();
        }

        public void Clear()
        {
            this._requests.Clear();
        }

        private string GetPropertyName<TEntity, TProperty>(Expression<Func<TEntity, TProperty>> selector)
        {
            Contract.Requires(selector != null);

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

        private string ObjectToSqlStringValue(object obj)
        {
            if (obj == null)
                return "NULL";

            var objType = obj.GetType();

            if (objType.IsEnum) {
                var enumValue = (Enum)obj;
                object numberValue = Convert.ChangeType(enumValue, enumValue.GetTypeCode());
                return $"'{numberValue.ToString()}'";
            }

            if (objType == typeof(DateTime))
                return $"'{((DateTime)obj).ToString("yyyy.MM.dd HH:mm:ss")}'";

            return $"'{obj}'";
        }
    }
}