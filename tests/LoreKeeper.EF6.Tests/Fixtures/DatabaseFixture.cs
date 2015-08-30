// -----------------------------------------------------------------------
// <copyright file="DatabaseFixture.cs">
// Copyright (c) 2015 Andrey Veselov. All rights reserved.
// License: Apache License 2.0
// Contacts: http://andrey.moveax.com andrey@moveax.com
// </copyright>
// -----------------------------------------------------------------------

namespace LoreKeeper.EF6.Tests.Fixtures
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using LoreKeeper.Bootstrappers.Autofac;
    using LoreKeeper.Core;
    using LoreKeeper.EF6.Tests.DataAccess.Database;
    using LoreKeeper.EF6.Tests.DataAccess.Database.Dto;
    using LoreKeeper.Tests.Core.Commands.Links;
    using LoreKeeper.Tests.Core.Commands.Users;
    using LoreKeeper.Tests.Core.Models;
    using LoreKeeper.Tests.Core.Queries;

    public sealed class DatabaseFixture : IDisposable
    {
        private static string _modelAssembly = "LoreKeeper.Tests.Core";
        private static string _dataAccessAssembly = "LoreKeeper.EF6.Tests";

        public IUnitOfWorkFactory UnitOfWorkFactory { get; }

        public User SampleData { get; }

        public DatabaseFixture()
        {
            // Set sample data 
            this.SampleData = new User() {
                Name = "Andrey Veselov",
                Email = "andrey@moveax.com",
                IsDisabled = false,
                Links = new List<Link>() {
                    new Link() {Title = "Project home page", Url = @"http://highway.codeplex.com/"},
                    new Link() {Title = "Andrey on .NET blog", Url = @"http://andrey.moveax.com/"}
                }
            };

            // Load queries and command handlers from test assemblies
            var modelsAssembly = Assembly.Load(DatabaseFixture._modelAssembly);
            var dalAssembly = Assembly.Load(DatabaseFixture._dataAccessAssembly);
            var builder = new CqrsDependencyResolverBuilder();

            builder.CollectQueryInterfaces(modelsAssembly);
            builder.CollectCommands(modelsAssembly);

            builder.BindQueries(dalAssembly);
            builder.BindCommandHandlers(dalAssembly);

            this.UnitOfWorkFactory = new EFUnitOfWorkFactory<TestDbContext>(builder.Build());
        }

        public void TestInitialize()
        {
            // Cleanup first 
            this.TestCleanup();

            // Set data
            var userCommand = new CreateUserCommand(this.SampleData.Name, this.SampleData.Email, this.SampleData.IsDisabled, null);
            this.UnitOfWorkFactory.ExecuteSingleCommand(userCommand);

            this.SampleData.Id = userCommand.Id;
            this.SampleData.Created = userCommand.Created;

            foreach (Link link in this.SampleData.Links) {
                var linkCommand = new CreateLinkCommand(userCommand.Id, link.Title, link.Url);
                this.UnitOfWorkFactory.ExecuteSingleCommand(linkCommand);
                link.Id = linkCommand.Id;
                link.UserProfileId = userCommand.Id;
            }
        }

        public void TestCleanup()
        {
            using (var unitOfWork = this.UnitOfWorkFactory.Create()) {
                var entities = unitOfWork.ResolveQuery<IGetAllQuery>()
                    .Execute<UserDto>();

                foreach (var entity in entities) {
                    var command = new RemoveUserCommand(entity.Id);
                    unitOfWork.ExecuteCommand(command);
                }

                unitOfWork.Commit();
            }
        }

        public void InsertObjectsIntoRepository(IUnitOfWork unitOfWork, int objectsCount)
        {
            for (var i = 1; i <= objectsCount; ++i) {
                var cmd = new CreateUserCommand(
                    name: $"UserName{i}",
                    email: $"userName{i}@site{i}.com",
                    isDisabled: (i % 2) == 1,
                    links: new[] { new Link() {
                            Title = $"Link{i}",
                            Url = $@"http://site{i}.com"
                    }});

                unitOfWork.ExecuteCommand(cmd);
            }
        }

        public void Dispose()
        {
            using (var context = new TestDbContext())
                context.Database.Delete();
        }
    }
}
