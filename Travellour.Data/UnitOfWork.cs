using Travellour.Core;
using Travellour.Core.Interfaces;
using Travellour.Data.DAL;
using Travellour.Data.Implementations;

namespace Travellour.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private IUserRepository _userRepository;
    private IEventRepository _eventRepository;
    private ICommentRepository _commentRepository;
    private IForumRepository _forumRepository;
    private IFriendRepository _friendRepository;
    private IGroupRepository _groupRepository;
    private IImageRepository _imageRepository;
    private ILikeRepository _likeRepository;
    private INotificationRepository _notificationRepository;
    private IPostRepository _postRepository;
    private IMessageRepository _messageRepository;

    #pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }
    #pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public IUserRepository UserRepository => _userRepository ??= new UserRepository(_context);
    public IEventRepository EventRepository => _eventRepository ??= new EventRepository(_context);

    public ICommentRepository CommentRepository => _commentRepository ??= new CommentRepository(_context);

    public IForumRepository ForumRepository => _forumRepository ??= new ForumRepository(_context);

    public IFriendRepository FriendRepository => _friendRepository ??= new FriendRepository(_context);

    public IGroupRepository GroupRepository => _groupRepository ??= new GroupRepository(_context);

    public IImageRepository ImageRepository => _imageRepository ??= new ImageRepository(_context);

    public ILikeRepository LikeRepository => _likeRepository ??= new LikeRepository(_context);

    public INotificationRepository NotificationRepository => _notificationRepository ??= new NotificationRepository(_context);

    public IPostRepository PostRepository => _postRepository ??= new PostRepository(_context);
    public IMessageRepository MessageRepository => _messageRepository ??= new MessageRepository(_context);
    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}
