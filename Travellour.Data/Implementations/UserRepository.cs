using Travellour.Core.Entities;
using Travellour.Core.Interfaces;
using Travellour.Data.DAL;

namespace Travellour.Data.Implementations;

public class UserRepository : Repository<AppUser>, IUserRepository
{
    private readonly AppDbContext? _context;
    public UserRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }
}
