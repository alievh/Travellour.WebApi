using Travellour.Core.Interfaces;

namespace Travellour.Core;

public interface IUnitOfWork
{
    IUserRepository UserRepository { get; }
    IEventRepository EventRepository { get; }
    ICommentRepository CommentRepository { get; }
    IForumRepository ForumRepository { get; }
    IFriendRepository FriendRepository { get; }
    IGroupRepository GroupRepository { get; }
    IImageRepository ImageRepository { get; }
    ILikeRepository LikeRepository { get; }
    INotificationRepository NotificationRepository { get; }
    IPostRepository PostRepository { get; }
}
