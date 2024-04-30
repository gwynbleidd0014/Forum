// Copyright (C) TBC Bank. All Rights Reserved.

using Forum.Domain.Comments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Forum.Persistance.Configurations;

public class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.Property(x => x.Text).IsRequired().HasMaxLength(100);
        builder.HasOne(x => x.User).WithMany(x => x.Comments).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(x => x.Topic).WithMany(x => x.Comments).HasForeignKey(x => x.TopicId).OnDelete(DeleteBehavior.NoAction);
        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}
