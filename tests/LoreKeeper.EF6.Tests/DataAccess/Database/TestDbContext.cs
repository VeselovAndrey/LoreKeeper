// -----------------------------------------------------------------------
// <copyright file="TestDbContext.cs">
// Copyright (c) 2013-2015 Andrey Veselov. All rights reserved.
// License: Apache License 2.0
// Contacts: http://andrey.moveax.com andrey@moveax.com
// </copyright>
// -----------------------------------------------------------------------

namespace LoreKeeper.EF6.Tests.DataAccess.Database
{
	using System.Data.Entity;
	using LoreKeeper.EF6.Tests.DataAccess.Database.DbMaps;

	internal class TestDbContext : DbContext
	{
		public TestDbContext()
		{
			this.Configuration.LazyLoadingEnabled = false;
			this.Configuration.ProxyCreationEnabled = false;
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Configurations.Add(new UserDtoMap());
			modelBuilder.Configurations.Add(new LinkDtoMap());
		}
	}
}