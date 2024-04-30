// Copyright (C) TBC Bank. All Rights Reserved.

using Forum.Application.Comments.Response;
using Forum.Application.Images.Request;
using Forum.Application.Topics.Request;
using Forum.Application.Topics.Response;
using Forum.Application.Users.Request;
using Forum.Application.Users.Response;
using Forum.Domain.Comments;
using Forum.Domain.Images;
using Forum.Domain.Topics;
using Forum.Domain.Users;
using Mapster;
using Microsoft.Extensions.DependencyInjection;

namespace Forum.Common.Mapping;

public static class CommonMapping
{
    public static void AddCustomMapping(this IServiceCollection _)
    {
        //User Mapping
        TypeAdapterConfig<UserRegisterModel, User>
            .NewConfig();

        TypeAdapterConfig<User, UserResponseModel>
            .NewConfig()
            .Map(des => des.ImgPath, src => src.Image.Path);

        //Comment Mapping
        TypeAdapterConfig<Comment, CommentResponseModel>
            .NewConfig()
            .Map(des => des.Author, src => src.User.UserName)
            .Map(des => des.AuthorImgPath, src => src.User.Image.Path)
            .Map(des => des.DaysOld, src => (DateTime.UtcNow - src.CreatedAt).TotalDays);

        //Topic Mapping
        TypeAdapterConfig<Topic, TopicResponseModel>
            .NewConfig()
            .Map(des => des.Comments, src => src.Comments.Adapt<List<CommentResponseModel>>())
            .Map(des => des.Author, src => src.User.UserName)
            .Map(des => des.AuthorId, src => src.UserId)
            .Map(des => des.AuthorImgPath, src => src.User.Image.Path)
            .Map(des => des.DaysOld, src => (DateTime.UtcNow - src.CreatedAt).TotalDays);
        TypeAdapterConfig<Topic, TopicAdminResponseModel>
            .NewConfig()
            .Map(des => des.Comments, src => src.Comments.Adapt<List<CommentResponseModel>>())
            .Map(des => des.Author, src => src.User.UserName)
            .Map(des => des.AuthorId, src => src.UserId)
            .Map(des => des.AuthorImgPath, src => src.User.Image.Path)
            .Map(des => des.DaysOld, src => (DateTime.UtcNow - src.CreatedAt).TotalDays);

        TypeAdapterConfig<TopicCreateModel, Topic>
            .NewConfig()
            .Map(des => des.UserId, src => src.UserId);

        TypeAdapterConfig<TopicWithCommentCount, TopicResponseModelWithCommentCount>
            .NewConfig()
            .Map(des => des.Id, src => src.Topic.Id)
            .Map(des => des.Name, src => src.Topic.Name)
            .Map(des => des.Text, src => src.Topic.Text)
            .Map(des => des.AuthorId, src => src.Topic.UserId)
            .Map(des => des.Author, src => src.Topic.User.UserName)
            .Map(des => des.CreatedAt, src => src.Topic.CreatedAt)
            .Map(des => des.Status, src => src.Topic.Status);

        TypeAdapterConfig<TopicWithCommentCount, TopicAdminResponseModelWithCommentCount>
            .NewConfig()
            .Map(des => des.Id, src => src.Topic.Id)
            .Map(des => des.Name, src => src.Topic.Name)
            .Map(des => des.Text, src => src.Topic.Text)
            .Map(des => des.AuthorId, src => src.Topic.UserId)
            .Map(des => des.Author, src => src.Topic.User.UserName)
            .Map(des => des.CreatedAt, src => src.Topic.CreatedAt)
            .Map(des => des.Status, src => src.Topic.Status)
            .Map(des => des.State, src => src.Topic.State);

        TypeAdapterConfig<Topic, TopicAdminResponseModelWithCommentCount>
            .NewConfig()
            .Map(des => des.Author, src => src.User.UserName)
            .Map(des => des.CommentCount, src => src.Comments.Count);

        //Image Mapping
        TypeAdapterConfig<ImageCreateModel, Image>
            .NewConfig()
            .Map(des => des.Id, src => src.UserId);

    }
}
