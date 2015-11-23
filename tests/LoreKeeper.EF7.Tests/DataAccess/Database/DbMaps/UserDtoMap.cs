// -----------------------------------------------------------------------
// <copyright file="UserDtoMap.cs">
// Copyright (c) 2012-2015 Andrey Veselov. All rights reserved.
// License: Apache License 2.0
// Contacts: http://andrey.moveax.com andrey@moveax.com
// </copyright>
// -----------------------------------------------------------------------

namespace LoreKeeper.EF7.Tests.DataAccess.Database.DbMaps
{

    using LoreKeeper.EF7.Tests.DataAccess.Database.Dto;
    using Microsoft.Data.Entity;
    using Microsoft.Data.Entity.Metadata;
    using Microsoft.Data.Entity.Metadata.Builders;

    internal class UserDtoMap
    {
        public void Configure(ModelBuilder modelBuilder)
        {
            EntityTypeBuilder<UserDto> entityTypeBuilder = modelBuilder.Entity<UserDto>();

            // Primary Key
            entityTypeBuilder.HasKey(t => t.Id);

            // Properties
            entityTypeBuilder.Property(t => t.Created)
                .IsRequired();

            entityTypeBuilder.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(64);

            entityTypeBuilder.Property(t => t.Email)
                .IsRequired()
                .HasMaxLength(64);

            this.ConfigureMappingsForSql(entityTypeBuilder);

            // Relationships
            entityTypeBuilder.HasMany(u => u.Links)
                .WithOne(l => l.UserProfile)
                .HasForeignKey(c => c.UserProfileId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        private void ConfigureMappingsForSql(EntityTypeBuilder<UserDto> entityTypeBuilder)
        {
            // Table & Column Mappings
            entityTypeBuilder.ToTable("Users");

            entityTypeBuilder.Property(b => b.Created).HasColumnType("DATETIME2");
        }
    }
}
