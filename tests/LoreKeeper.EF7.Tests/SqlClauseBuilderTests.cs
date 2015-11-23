// -----------------------------------------------------------------------
// <copyright file="SqlClauseBuilderTests.cs">
// Copyright (c) 2013-2015 Andrey Veselov. All rights reserved.
// License: Apache License 2.0
// Contacts: http://andrey.moveax.com andrey@moveax.com
// </copyright>
// -----------------------------------------------------------------------

namespace LoreKeeper.EF7.Tests
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using LoreKeeper.EF7.AdvancedFeatures;
    using LoreKeeper.Tests.Core.Models;
    using Xunit;

    public class SqlClauseBuilderTests
    {
        [Fact]
        public void SqlClauseBuilderSimpleWithConst()
        {
            // Arrange
            const string expectedSql = "(Id = '5')";
            Expression<Func<User, bool>> clause = u => u.Id == 5;

            var sqlClauseBuilder = new SqlClauseBuilder();

            // Act
            string actualSql = sqlClauseBuilder.Build(clause.Body);

            // Assert
            Assert.Equal(expectedSql, actualSql);
        }

        [Fact]
        public void SqlClauseBuilderSimpleWithVaue()
        {
            // Arrange
            string name = "some Name";
            string expectedSql = $"(Name = '{name}')";

            Expression<Func<User, bool>> clause = u => u.Name == name;

            var sqlClauseBuilder = new SqlClauseBuilder();

            // Act
            string actualSql = sqlClauseBuilder.Build(clause.Body);

            // Assert
            Assert.Equal(expectedSql, actualSql);
        }

        [Fact]
        public void SqlClauseBuilderSimpleWithConvertFromByte()
        {
            // Arrange
            byte id = 5;
            const string expectedSql = "(Id = '5')";

            Expression<Func<User, bool>> clause = u => u.Id == id;

            var sqlClauseBuilder = new SqlClauseBuilder();

            // Act
            string actualSql = sqlClauseBuilder.Build(clause.Body);

            // Assert
            Assert.Equal(expectedSql, actualSql);
        }

        [Fact]
        public void SqlClauseBuilderSimpleWithConvertFromEnum()
        {
            // Arrange
            var userType = UserType.Reader;
            string expectedSql = $"(Type = {(byte)UserType.Reader})";

            Expression<Func<User, bool>> clause = u => u.Type == userType;

            var sqlClauseBuilder = new SqlClauseBuilder();

            // Act
            string actualSql = sqlClauseBuilder.Build(clause.Body);

            // Assert
            Assert.Equal(expectedSql, actualSql);
        }

        [Fact]
        public void SqlClauseBuilderPropertyIsNullConst()
        {
            // Arrange
            string expectedSql = "(Name IS NULL)";

            Expression<Func<User, bool>> clause = u => u.Name == null;

            var sqlClauseBuilder = new SqlClauseBuilder();

            // Act
            string actualSql = sqlClauseBuilder.Build(clause.Body);

            // Assert
            Assert.Equal(expectedSql, actualSql);
        }

        [Fact]
        public void SqlClauseBuilderPropertyNotNullConst()
        {
            // Arrange
            string expectedSql = "(Name IS NOT NULL)";

            Expression<Func<User, bool>> clause = u => u.Name != null;

            var sqlClauseBuilder = new SqlClauseBuilder();

            // Act
            string actualSql = sqlClauseBuilder.Build(clause.Body);

            // Assert
            Assert.Equal(expectedSql, actualSql);
        }

        [Fact]
        public void SqlClauseBuilderAndClause()
        {
            // Arrange
            string name = "some Name";
            string expectedSql = $"((Id = '5') AND (Name = '{name}'))";

            Expression<Func<User, bool>> clause = u => u.Id == 5 && u.Name == name;

            var sqlClauseBuilder = new SqlClauseBuilder();

            // Act
            string actualSql = sqlClauseBuilder.Build(clause.Body);

            // Assert
            Assert.Equal(expectedSql, actualSql);
        }

        [Fact]
        public void SqlClauseBuilderNotClause()
        {
            // Arrange
            int id = 42;
            string expectedSql = $"(Id != '{id}')";

            Expression<Func<User, bool>> clause = u => u.Id != id;

            var sqlClauseBuilder = new SqlClauseBuilder();

            // Act
            string actualSql = sqlClauseBuilder.Build(clause.Body);

            // Assert
            Assert.Equal(expectedSql, actualSql);
        }

        [Fact]
        public void SqlClauseBuilderBooleanClauseWithConst()
        {
            // Arrange
            const string expectedSql = "(IsDisabled != 1)";

            Expression<Func<User, bool>> clause = u => u.IsDisabled != true;

            var sqlClauseBuilder = new SqlClauseBuilder();

            // Act
            string actualSql = sqlClauseBuilder.Build(clause.Body);

            // Assert
            Assert.Equal(expectedSql, actualSql);
        }

        [Fact]
        public void SqlClauseBuilderBooleanClauseWithValue()
        {
            // Arrange
            const bool isDisabled = false;
            string expectedSql = $"(IsDisabled != {(isDisabled ? "1" : "0")})";

            Expression<Func<User, bool>> clause = u => u.IsDisabled != isDisabled;

            var sqlClauseBuilder = new SqlClauseBuilder();

            // Act
            string actualSql = sqlClauseBuilder.Build(clause.Body);

            // Assert
            Assert.Equal(expectedSql, actualSql);
        }

        [Fact]
        public void SqlClauseBuilderBooleanProperty()
        {
            // Arrange
            const string expectedSql = "(IsDisabled = 1)";

            Expression<Func<User, bool>> clause = u => u.IsDisabled;

            var sqlClauseBuilder = new SqlClauseBuilder();

            // Act
            string actualSql = sqlClauseBuilder.Build(clause.Body);

            // Assert
            Assert.Equal(expectedSql, actualSql);
        }

        [Fact]
        public void SqlClauseBuilderNotProperty()
        {
            // Arrange
            const string expectedSql = "(IsDisabled = 0)";

            Expression<Func<User, bool>> clause = u => !u.IsDisabled;

            var sqlClauseBuilder = new SqlClauseBuilder();

            // Act
            string actualSql = sqlClauseBuilder.Build(clause.Body);

            // Assert
            Assert.Equal(expectedSql, actualSql);
        }

        [Fact]
        public void SqlClauseBuilderNotExpression()
        {
            // Arrange
            const int id = 42;
            string expectedSql = $"(NOT (Id > '{id}'))";

            Expression<Func<User, bool>> clause = u => !(u.Id > id);

            var sqlClauseBuilder = new SqlClauseBuilder();

            // Act
            string actualSql = sqlClauseBuilder.Build(clause.Body);

            // Assert
            Assert.Equal(expectedSql, actualSql);
        }

        [Fact]
        public void SqlClauseBuilderOrClause()
        {
            // Arrange
            const int id = 42;
            string expectedSql = $"((Id = '{id}') OR (Name = 'some Value'))";

            Expression<Func<User, bool>> clause = u => u.Id == id || u.Name == "some Value";

            var sqlClauseBuilder = new SqlClauseBuilder();

            // Act
            string actualSql = sqlClauseBuilder.Build(clause.Body);

            // Assert
            Assert.Equal(expectedSql, actualSql);
        }

        [Fact]
        public void SqlClauseBuilderAndOrClause()
        {
            // Arrange
            const int id = 42;
            var now = DateTime.Now;
            string expectedSql = $"((Id = '{id}') OR ((Name = 'some Value') AND (Created < '{now.ToString("yyyy.MM.dd HH:mm:ss")}')))";

            Expression<Func<User, bool>> clause = u => u.Id == id || (u.Name == "some Value" && u.Created < now);

            var sqlClauseBuilder = new SqlClauseBuilder();

            // Act
            string actualSql = sqlClauseBuilder.Build(clause.Body);

            // Assert
            Assert.Equal(expectedSql, actualSql);
        }

        [Fact]
        public void SqlClauseBuilderWithBuilderExpression()
        {
            // Arrange
            const int id = 42;
            var now = DateTime.Now;
            string expectedSql = $"(((Id = '{id}') AND (Created < '{now.ToString("yyyy.MM.dd HH:mm:ss")}')) AND (Name = 'some Value'))";

            Expression<Func<User, bool>> clause = u => u.Id == id;
            clause = this.Combine(clause, u => u.Created < now);
            clause = this.Combine(clause, u => u.Name == "some Value");

            var sqlClauseBuilder = new SqlClauseBuilder();

            // Act
            string actualSql = sqlClauseBuilder.Build(clause.Body);

            // Assert
            Assert.Equal(expectedSql, actualSql);
        }

        private Expression<Func<TParam, TResult>> Combine<TParam, TResult>(Expression<Func<TParam, TResult>> a, Expression<Func<TParam, TResult>> b)
        {
            if (a == null)
                return b;

            ParameterExpression firstParameter = a.Parameters.First();
            Expression<Func<TParam, TResult>> b1 = Expression.Lambda<Func<TParam, TResult>>(Expression.Invoke(b, firstParameter), firstParameter);
            return Expression.Lambda<Func<TParam, TResult>>(Expression.And(a.Body, b1.Body), firstParameter);
        }
    }
}