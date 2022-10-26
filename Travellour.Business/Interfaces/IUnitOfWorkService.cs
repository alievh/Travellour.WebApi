using ChatApp.Business.Services.Interfaces;

namespace Travellour.Business.Interfaces;

public interface IUnitOfWorkService
{
    ICommentService CommentService { get; }
    IEventService EventService { get; }
    IForumService ForumService { get; }
    IGroupService GroupService { get; }
    IFriendService FriendService { get; }
    IImageService ImageService { get; }
    ILikeService LikeService { get; }
    INotificationService NotificationService { get; }
    IPostService PostService { get; }
    IUserService UserService { get; }
    IMessageService MessageService { get; }
}
