using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BasicApi.Items.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace BasicApi.Data.Configurations
{
    public abstract class AuditableEntityBaseConfiguration<TEntity> : EntityBaseConfiguration<TEntity> where TEntity : AuditableEntityBase
    {
        public override void Configure(EntityTypeBuilder<TEntity> builder)
        {
            base.Configure(builder);

            builder.Property(x => x.CreatedBy)
                .HasColumnName("CreatedBy")
                .HasColumnType("uniqueidentifier");

            builder.Property(x => x.ModifiedBy)
                .HasColumnName("ModifiedBy")
                .HasColumnType("uniqueidentifier");
        }
    }
}
