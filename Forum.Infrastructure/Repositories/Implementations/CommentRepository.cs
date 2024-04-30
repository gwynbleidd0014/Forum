// Copyright (C) TBC Bank. All Rights Reserved.

using Forum.Application.Comments;
using Forum.Domain.Comments;
using Forum.Persistance.DbContext;

namespace Forum.Infrastructure.Repositories.Implementations.Repositories;

public class CommentRepository : BaseRepository<Comment>, ICommentRepository
{
    public CommentRepository(AppDbContext context) : base(context)
    {
    }

    public async Task AddCommentAsync(Comment model, CancellationToken token)
    {
        await base.AddAsync(model, token);
    }
    public async Task RemoveCommentAsync(int id, CancellationToken token)
    {
        await base.RemoveAsync(token, id);
    }

    public async Task<Comment?> GetAsync(int id, CancellationToken token)
    {
        return await base.GetAsync(token, id);
    }
}
