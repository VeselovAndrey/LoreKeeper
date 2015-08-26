// -----------------------------------------------------------------------
// <copyright file="CrudTests.cs">
// Copyright (c) 2012-2015 Andrey Veselov. All rights reserved.
// License: Apache License 2.0
// Contacts: http://andrey.moveax.com andrey@moveax.com
// </copyright>
// -----------------------------------------------------------------------

namespace LoreKeeper.EF6.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using LoreKeeper.Core;
    using LoreKeeper.EF6.Tests.DataAccess.Database.Dto;
    using LoreKeeper.EF6.Tests.Fixtures;
    using LoreKeeper.Tests.Core.Commands.Links;
    using LoreKeeper.Tests.Core.Commands.Users;
    using LoreKeeper.Tests.Core.Models;
    using LoreKeeper.Tests.Core.Queries;
    using LoreKeeper.Tests.Core.Queries.Links;
    using LoreKeeper.Tests.Core.Queries.Users;
    using Moq;
    using Xunit;

    [Collection("DatabaseCollection")]
    public class CrudTests
    {
        private readonly DatabaseFixture _dbFixture;
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly User _sampleData;

        public CrudTests(DatabaseFixture dbFixture)
        {
            this._dbFixture = dbFixture;
            this._unitOfWorkFactory = dbFixture.UnitOfWorkFactory;
            this._sampleData = dbFixture.SampleData;
        }

        [Fact]
        public void GetExistingObject()
        {
            // Arrange 
            this._dbFixture.TestInitialize();

            User profile = null;

            // Act
            using (var unitOfWork = this._unitOfWorkFactory.Create())
                profile = unitOfWork.ResolveQuery<IGetUserByIdQuery>().Execute(this._sampleData.Id, includeLinks: false);

            // Assert
            Assert.NotNull(profile);
            Assert.Equal(this._sampleData.Id, profile.Id);
            Assert.Equal(this._sampleData.Name, profile.Name);
            Assert.Equal(this._sampleData.Email, profile.Email);
            Assert.Equal(this._sampleData.IsDisabled, profile.IsDisabled);
        }

        [Fact]
        public void GetNonexistentObject()
        {
            // Arrange 
            this._dbFixture.TestInitialize();

            User profile = null;

            // Act
            using (var unitOfWork = this._unitOfWorkFactory.Create())
                profile = unitOfWork.ResolveQuery<IGetUserByIdQuery>().Execute(int.MaxValue, includeLinks: false);

            // Assert
            Assert.Null(profile);
        }

        [Fact]
        public void GetObjectIncludingNavigationProperties()
        {
            // Arrange 
            this._dbFixture.TestInitialize();

            User profile = null;

            // Act
            using (var unitOfWork = this._unitOfWorkFactory.Create())
                profile = unitOfWork.ResolveQuery<IGetUserByIdQuery>().Execute(this._sampleData.Id, includeLinks: true);

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
        public void AddObject()
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
            this._unitOfWorkFactory.ExecuteSingleCommand(cmd);

            using (var unitOfWork = this._unitOfWorkFactory.Create())
                savedData = unitOfWork.ResolveQuery<IGetUserByIdQuery>().Execute(cmd.Id, includeLinks: true);

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
        public void RemoveObject()
        {
            // Arrange
            this._dbFixture.TestInitialize();

            int initialCount = 0;
            var cmd = new RemoveUserCommand(this._sampleData.Id);

            // Act
            initialCount = this._unitOfWorkFactory
                .ExecuteSingleQuery<IGetCountQuery, int>(q => q.Execute<UserDto>());

            this._unitOfWorkFactory.ExecuteSingleCommand(cmd);

            int resultCount = this._unitOfWorkFactory
                .ExecuteSingleQuery<IGetCountQuery, int>(q => q.Execute<UserDto>());

            // Assert
            Assert.Equal(initialCount - 1, resultCount);
        }

        [Fact]
        public void RemoveMatchedObjects()
        {
            // Arrange
            this._dbFixture.TestInitialize();

            const int insertedObjectsCount = 10;

            using (var unitOfWork = this._unitOfWorkFactory.Create()) {
                this._dbFixture.InsertObjectsIntoRepository(unitOfWork, insertedObjectsCount);
                unitOfWork.Commit();
            }

            IEnumerable<UserDto> users = this._unitOfWorkFactory
                .ExecuteSingleQuery<IGetAllQuery, IEnumerable<UserDto>>(q => q.Execute<UserDto>());

            if (!users.Any(u => u.IsDisabled))
                throw new InvalidOperationException("There are no disabled users in the database.");

            var cmd = new RemoveDisabledUsersCommand();

            // Act
            this._unitOfWorkFactory.ExecuteSingleCommand(cmd);

            users = this._unitOfWorkFactory
                .ExecuteSingleQuery<IGetAllQuery, IEnumerable<UserDto>>(q => q.Execute<UserDto>());

            // Assert
            Assert.True(users.All(u => !u.IsDisabled));
        }

        [Fact]
        public void UpdateSingleObject()
        {
            // Arrange
            this._dbFixture.TestInitialize();

            User user = null;
            const string name = "John Doe";
            const string email = "johndoe@unknownserver.doe";
            const bool isDisabled = false;

            // NOTE: handler for this command should read entire profile and update rating value
            var cmd = new UpdateUserCommand(this._sampleData.Id, name, email, isDisabled);

            // Act
            this._unitOfWorkFactory.ExecuteSingleCommand(cmd);

            user = this._unitOfWorkFactory
                .ExecuteSingleQuery<IGetUserByIdQuery, User>(q => q.Execute(cmd.Id, includeLinks: false));

            // Assert
            Assert.NotNull(user);
            Assert.Equal(this._sampleData.Id, user.Id);
            Assert.Equal(name, user.Name);
            Assert.Equal(email, user.Email);
            Assert.Equal(isDisabled, user.IsDisabled);
        }

        [Fact]
        public void UpdateMultipleObjects()
        {
            // Validate command handler (it must call Update() method)
            var mockedRepository = new Mock<IRepository>();
            var cmdHandler = new DataAccess.CommandHandlers.Users.DisableOldUsersCommandHandler(mockedRepository.Object);
            cmdHandler.Execute(new DisableOldUsersCommand(DateTime.UtcNow));
            mockedRepository.Verify(r => r.Update(
                It.IsAny<UserDto>(),
                It.IsAny<Expression<Func<UserDto, bool>>>(),
                It.IsAny<Expression<Func<UserDto, object>>[]>()),
                Times.AtLeastOnce());

            // Arrange
            this._dbFixture.TestInitialize();

            const int insertedObjectsCount = 10;

            using (var unitOfWork = this._unitOfWorkFactory.Create()) {
                this._dbFixture.InsertObjectsIntoRepository(unitOfWork, insertedObjectsCount);
                unitOfWork.Commit();
            }

            IEnumerable<UserDto> usersBefore = this._unitOfWorkFactory
                .ExecuteSingleQuery<IGetAllQuery, IEnumerable<UserDto>>(q => q.Execute<UserDto>());

            // NOTE: handler for this command should update all entries at once
            var cmd = new DisableOldUsersCommand(DateTime.Now);

            // Act
            this._unitOfWorkFactory.ExecuteSingleCommand(cmd);

            IEnumerable<UserDto> usersAfter = this._unitOfWorkFactory
                .ExecuteSingleQuery<IGetAllQuery, IEnumerable<UserDto>>(q => q.Execute<UserDto>());

            // Assert
            Assert.Equal(usersBefore.Count(), usersAfter.Count());

            foreach (var user in usersAfter) {
                var userBefore = usersBefore.FirstOrDefault(u => u.Id == user.Id);
                Assert.NotNull(userBefore);
                Assert.Equal(userBefore.Name, user.Name);
                Assert.Equal(userBefore.Email, user.Email);
                Assert.Equal(userBefore.Created, user.Created);
                Assert.True(user.IsDisabled);
            }
        }

        [Fact]
        public void UpdateSpecifiedProperties()
        {
            // Arrange 
            this._dbFixture.TestInitialize();

            const string newName = "John Doe";

            // Validate command handler (it must call UpdateSpecifiedProperties() method)
            var mockedRepository = new Mock<IRepository>();
            var cmdHandler = new DataAccess.CommandHandlers.Users.UpdateUserNameCommandHandler(mockedRepository.Object);
            cmdHandler.Execute(new UpdateUserNameCommand(this._sampleData.Id, newName));
            mockedRepository.Verify(r => r.UpdateSpecifiedProperties(
                It.IsAny<UserDto>(),
                It.IsAny<Expression<Func<UserDto, object>>[]>()),
                Times.AtLeastOnce());

            // NOTE: handler for this command should update login only (without reading entire entity)
            var cmd = new UpdateUserNameCommand(this._sampleData.Id, newName);

            // Act
            this._unitOfWorkFactory.ExecuteSingleCommand(cmd);

            User user = this._unitOfWorkFactory
                .ExecuteSingleQuery<IGetUserByIdQuery, User>(q => q.Execute(cmd.Id, includeLinks: false));

            // Assert
            Assert.NotNull(user);
            Assert.Equal(this._sampleData.Id, user.Id);
            Assert.Equal(newName, user.Name);
            Assert.Equal(this._sampleData.IsDisabled, user.IsDisabled);
        }

        [Fact]
        public void UpdateExcludeProperties()
        {
            // Arrange
            this._dbFixture.TestInitialize();

            Link expectedLinkInfo = this._sampleData.Links.First();
            Link resultLinkInfo = null;
            var newTitle = DateTime.Now.ToShortTimeString();

            // Validate command handler (it must call UpdateExcludeProperties() method)
            var mockedRepository = new Mock<IRepository>();
            var cmdHandler = new DataAccess.CommandHandlers.Links.UpdateLinkTitleCommandHandler(mockedRepository.Object);
            cmdHandler.Execute(new UpdateLinkTitleCommand(expectedLinkInfo.Id, newTitle));
            mockedRepository.Verify(r => r.UpdateExcludeProperties(
                It.IsAny<LinkDto>(),
                It.IsAny<Expression<Func<LinkDto, object>>[]>()),
                Times.AtLeastOnce());

            // NOTE: handler for this command should update title only (without reading entire entity)
            var cmd = new UpdateLinkTitleCommand(expectedLinkInfo.Id, newTitle);

            // Act
            this._unitOfWorkFactory.ExecuteSingleCommand(cmd);

            resultLinkInfo = this._unitOfWorkFactory
                .ExecuteSingleQuery<IGetLinksByUserIdQuery, IEnumerable<Link>>(q => q.Execute(this._sampleData.Id))
                .FirstOrDefault(l => l.Id == cmd.Id);

            // Assert
            Assert.NotNull(resultLinkInfo);
            Assert.Equal(expectedLinkInfo.Id, resultLinkInfo.Id);
            Assert.Equal(newTitle, resultLinkInfo.Title);
            Assert.Equal(expectedLinkInfo.Url, resultLinkInfo.Url);
            Assert.Equal(expectedLinkInfo.UserProfileId, resultLinkInfo.UserProfileId);
        }
    }
}