﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Travellour.Business.Interfaces;
using Travellour.Core;

namespace Travellour.Business.Implementations;

public class UnitOfWorkService : IUnitOfWorkService
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private IUserService _userService;
    private ICommentService _commentService;
    private IEventService _eventService;
    private IForumService _forumService;
    private IFriendService _friendService;
    private IImageService _imageService;
    private ILikeService _likeService;
    private INotificationService _notificationService;
    private IPostService _postService;
    private IGroupService _groupService;

    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UnitOfWorkService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
    }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public IUserService UserService => _userService ??= new UserService(_unitOfWork, _mapper, _httpContextAccessor);

    public ICommentService CommentService => _commentService ??= new CommentService();

    public IForumService ForumService => _forumService ??= new ForumService(_unitOfWork, _mapper);

    public IFriendService FriendService => _friendService ??= new FriendService();

    public IGroupService GroupService => _groupService ??= new GroupService();

    public IImageService ImageService => _imageService ??= new ImageService();

    public ILikeService LikeService => _likeService ??= new LikeService();

    public INotificationService NotificationService => _notificationService ??= new NotificationService();

    public IPostService PostService => _postService ??= new PostService(_unitOfWork, _mapper, _httpContextAccessor);

    public IEventService EventService => _eventService ??= new EventService(_unitOfWork, _mapper);
}
