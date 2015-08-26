// -----------------------------------------------------------------------
// <copyright file="UserDtoMap.cs">
// Copyright (c) 2012-2015 Andrey Veselov. All rights reserved.
// License: Apache License 2.0
// Contacts: http://andrey.moveax.com andrey@moveax.com
// </copyright>
// -----------------------------------------------------------------------

namespace LoreKeeper.EF6.Tests.DataAccess.Database.DbMaps
{
	using System.Data.Entity.ModelConfiguration;
	using LoreKeeper.EF6.Tests.DataAccess.Database.Dto;

	internal class UserDtoMap : EntityTypeConfiguration<UserDto>
	{
		public UserDtoMap()
		{
			// Primary Key
			this.HasKey(t => new { t.Id });

			// Properties
			this.Property(t => t.Created)
				.IsRequired()
				.HasColumnType("DATETIME2");

			this.Property(t => t.Name)
				.IsRequired()
				.HasMaxLength(64);

			this.Property(t => t.Email)
				.IsRequired()
				.HasMaxLength(64);

			// Table & Column Mappings
			this.ToTable("Users");
			this.Property(t => t.Id).HasColumnName("Id");
			this.Property(t => t.Type).HasColumnName("Type");
			this.Property(t => t.Created).HasColumnName("Created");
			this.Property(t => t.Name).HasColumnName("Name");
			this.Property(t => t.Email).HasColumnName("Email");
			this.Property(t => t.IsDisabled).HasColumnName("IsDisabled");

			// Relationships
			this.HasMany(t => t.Links)
				.WithRequired(t => t.UserProfile)
				.HasForeignKey(t => t.UserProfileId);
		}
	}
}
