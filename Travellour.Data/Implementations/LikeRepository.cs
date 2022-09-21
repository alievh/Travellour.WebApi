using Travellour.Core.Entities;
using Travellour.Core.Interfaces;
using Travellour.Data.DAL;

namespace Travellour.Data.Implementations;

public class LikeRepository : Repository<Like>, ILikeRepository
{
    private readonly AppDbContext? _context;
    public LikeRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }
}
