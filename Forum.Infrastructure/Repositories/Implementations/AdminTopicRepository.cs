// Copyright (C) TBC Bank. All Rights Reserved.

using Forum.Domain.Enums;
using Forum.Domain.Topics;
using Forum.Infrastructure.Repositories.Abstractions;
using Forum.Infrastructure.Repositories.Implementations.Repositories;
using Forum.Persistance.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Forum.Infrastructure.Repositories.Implementations;

public class AdminTopicRepository : BaseRepository<Topic>, IAdminTopicRepository
{
    public AdminTopicRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<TopicsWithTotalCount> GetAllAsync(int skip, int take, CancellationToken token)
    {
        IQueryable<Topic> topics = _dbSet
            .Include(x => x.User);

        var count = topics.Count();
        var result = await topics
            .OrderByDescending(x => x.ModifiedAt)
            .Skip(skip)
            .Take(take)
            .Select(x => new TopicWithCommentCount
            {
                Topic = x,
                CommentCount = x.Comments.Count()
            })
            .ToListAsync(token);

        return new TopicsWithTotalCount { Topics = result, TotalCount = count};
    }

    public async Task<TopicsWithTotalCount> GetArchivedAsync(int skip, int take, CancellationToken token)
    {
        IQueryable<Topic> topics = _dbSet
            .Include(x => x.User)
            .Where(x => x.Status == TopicStatus.Inactive);

        var count = topics.Count();
        var result = await topics
            .OrderByDescending(x => x.ModifiedAt)
            .Skip(skip)
            .Take(take)
            .Select(x => new TopicWithCommentCount
            {
                Topic = x,
                CommentCount = x.Comments.Count()
            })
            .ToListAsync(token);

        return new TopicsWithTotalCount { Topics = result, TotalCount = count };
    }

    public async Task<TopicsWithTotalCount> GetUsersTopicsWithCommentCountAsync(int userId, int skip, int take, CancellationToken token)
    {
        IQueryable<Topic> topics = _dbSet
            .Include(x => x.User)
            .Where(x => x.UserId == userId);

        var count = topics.Count();

        var result = await topics
            .OrderByDescending(x => x.ModifiedAt)
            .Skip(skip)
            .Take(take)
            .Select(x => new TopicWithCommentCount
            {
                Topic = x,
                CommentCount = x.Comments.Count()
            })
            .ToListAsync(token);

        return new TopicsWithTotalCount { Topics = result, TotalCount = count };
    }

    public async Task<Topic?> GetTopicByIdAsync(int id, CancellationToken token)
    {
        var topic = await _dbSet
            .Include(x => x.User)
            .ThenInclude(x => x.Image)
            .Include(x => x.Comments)
            .ThenInclude(x => x.User)
            .ThenInclude(x => x.Image)
            .SingleOrDefaultAsync(x => x.Id == id, token);

        return topic;
    }

    public async Task<List<TopicWithLatestComment>> GetTopicsWithLatestCommentNoTrackingAsync(CancellationToken token)
    {
        var topics = await _dbSet
            .AsNoTracking()
            .Where(x => x.State != TopicState.Pending && x.Status == TopicStatus.Active)
            .Select(x => new TopicWithLatestComment
            {
                TopicId = x.Id,
                ModifiedAt = x.ModifiedAt,
                LatestComment = x.Comments.OrderByDescending(x => x.CreatedAt).FirstOrDefault()
            })
            .ToListAsync(token);

        return topics;
    }

    public new async Task UpdateAsync(Topic model, CancellationToken token)
    {
        await base.UpdateAsync(model, token);
    }

    public async Task<Topic?> GetTopicByIdWithoutCommentsAsync(int id, CancellationToken token)
    {
        return await base.GetAsync(token, id);
    }

}
