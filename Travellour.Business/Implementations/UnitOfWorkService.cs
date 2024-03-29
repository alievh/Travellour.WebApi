﻿using AutoMapper;
using ChatApp.Business.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Travellour.Business.Hubs;
using Travellour.Business.Interfaces;
using Travellour.Core;
using Travellour.Core.Entities;

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
    private IMessageService _messageService;

    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IHostEnvironment _hostEnvironment;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly UserManager<AppUser> _userManager;
    private readonly IHubContext<ChatHub, IChatClient> _hubContext;

    public UnitOfWorkService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor, IHostEnvironment hostEnvironment, UserManager<AppUser> userManager, IHubContext<ChatHub, IChatClient> hubContext)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
        _hostEnvironment = hostEnvironment;
        _userManager = userManager;
        _hubContext = hubContext;
    }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public IUserService UserService => _userService ??= new UserService(_unitOfWork, _mapper, _httpContextAccessor, _hostEnvironment, _userManager);

    public ICommentService CommentService => _commentService ??= new CommentService(_unitOfWork, _httpContextAccessor, _mapper);

    public IForumService ForumService => _forumService ??= new ForumService(_unitOfWork, _mapper, _httpContextAccessor);

    public IFriendService FriendService => _friendService ??= new FriendService(_unitOfWork, _httpContextAccessor, _mapper);

    public IGroupService GroupService => _groupService ??= new GroupService(_unitOfWork, _mapper, _httpContextAccessor, _hostEnvironment);

    public IImageService ImageService => _imageService ??= new ImageService();

    public ILikeService LikeService => _likeService ??= new LikeService(_unitOfWork, _httpContextAccessor);

    public INotificationService NotificationService => _notificationService ??= new NotificationService(_unitOfWork, _httpContextAccessor, _mapper);

    public IPostService PostService => _postService ??= new PostService(_unitOfWork, _mapper, _httpContextAccessor, _hostEnvironment);

    public IEventService EventService => _eventService ??= new EventService(_unitOfWork, _mapper, _httpContextAccessor, _hostEnvironment);
    public IMessageService MessageService => _messageService ??= new MessageService(_unitOfWork, _mapper, _httpContextAccessor, _hubContext);
}
