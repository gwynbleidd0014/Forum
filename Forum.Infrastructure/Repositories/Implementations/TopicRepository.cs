// Copyright (C) TBC Bank. All Rights Reserved.

using Forum.Domain.Enums;
using Forum.Domain.Topics;
using Forum.Infrastructure.Repositories.Abstractions;
using Forum.Persistance.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Forum.Infrastructure.Repositories.Implementations.Repositories;

public class TopicRepository : BaseRepository<Topic>, ITopicRepository
{
    public TopicRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<TopicsWithTotalCount> GetAllAsync(int skip, int take, CancellationToken token)
    {
        IQueryable<Topic> topics = _dbSet
            .Include(x => x.User)
            .Where(x => x.State == TopicState.Show);

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
            .Where(x => x.Status == TopicStatus.Inactive && x.State == TopicState.Show);

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
            .Where(x => x.UserId == userId)
            .Where(x => x.State == TopicState.Show);

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

    public async Task CreateTopicAsync(Topic topic, CancellationToken token)
    {
        await base.AddAsync(topic, token);
    }
}
