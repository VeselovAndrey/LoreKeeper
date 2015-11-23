// -----------------------------------------------------------------------
// <copyright file="TransactionsTests.cs">
// Copyright (c) 2012-2015 Andrey Veselov. All rights reserved.
// License: Apache License 2.0
// Contacts: http://andrey.moveax.com andrey@moveax.com
// </copyright>
// -----------------------------------------------------------------------

namespace LoreKeeper.EF7.Tests
{
    using System.Transactions;
    using LoreKeeper.Core;
    using LoreKeeper.EF7.Tests.DataAccess.Database.Dto;
    using LoreKeeper.EF7.Tests.Fixtures;
    using LoreKeeper.Tests.Core.Queries;
    using Xunit;

    [Collection("DatabaseCollection")]
    public class TransactionsTests
    {
        private readonly DatabaseFixture _dbFixture;
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        public TransactionsTests(DatabaseFixture dbFixture)
        {
            this._dbFixture = dbFixture;
            this._unitOfWorkFactory = dbFixture.UnitOfWorkFactory;
        }

        [Fact]
        public void TransactionCommit()
        {
            // Arrange 
            this._dbFixture.TestInitialize();

            int objectCountAtBegin = 0;
            int objectCountAtInnerEnd = 0;
            int objectCountAtEnd = 0;
            const int objectToInsert = 15;

            // Act
            using (var unitOfWork = this._unitOfWorkFactory.Create(enableTransactions: true)) {
                objectCountAtBegin = unitOfWork.ResolveQuery<IGetCountQuery>()
                    .Execute<UserDto>();

                this._dbFixture.InsertObjectsIntoRepository(unitOfWork, objectToInsert);

                using (var innerUnitOfWork = unitOfWork.BeginTransaction()) {
                    this._dbFixture.InsertObjectsIntoRepository(innerUnitOfWork, objectToInsert);
                    innerUnitOfWork.Commit();
                }

                objectCountAtInnerEnd = unitOfWork.ResolveQuery<IGetCountQuery>()
                    .Execute<UserDto>();

                unitOfWork.Commit();
            }

            objectCountAtEnd = this._unitOfWorkFactory.ExecuteSingleQuery<IGetCountQuery, int>(q => q.Execute<UserDto>());

            // Assert
            Assert.Equal(objectCountAtBegin + objectToInsert, objectCountAtInnerEnd);
            Assert.Equal(objectCountAtBegin + objectToInsert * 2, objectCountAtEnd);
        }

        [Fact]
        public void TransactionCommitWithRollback()
        {
            // Arrange
            this._dbFixture.TestInitialize();

            int objectCountAtBegin = 0;
            int objectCountAtInnerEnd = 0;
            int objectCountAtEnd = 0;
            const int objectToInsert = 15;

            // Act
            using (var unitOfWork = this._unitOfWorkFactory.Create(enableTransactions: true)) {
                objectCountAtBegin = unitOfWork.ResolveQuery<IGetCountQuery>().Execute<UserDto>();

                this._dbFixture.InsertObjectsIntoRepository(unitOfWork, objectToInsert);

                using (var innerUnitOfWork = unitOfWork.BeginTransaction(enableInnerTransaction: true)) {
                    this._dbFixture.InsertObjectsIntoRepository(innerUnitOfWork, objectToInsert);
                    innerUnitOfWork.Commit();
                }

                objectCountAtInnerEnd = unitOfWork.ResolveQuery<IGetCountQuery>().Execute<UserDto>();

                unitOfWork.Rollback();
            }

            objectCountAtEnd = this._unitOfWorkFactory.ExecuteSingleQuery<IGetCountQuery, int>(q => q.Execute<UserDto>());

            // Assert
            Assert.Equal(objectCountAtBegin + objectToInsert, objectCountAtInnerEnd);
            Assert.Equal(objectCountAtBegin, objectCountAtEnd);
        }

        [Fact]
        public void TransactionRollbackWithCommit()
        {
            // Arrange
            this._dbFixture.TestInitialize();

            int objectCountAtBegin = 0;
            int objectCountAtInnerEnd = 0;
            int objectCountAtEnd = 0;
            const int objectToInsert = 15;

            // Act
            using (var unitOfWork = this._unitOfWorkFactory.Create(enableTransactions: true)) {
                objectCountAtBegin = unitOfWork.ResolveQuery<IGetCountQuery>().Execute<UserDto>();

                this._dbFixture.InsertObjectsIntoRepository(unitOfWork, objectToInsert);

                using (var innerUnitOfWork = unitOfWork.BeginTransaction()) {
                    this._dbFixture.InsertObjectsIntoRepository(innerUnitOfWork, objectToInsert * 2);
                    innerUnitOfWork.Rollback();
                }

                objectCountAtInnerEnd = unitOfWork.ResolveQuery<IGetCountQuery>().Execute<UserDto>();

                unitOfWork.Commit();
            }

            objectCountAtEnd =
                this._unitOfWorkFactory.ExecuteSingleQuery<IGetCountQuery, int>(q => q.Execute<UserDto>());

            // Assert
            Assert.Equal(objectCountAtBegin, objectCountAtInnerEnd);
            Assert.Equal(objectCountAtBegin + objectToInsert, objectCountAtEnd);
        }

        [Fact]
        public void TransactionRollback()
        {
            // Arrange
            this._dbFixture.TestInitialize();

            int objectCountAtBegin = 0;
            int objectCountAtInnerEnd = 0;
            int objectCountAtEnd = 0;
            const int objectToInsert = 15;

            // Act
            using (var unitOfWork = this._unitOfWorkFactory.Create(enableTransactions: true)) {
                objectCountAtBegin = unitOfWork.ResolveQuery<IGetCountQuery>().Execute<UserDto>();

                this._dbFixture.InsertObjectsIntoRepository(unitOfWork, objectToInsert);

                using (var innerUnitOfWork = unitOfWork.BeginTransaction()) {
                    this._dbFixture.InsertObjectsIntoRepository(innerUnitOfWork, objectToInsert);
                    innerUnitOfWork.Rollback();
                }

                objectCountAtInnerEnd = unitOfWork.ResolveQuery<IGetCountQuery>().Execute<UserDto>();

                unitOfWork.Rollback();
            }

            objectCountAtEnd = this._unitOfWorkFactory.ExecuteSingleQuery<IGetCountQuery, int>(q => q.Execute<UserDto>());

            // Assert
            Assert.Equal(objectCountAtBegin, objectCountAtInnerEnd);
            Assert.Equal(objectCountAtBegin, objectCountAtEnd);
        }
    }
}