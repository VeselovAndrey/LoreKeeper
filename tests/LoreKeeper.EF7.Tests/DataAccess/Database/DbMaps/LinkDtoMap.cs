// -----------------------------------------------------------------------
// <copyright file="LinkDtoMap.cs">
// Copyright (c) 2013-2015 Andrey Veselov. All rights reserved.
// License: Apache License 2.0
// Contacts: http://andrey.moveax.com andrey@moveax.com
// </copyright>
// -----------------------------------------------------------------------

namespace LoreKeeper.EF7.Tests.DataAccess.Database.DbMaps
{
    using LoreKeeper.EF7.Tests.DataAccess.Database.Dto;
    using Microsoft.Data.Entity;
    using Microsoft.Data.Entity.Metadata.Builders;

    class LinkDtoMap
    {
        public void Configure(ModelBuilder modelBuilder)
        {
            EntityTypeBuilder<LinkDto> entityTypeBuilder = modelBuilder.Entity<LinkDto>();

            // Primary Key
            entityTypeBuilder.HasKey(t => t.Id);

            // Properties
            entityTypeBuilder.Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(64);

            entityTypeBuilder.Property(t => t.Url)
                .IsRequired()
                .HasMaxLength(128);

            this.ConfigureMappingsForSql(entityTypeBuilder);
        }

        private void ConfigureMappingsForSql(EntityTypeBuilder<LinkDto> entityTypeBuilder)
        {
            // Table & Column Mappings
            entityTypeBuilder.ToTable("Links");
        }
    }
}
