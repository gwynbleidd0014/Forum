// Copyright (C) TBC Bank. All Rights Reserved.

using Forum.Domain.Topics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Forum.Persistance.Configurations;

public class TopicConfiguration : IEntityTypeConfiguration<Topic>
{
    public void Configure(EntityTypeBuilder<Topic> builder)
    {
        builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
        builder.Property(x => x.Text).IsRequired().HasMaxLength(1000);
        builder.HasOne(x => x.User).WithMany(x => x.Topics).HasForeignKey(x => x.UserId);
        builder.Property(x => x.State).HasConversion<string>();
        builder.Property(x => x.Status).HasConversion<string>();
    }
}
