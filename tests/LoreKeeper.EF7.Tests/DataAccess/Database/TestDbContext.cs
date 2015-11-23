// -----------------------------------------------------------------------
// <copyright file="TestDbContext.cs">
// Copyright (c) 2013-2015 Andrey Veselov. All rights reserved.
// License: Apache License 2.0
// Contacts: http://andrey.moveax.com andrey@moveax.com
// </copyright>
// -----------------------------------------------------------------------

namespace LoreKeeper.EF7.Tests.DataAccess.Database
{
    using LoreKeeper.EF7.Tests.DataAccess.Database.DbMaps;
    using Microsoft.Data.Entity;

    internal class TestDbContext : DbContext
    {
        public TestDbContext()
        { 
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            const string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=TestEF7DbContext;Integrated Security=True;MultipleActiveResultSets=True";
            optionsBuilder.UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            new UserDtoMap().Configure(modelBuilder);
            new LinkDtoMap().Configure(modelBuilder);
        }
    }
}