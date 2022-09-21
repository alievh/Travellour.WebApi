using Travellour.Core.Entities;
using Travellour.Core.Interfaces;
using Travellour.Data.DAL;

namespace Travellour.Data.Implementations;

public class FriendRepository : Repository<UserFriend>, IFriendRepository
{
    private readonly AppDbContext? _context;
    public FriendRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }
}
