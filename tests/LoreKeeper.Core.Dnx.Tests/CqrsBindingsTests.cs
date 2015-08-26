// -----------------------------------------------------------------------
// <copyright file="CqrsBindingsTests.cs">
// Copyright (c) 2013 Andrey Veselov. All rights reserved.
// License: Apache License 2.0
// Contacts: http://andrey.moveax.com andrey@moveax.com
// </copyright>
// -----------------------------------------------------------------------

namespace LoreKeeper.Core.Tests
{
    using System.Linq;
    using System.Reflection;
    using Xunit;

    public class CqrsBindingsTests
    {
        private static string _modelAssembly = "LoreKeeper.Tests.Core.Dnx";
        private static string _dataAccessAssembly = "LoreKeeper.Tests.DataAccess.Dnx";

        [Fact]
        public void BindQueries()
        {
            // Arrange
            var modelsAssembly = Assembly.Load(new AssemblyName(CqrsBindingsTests._modelAssembly));
            var dalAssembly = Assembly.Load(new AssemblyName(CqrsBindingsTests._dataAccessAssembly));
            var bindings = new CqrsBindings();

            // Act
            bindings.CollectQueryInterfaces(modelsAssembly);
            bindings.BindQueries(dalAssembly);

            // Assert
            Assert.True(bindings.Validate());
            Assert.NotNull(bindings.QueryBindings);
            Assert.True(bindings.QueryBindings.Any());

            foreach (var query in bindings.QueryBindings)
                Assert.True(query.Interface.IsAssignableFrom(query.Implementation));
        }

        [Fact]
        public void BindCommands()
        {
            // Arrange
            var modelsAssembly = Assembly.Load(new AssemblyName(CqrsBindingsTests._modelAssembly));
            var dalAssembly = Assembly.Load(new AssemblyName(CqrsBindingsTests._dataAccessAssembly));
            var bindings = new CqrsBindings();

            // Act
            bindings.CollectCommands(modelsAssembly);
            bindings.BindCommandHandlers(dalAssembly);

            // Assert
            Assert.True(bindings.Validate());
            Assert.NotNull(bindings.CommandBindings);
            Assert.True(bindings.CommandBindings.Any());

            foreach (var command in bindings.CommandBindings) {
                foreach (var handler in command.Handlers) {
                    var genType = typeof(ICommandHandler<>).MakeGenericType(command.Command);
                    Assert.True(genType.IsAssignableFrom(handler));
                }
            }
        }
    }
}
