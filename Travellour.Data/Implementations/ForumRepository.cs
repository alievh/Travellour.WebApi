using Travellour.Core.Entities;
using Travellour.Core.Interfaces;
using Travellour.Data.DAL;

namespace Travellour.Data.Implementations;

public class ForumRepository : Repository<Forum>, IForumRepository
{
    private readonly AppDbContext? _context;
    public ForumRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }
}
