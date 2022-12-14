// ----------------------------------------------------------------------------------
// <copyright company="Exesoft Inc.">
//	This code was generated by Instant Web API code automation software (https://www.InstantWebAPI.com)
//	Copyright Exesoft Inc. © 2019.  All rights reserved.
// </copyright>
// ----------------------------------------------------------------------------------

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TenantAPI.Entities {
    
    
    public class TenantConfiguration : IEntityTypeConfiguration<Tenant> {
        
        private string _schema = "dbo";
        
        public virtual void Configure(EntityTypeBuilder<Tenant> builder) {
            Configure(builder, _schema);
        }
        
        private void Configure(EntityTypeBuilder<Tenant> builder, string schema) {
            builder.ToTable("Tenant", schema);
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName(@"Id").HasColumnType("uniqueidentifier").IsRequired();
            builder.Property(x => x.CompanyName).HasColumnName(@"CompanyName").HasColumnType("nvarchar").IsRequired().HasMaxLength(500);
            builder.Property(x => x.Website).HasColumnName(@"Website").HasColumnType("nvarchar").IsRequired(false).HasMaxLength(500);
            builder.Property(x => x.DatabaseName).HasColumnName(@"DatabaseName").HasColumnType("nvarchar").IsRequired().HasMaxLength(50);
            builder.Property(x => x.DataSource).HasColumnName(@"DataSource").HasColumnType("nvarchar").IsRequired(false).HasMaxLength(50);
            builder.Property(x => x.UserId).HasColumnName(@"UserId").HasColumnType("nvarchar").IsRequired(false).HasMaxLength(50);
            builder.Property(x => x.Password).HasColumnName(@"Password").HasColumnType("nvarchar").IsRequired(false).HasMaxLength(50);

            //Foreign keys
        }
    }
}

