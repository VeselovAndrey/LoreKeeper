// -----------------------------------------------------------------------
// <copyright file="CrudAsyncTests.cs">
// Copyright (c) 2013-2015 Andrey Veselov. All rights reserved.
// License: Apache License 2.0
// Contacts: http://andrey.moveax.com andrey@moveax.com
// </copyright>
// -----------------------------------------------------------------------

namespace LoreKeeper.EF6.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using LoreKeeper.Core;
    using LoreKeeper.EF6.Tests.DataAccess.Database.Dto;
    using LoreKeeper.EF6.Tests.Fixtures;
    using LoreKeeper.Tests.Core.Commands.Users;
    using LoreKeeper.Tests.Core.Models;
    using LoreKeeper.Tests.Core.Queries;
    using LoreKeeper.Tests.Core.Queries.Links;
    using LoreKeeper.Tests.Core.Queries.Users;
    using Xunit;

    [Collection("DatabaseCollection")]
    public class CrudAsyncTests
    {
        private readonly DatabaseFixture _dbFixture;
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly User _sampleData;

        public CrudAsyncTests(DatabaseFixture dbFixture)
        {
            this._dbFixture = dbFixture;
            this._unitOfWorkFactory = dbFixture.UnitOfWorkFactory;
            this._sampleData = dbFixture.SampleData;
        }

        [Fact]
        public async Task GetExistingObjectAsync()
        {
            // Arrange 
            this._dbFixture.TestInitialize();

            User profile = null;

            // Act
            using (var unitOfWork = this._unitOfWorkFactory.Create())
                profile = await unitOfWork.ResolveQuery<IGetUserByIdAsyncQuery>().ExecuteAsync(this._sampleData.Id, includeLinks: false);

            // Assert
            Assert.NotNull(profile);
            Assert.Equal(this._sampleData.Id, profile.Id);
            Assert.Equal(this._sampleData.Name, profile.Name);
            Assert.Equal(this._sampleData.Email, profile.Email);
            Assert.Equal(this._sampleData.IsDisabled, profile.IsDisabled);
        }

        [Fact]
        public async Task GetNonexistentObjectAsync()
        {
            // Arrange 
            this._dbFixture.TestInitialize();

            User profile = null;

            // Act
            using (var unitOfWork = this._unitOfWorkFactory.Create())
                profile = await unitOfWork.ResolveQuery<IGetUserByIdAsyncQuery>().ExecuteAsync(int.MaxValue, includeLinks: false);

            // Assert
            Assert.Null(profile);
        }

        [Fact]
        public async Task GetObjectIncludingNavigationPropertiesAsync()
        {
            // Arrange 
            this._dbFixture.TestInitialize();

            User profile = null;

            // Act
            using (var unitOfWork = this._unitOfWorkFactory.Create())
                profile = await unitOfWork.ResolveQuery<IGetUserByIdAsyncQuery>().ExecuteAsync(this._sampleData.Id, includeLinks: true);

            // Assert
            Assert.NotNull(profile);
            Assert.Equal(this._sampleData.Id, profile.Id);
            Assert.Equal(this._sampleData.Name, profile.Name);
            Assert.Equal(this._sampleData.Email, profile.Email);
            Assert.Equal(this._sampleData.IsDisabled, profile.IsDisabled);

            Assert.NotNull(profile.Links);
            Assert.Equal(this._sampleData.Links.Count(), profile.Links.Count());

            bool allLinksWasFound = profile.Links
                .Select(l => this._sampleData.Links.Any(e => (e.Id == l.Id)))
                .Aggregate(true, (acc, val) => acc && val);

            Assert.True(allLinksWasFound);
        }

        [Fact]
        public async Task ExcecuteCommandAsync()
        {
            // Arrange
            this._dbFixture.TestInitialize();

            const string name = "John Doe";
            const string email = "johndoe@unknownserver.doe";
            const bool isDisabled = false;
            var links = new[] { new Link() { Title = "SomeTitle", Url = @"http://somesite.com/" } };

            User savedData = null;

            var cmd = new CreateUserCommand(name, email, isDisabled, links);

            // Act
            await this._unitOfWorkFactory.ExecuteSingleCommandAsync(cmd);

            using (var unitOfWork = this._unitOfWorkFactory.Create())
                savedData = await unitOfWork.ResolveQuery<IGetUserByIdAsyncQuery>().ExecuteAsync(cmd.Id, includeLinks: true);

            // Assert
            Assert.NotNull(savedData);
            Assert.Equal(cmd.Id, savedData.Id);
            Assert.Equal(name, savedData.Name);
            Assert.Equal(email, savedData.Email);
            Assert.Equal(isDisabled, savedData.IsDisabled);

            Assert.NotNull(savedData.Links);
            Assert.Equal(links.Count(), savedData.Links.Count());

            var link = savedData.Links.First();
            Assert.Equal(savedData.Id, link.UserProfileId);
            Assert.Equal(links[0].Title, link.Title);
            Assert.Equal(links[0].Url, link.Url);
        }

        [Fact]
        public async Task ExecuteAdvancedCommandAsync()
        {
            // Arrange 
            this._dbFixture.TestInitialize();

            User user = null;
            const string newName = "John Doe";

            // NOTE: handler for this command should update login only (without reading entire entity)
            var cmd = new UpdateUserNameCommand(this._sampleData.Id, newName);

            // Act
            await this._unitOfWorkFactory.ExecuteSingleCommandAsync(cmd);

            user = this._unitOfWorkFactory
                .ExecuteSingleQuery<IGetUserByIdQuery, User>(q => q.Execute(cmd.Id, includeLinks: false));

            // Assert
            Assert.NotNull(user);
            Assert.Equal(this._sampleData.Id, user.Id);
            Assert.Equal(newName, user.Name);
            Assert.Equal(this._sampleData.IsDisabled, user.IsDisabled);
        }

        [Fact]
        public async Task ExecuteQueriesAsync()
        {
            // Arrange 
            this._dbFixture.TestInitialize();

            User user = null;
            IEnumerable<Link> links = null;

            // Act
            using (var unitOfWork = this._unitOfWorkFactory.Create()) {
                user = await unitOfWork
                    .ResolveQuery<IGetUserByIdAsyncQuery>()
                    .ExecuteAsync(this._sampleData.Id, includeLinks: true)
                    .ConfigureAwait(false);

                links = await unitOfWork
                    .ResolveQuery<IGetLinksByUserIdAsyncQuery>()
                    .ExecuteAsync(this._sampleData.Id)
                    .ConfigureAwait(false);
            }

            // Assert
            Assert.NotNull(user);
            Assert.NotNull(user.Links);
            Assert.NotNull(links);

            Assert.Equal(user.Links.Count(), links.Count());

            bool allLinksWasFound = user.Links
                .Select(l => links.Any(e => (e.Id == l.Id)))
                .Aggregate(true, (acc, val) => acc && val);

            Assert.True(allLinksWasFound);
        }

        [Fact]
        public async Task ExecuteCommandsAsync()
        {
            // Arrange 
            this._dbFixture.TestInitialize();

            const string email = "johndoe@unknownserver.doe";
            const int insertedObjectsCount = 20;

            using (var unitOfWork = this._unitOfWorkFactory.Create()) {
                this._dbFixture.InsertObjectsIntoRepository(unitOfWork, insertedObjectsCount);
            }

            // Act
            var users = this._unitOfWorkFactory.ExecuteSingleQuery<IGetAllQuery, IEnumerable<UserDto>>(q => q.Execute<UserDto>());

            using (var unitOfWork = this._unitOfWorkFactory.Create()) {
                foreach (var user in users) {
                    var cmd = new UpdateUserEmailCommand(user.Id, email);
                    await unitOfWork.ExecuteCommandAsync(cmd);
                }

                await unitOfWork.CommitAsync();
            }

            users = this._unitOfWorkFactory.ExecuteSingleQuery<IGetAllQuery, IEnumerable<UserDto>>(q => q.Execute<UserDto>());

            // Assert
            Assert.NotNull(users);
            Assert.True(users.All(u => string.Equals(u.Email, email, StringComparison.Ordinal)));
        }
    }
}