// Copyright (C) TBC Bank. All Rights Reserved.

using Forum.Domain.Images;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Forum.Persistance.Configurations;

public class ImageConfiguration : IEntityTypeConfiguration<Image>
{
    public void Configure(EntityTypeBuilder<Image> builder)
    {
        builder.Property(x => x.Path).IsRequired().HasMaxLength(1000);
        builder.HasOne(x => x.User).WithOne(x => x.Image).HasForeignKey<Image>(x => x.Id);
    }
}
