using Travellour.Core.Entities;
using Travellour.Core.Interfaces;
using Travellour.Data.DAL;

namespace Travellour.Data.Implementations;

public class GroupRepository : Repository<Group>, IGroupRepository
{
    private readonly AppDbContext? _context;
    public GroupRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }
}
