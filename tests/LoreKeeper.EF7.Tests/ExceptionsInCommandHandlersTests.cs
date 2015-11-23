// -----------------------------------------------------------------------
// <copyright file="ExceptionsInCommandHandlersTests.cs">
// Copyright (c) 2013-2015 Andrey Veselov. All rights reserved.
// License: Apache License 2.0
// Contacts: http://andrey.moveax.com andrey@moveax.com
// </copyright>
// -----------------------------------------------------------------------

namespace LoreKeeper.EF7.Tests
{
    using System;
    using System.Threading.Tasks;
    using LoreKeeper.Core;
    using LoreKeeper.EF7.Tests.Fixtures;
    using LoreKeeper.Tests.Core.Commands;
    using Xunit;

    [Collection("DatabaseCollection")]
    public class ExceptionsInCommandHandlersTests
    {
        private readonly DatabaseFixture _dbFixture;
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        public ExceptionsInCommandHandlersTests(DatabaseFixture dbFixture)
        {
            this._dbFixture = dbFixture;
            this._unitOfWorkFactory = dbFixture.UnitOfWorkFactory;
        }

        [Fact]
        public void CatchingSyncException()
        {
            // Arrange
            this._dbFixture.TestInitialize();

            var cmd = new AlwaysFailingSyncCommand();
            bool isExceptionCatched = false;

            // Act
            try {
                this._unitOfWorkFactory.ExecuteSingleCommand(cmd);
            }
            catch (Exception) {
                isExceptionCatched = true;
            }

            // Assert
            Assert.True(isExceptionCatched);
        }

        [Fact]
        public async Task CatchingAsyncException()
        {
            // Arrange
            this._dbFixture.TestInitialize();

            var cmd = new AlwaysFailingAsyncCommand();
            bool isExceptionCatched = false;

            // Act
            try {
                using (var unitOfWork = this._unitOfWorkFactory.Create())
                    await unitOfWork.ExecuteCommandAsync(cmd);
            }
            catch (Exception) {
                isExceptionCatched = true;
            }

            // Assert
            Assert.True(isExceptionCatched);
        }
    }
}
