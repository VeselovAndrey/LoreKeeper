// -----------------------------------------------------------------------
// <copyright file="DatabaseCollection.cs">
// Copyright (c) 2015 Andrey Veselov. All rights reserved.
// License: Apache License 2.0
// Contacts: http://andrey.moveax.com andrey@moveax.com
// </copyright>
// -----------------------------------------------------------------------

namespace LoreKeeper.EF7.Tests.Fixtures
{
    using Xunit;

    [CollectionDefinition("DatabaseCollection")]
    public class DatabaseCollection : ICollectionFixture<DatabaseFixture>
    {
    }
}