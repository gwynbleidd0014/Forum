using FluentAssertions;
using Forum.Application.Errors.CustomErrors;
using Forum.Application.Resourses;
using Forum.Application.Topics;
using Forum.Application.Topics.Request;
using Forum.Application.Topics.Response;
using Forum.Application.Users;
using Forum.Domain.Topics;
using Forum.Infrastructure.Repositories.Abstractions;
using Mapster;
using Microsoft.Extensions.Configuration;
using Moq;

namespace Forum.Tests.Unit;

public class TopicServiceTests
{
    private readonly TopicService _sut;
    private readonly Mock<ITopicRepository> _topicRepositoryMock = new();
    private readonly Mock<IUserService> _userServiceMock = new();
    private readonly Mock<IConfiguration> _configMock = new();

    public TopicServiceTests()
    {
        _sut = new TopicService(_topicRepositoryMock.Object, _userServiceMock.Object, _configMock.Object);
    }

    //GetUserTopicsWithCommentCountAsync
    [Theory]
    [InlineData(0, 10)]
    public async Task GetUserTopicsWithCommentCountAsync_WhenTopicsAreNotPresentAndSkipIsZero_ShouldReturnZeroCountAndEmptyListOfTopics(int skip, int take)
    {
        //Stage
        var topics = new TopicsWithTotalCount { Topics = new List<TopicWithCommentCount>(), TotalCount = 0 };
        _topicRepositoryMock
            .Setup(x => x.GetUsersTopicsWithCommentCountAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(topics);

        //Act
        var result = await _sut.GetUserTopicsWithCommentCountAsync(It.IsAny<int>(), skip, take, CancellationToken.None);

        //assert
        result.TotalCount.Should().Be(0);
        result.Topics.Should().BeEmpty();
        result.Topics.Should().BeOfType<List<TopicResponseModelWithCommentCount>>();
    }

    [Theory]
    [InlineData(3, 3)]
    public async Task GetUserTopicsWithCommentCountAsync_WhenTopicsAreNotPresentAndSkipIsPositive_ShouldThrowException(int skip, int take)
    {
        //Stage
        var topics = new TopicsWithTotalCount { Topics = new List<TopicWithCommentCount>(), TotalCount = 0 };
        _topicRepositoryMock
            .Setup(x => x.GetUsersTopicsWithCommentCountAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(topics);

        //Act
        var result = async () => await _sut.GetUserTopicsWithCommentCountAsync(It.IsAny<int>(), skip, take, CancellationToken.None);

        //assert
        await result.Should().ThrowAsync<NotFound>().WithMessage(ErrorMessages.PageNotFound);
    }

    [Theory]
    [InlineData(1, 0)]
    [InlineData(1, -1)]
    [InlineData(-1, 1)]
    public async Task GetUserTopicsWithCommentCountAsync_WhenTakeIsLessThenOneOrSkipIsNegative_ShouldThrowException(int skip, int take)
    {
        //Stage
        _topicRepositoryMock
            .Setup(x => x.GetUsersTopicsWithCommentCountAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(It.IsAny<TopicsWithTotalCount>());

        //Act
        var result = async () => await _sut.GetUserTopicsWithCommentCountAsync(It.IsAny<int>(), skip, take, CancellationToken.None);

        //assert
        await result.Should().ThrowAsync<Forbiden>().WithMessage(ErrorMessages.NotAllowedPageSize);
    }

    [Theory]
    [InlineData(3)]
    public async Task GetUserTopicsWithCommentCountAsync_WhenTopicsArePresent_ShouldReturnPositiveCountAndNotEmptyListOfTopics(int take)
    {
        //Stage
        var topics = new TopicsWithTotalCount
        {
            Topics = new List<TopicWithCommentCount> { new(), new(), new() },
            TotalCount = 3
        };
        _topicRepositoryMock
            .Setup(x => x.GetUsersTopicsWithCommentCountAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(topics);

        //Act
        var result = await _sut.GetUserTopicsWithCommentCountAsync(It.IsAny<int>(), It.IsAny<int>(), take, CancellationToken.None);

        //assert
        result.TotalCount.Should().BePositive();
        result.Topics.Should().NotBeEmpty();
        result.Topics.Should().BeOfType<List<TopicResponseModelWithCommentCount>>();
    }

    [Theory]
    [InlineData(3)]
    public async Task GetUserTopicsWithCommentCountAsync_WhenCancelationIsRequested_ShouldCancelOperation(int take)
    {
        //Stage
        var cancelationToken = new CancellationTokenSource();
        _topicRepositoryMock
            .Setup(x => x.GetUsersTopicsWithCommentCountAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), cancelationToken.Token))
            .ThrowsAsync(new OperationCanceledException());

        //Act
        var result = async () => await _sut.GetUserTopicsWithCommentCountAsync(It.IsAny<int>(), It.IsAny<int>(), take, cancelationToken.Token);

        //assert
        await result.Should().ThrowAsync<OperationCanceledException>();
    }

    //GetAllAsync
    [Theory]
    [InlineData(0, 10)]
    public async Task GetAllAsync_WhenTopicsAreNotPresentAndSkipIsZero_ShouldReturnZeroCountAndEmptyListOfTopics(int skip, int take)
    {
        //Stage
        var topics = new TopicsWithTotalCount { Topics = new List<TopicWithCommentCount>(), TotalCount = 0 };
        _topicRepositoryMock
            .Setup(x => x.GetAllAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(topics);

        //Act
        var result = await _sut.GetAllAsync(skip, take, CancellationToken.None);

        //assert
        result.TotalCount.Should().Be(0);
        result.Topics.Should().BeEmpty();
        result.Topics.Should().BeOfType<List<TopicResponseModelWithCommentCount>>();
    }

    [Theory]
    [InlineData(3, 3)]
    public async Task GetAllAsync_WhenTopicsAreNotPresentAndSkipIsPositive_ShouldThrowException(int skip, int take)
    {
        //Stage
        var topics = new TopicsWithTotalCount { Topics = new List<TopicWithCommentCount>(), TotalCount = 0 };
        _topicRepositoryMock
            .Setup(x => x.GetAllAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(topics);

        //Act
        var result = async () => await _sut.GetAllAsync(skip, take, CancellationToken.None);

        //assert
        await result.Should().ThrowAsync<NotFound>().WithMessage(ErrorMessages.PageNotFound);
    }

    [Theory]
    [InlineData(1, 0)]
    [InlineData(1, -1)]
    [InlineData(-1, 1)]
    public async Task GetAllAsync_WhenTakeIsLessThenOneOrSkipIsNegative_ShouldThrowException(int skip, int take)
    {
        //Stage
        _topicRepositoryMock
            .Setup(x => x.GetAllAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(It.IsAny<TopicsWithTotalCount>());

        //Act
        var result = async () => await _sut.GetAllAsync(skip, take, CancellationToken.None);

        //assert
        await result.Should().ThrowAsync<Forbiden>().WithMessage(ErrorMessages.NotAllowedPageSize);
    }

    [Theory]
    [InlineData(3)]
    public async Task GetAllAsync_WhenTopicsArePresent_ShouldReturnPositiveCountAndNotEmptyListOfTopics(int take)
    {
        //Stage
        var topics = new TopicsWithTotalCount
        {
            Topics = new List<TopicWithCommentCount> { new(), new(), new() },
            TotalCount = 3
        };
        _topicRepositoryMock
            .Setup(x => x.GetAllAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(topics);

        //Act
        var result = await _sut.GetAllAsync(It.IsAny<int>(), take, CancellationToken.None);

        //assert
        result.TotalCount.Should().BePositive();
        result.Topics.Should().NotBeEmpty();
        result.Topics.Should().BeOfType<List<TopicResponseModelWithCommentCount>>();
    }

    [Theory]
    [InlineData(3)]
    public async Task GetAllAsync_WhenCancelationIsRequested_ShouldCancelOperation(int take)
    {
        //Stage
        var cancelactionToken = new CancellationTokenSource();
        _topicRepositoryMock
            .Setup(x => x.GetAllAsync(It.IsAny<int>(), It.IsAny<int>(), cancelactionToken.Token))
            .ThrowsAsync(new OperationCanceledException());

        //Act
        var result = async () => await _sut.GetAllAsync(It.IsAny<int>(), take, cancelactionToken.Token);

        //assert
        await result.Should().ThrowAsync<OperationCanceledException>();
    }

    //GetArchivedAsync
    [Theory]
    [InlineData(0, 10)]
    public async Task GetArchivedAsync_WhenTopicsAreNotPresentAndSkipIsZero_ShouldReturnZeroCountAndEmptyListOfTopics(int skip, int take)
    {
        //Stage
        var topics = new TopicsWithTotalCount { Topics = new List<TopicWithCommentCount>(), TotalCount = 0 };
        _topicRepositoryMock
            .Setup(x => x.GetArchivedAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(topics);

        //Act
        var result = await _sut.GetArchivedAsync(skip, take, CancellationToken.None);

        //assert
        result.TotalCount.Should().Be(0);
        result.Topics.Should().BeEmpty();
        result.Topics.Should().BeOfType<List<TopicResponseModelWithCommentCount>>();
    }

    [Theory]
    [InlineData(3, 3)]
    public async Task GetArchivedAsync_WhenTopicsAreNotPresentAndSkipIsPositive_ShouldThrowException(int skip, int take)
    {
        //Stage
        var topics = new TopicsWithTotalCount { Topics = new List<TopicWithCommentCount>(), TotalCount = 0 };
        _topicRepositoryMock
            .Setup(x => x.GetArchivedAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(topics);

        //Act
        var result = async () => await _sut.GetArchivedAsync(skip, take, CancellationToken.None);

        //assert
        await result.Should().ThrowAsync<NotFound>().WithMessage(ErrorMessages.PageNotFound);
    }

    [Theory]
    [InlineData(1, 0)]
    [InlineData(1, -1)]
    [InlineData(-1, 1)]
    public async Task GetArchivedAsync_WhenTakeIsLessThenOneOrSkipIsNegative_ShouldThrowException(int skip, int take)
    {
        //Stage
        _topicRepositoryMock
            .Setup(x => x.GetAllAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(It.IsAny<TopicsWithTotalCount>());

        //Act
        var result = async () => await _sut.GetArchivedAsync(skip, take, CancellationToken.None);

        //assert
        await result.Should().ThrowAsync<Forbiden>().WithMessage(ErrorMessages.NotAllowedPageSize);
    }

    [Theory]
    [InlineData(3)]
    public async Task GetArchivedAsync_WhenTopicsArePresent_ShouldReturnPositiveCountAndNotEmptyListOfTopics(int take)
    {
        //Stage
        var topics = new TopicsWithTotalCount
        {
            Topics = new List<TopicWithCommentCount> { new(), new(), new() },
            TotalCount = 3
        };
        _topicRepositoryMock
            .Setup(x => x.GetArchivedAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(topics);

        //Act
        var result = await _sut.GetArchivedAsync(It.IsAny<int>(), take, CancellationToken.None);

        //Assert
        result.TotalCount.Should().BePositive();
        result.Topics.Should().NotBeEmpty();
        result.Topics.Should().BeOfType<List<TopicResponseModelWithCommentCount>>();
    }

    [Theory]
    [InlineData(3)]
    public async Task GetArchivedAsync_WhenCancelationIsRequested_ShouldCancelOperation(int take)
    {
        //Stage
        var cancelationToken = new CancellationTokenSource();
        _topicRepositoryMock
            .Setup(x => x.GetArchivedAsync(It.IsAny<int>(), It.IsAny<int>(), cancelationToken.Token))
            .ThrowsAsync(new OperationCanceledException());

        //Act
        var result = async () => await _sut.GetArchivedAsync(It.IsAny<int>(), take, cancelationToken.Token);

        //Assert
        await result.Should().ThrowAsync<OperationCanceledException>();
    }

    //GetTopicByIdAsync
    [Fact]
    public async Task GetTopicByIdAsync_WhenTopicDoesNotExists_ShouldThrowException()
    {
        //Stage
        _topicRepositoryMock
            .Setup(x => x.GetTopicByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: null);
        //Act
        var result = async () => await _sut.GetTopicByIdAsync(It.IsAny<int>(), CancellationToken.None);
        //Assert
        await result.Should().ThrowAsync<NotFound>().WithMessage(ErrorMessages.TopicNotFound);
    }

    [Fact]
    public async Task GetTopicByIdAsync_WhenTopicExists_ShouldReturnTopic()
    {
        //Stage
        var topic = new Topic() { Name = "Topic", Text = "Topic" };
        _topicRepositoryMock
            .Setup(x => x.GetTopicByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(topic);
        //Act
        var result = await _sut.GetTopicByIdAsync(It.IsAny<int>(), CancellationToken.None);
        //Assert
        result.Should().BeEquivalentTo(topic.Adapt<TopicResponseModel>());
        result.Should().BeOfType<TopicResponseModel>();
    }

    [Fact]
    public async Task GetTopicByIdAsync_WhenCancelationIsRequested_ShouldCancelOperation()
    {
        //Stage
        var cancelationToken = new CancellationTokenSource();
        _topicRepositoryMock
            .Setup(x => x.GetTopicByIdAsync(It.IsAny<int>(), cancelationToken.Token))
            .ThrowsAsync(new OperationCanceledException());
        //Act
        var result = async () => await _sut.GetTopicByIdAsync(It.IsAny<int>(), cancelationToken.Token);
        //Assert
        await result.Should().ThrowAsync<OperationCanceledException>();
    }

    //CreateTopicAsync
    [Fact]
    public async Task CreateTopicAsync_WhenUserWithSuchIdDoesNotExists_ShouldThrowException()
    {
        //Stage
        _userServiceMock.Setup(x => x.IsAbleToCreateTopic(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _userServiceMock.Setup(x => x.UserExists(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);

        //Act
        var result = async () => await _sut.CreateTopicAsync(It.IsAny<TopicRequestModel>(), It.IsAny<int>(), CancellationToken.None);

        //Assert
        await result.Should().ThrowAsync<NotFound>().WithMessage(ErrorMessages.UserNotFound);
    }

    [Theory]
    [InlineData("10")]
    public async Task CreateTopicAsync_WhenUserWithSuchIdExistsAndDoesNotHaveEnoughComments_ShouldThrowException(string count)
    {
        //Stage
        var model = new TopicRequestModel();
        _configMock.Setup(x => x["Constants:ValidCommentCount"]).Returns("10");
        _userServiceMock.Setup(x => x.IsAbleToCreateTopic(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);
        _userServiceMock.Setup(x => x.UserExists(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);

        //Act
        var result = async () => await _sut.CreateTopicAsync(model, It.IsAny<int>(), CancellationToken.None);

        //Assert
        await result.Should().ThrowAsync<Forbiden>().WithMessage(string.Format(ErrorMessages.NotEnoughComments, count));
    }

    [Theory]
    [InlineData("10")]
    public async Task CreateTopicAsync_WhenUserWithSuchIdExistsAndHasEnoughCommentCount_ShouldNotThrowException(string count)
    {
        //Stage
        var model = new TopicRequestModel();
        _configMock.Setup(x => x["Constants:ValidCommentCount"]).Returns(count);
        _userServiceMock.Setup(x => x.IsAbleToCreateTopic(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _userServiceMock.Setup(x => x.UserExists(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);

        //Act
        var result = async () => await _sut.CreateTopicAsync(model, It.IsAny<int>(), CancellationToken.None);

        //Assert
        await result.Should().NotThrowAsync();
    }

    [Theory]
    [InlineData("10")]
    public async Task CreateTopicAsync_WhenCancelationIsRequested_ShouldCancelOperation(string count)
    {
        //Stage
        var model = new TopicRequestModel();
        var cancelationToken = new CancellationTokenSource();
        _userServiceMock
            .Setup(x => x.UserExists(It.IsAny<int>(), cancelationToken.Token))
            .ThrowsAsync(new OperationCanceledException());

        //Act
        var result = async () => await _sut.CreateTopicAsync(model, It.IsAny<int>(), cancelationToken.Token);

        //Assert
        await result.Should().ThrowAsync<OperationCanceledException>();
    }
}
