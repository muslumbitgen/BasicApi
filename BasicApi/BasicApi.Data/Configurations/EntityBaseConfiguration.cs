using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BasicApi.Items.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace BasicApi.Data.Configurations
{
    public abstract class EntityBaseConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : EntityBase
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.Property(x => x.Id)
                .HasColumnName("Id")
                .HasColumnType("uniqueidentifier");

            builder.Property(x => x.IsActive)
                .HasColumnName("IsActive")
                .HasDefaultValue(true);

            builder.Property(x => x.CreatedAt)
                .HasColumnName("CreatedAt")
                .HasColumnType("datetime");

            builder.Property(x => x.ModifiedAt)
                .HasColumnName("ModifiedAt")
                .HasColumnType("datetime");
        }
    }
}
