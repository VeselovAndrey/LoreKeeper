// -----------------------------------------------------------------------
// <copyright file="LinkDto.cs">
// Copyright (c) 2013-2015 Andrey Veselov. All rights reserved.
// License: Apache License 2.0
// Contacts: http://andrey.moveax.com andrey@moveax.com
// </copyright>
// -----------------------------------------------------------------------

namespace LoreKeeper.EF6.Tests.DataAccess.Database.DbMaps
{
	using System.Data.Entity.ModelConfiguration;
	using LoreKeeper.EF6.Tests.DataAccess.Database.Dto;

	class LinkDtoMap : EntityTypeConfiguration<LinkDto>
	{
		public LinkDtoMap()
		{
			// Primary Key
			this.HasKey(t => new { t.Id });

			// Properties
			this.Property(t => t.Title)
				.IsRequired()
				.HasMaxLength(64);

			this.Property(t => t.Url)
				.IsRequired()
				.HasMaxLength(128);

			// Table & Column Mappings
			this.ToTable("Links");
			this.Property(t => t.Id).HasColumnName("Id");
			this.Property(t => t.UserProfileId).HasColumnName("UserProfileId");
			this.Property(t => t.Title).HasColumnName("Title");
			this.Property(t => t.Url).HasColumnName("Url");

			// Relationships
		}
	}
}
