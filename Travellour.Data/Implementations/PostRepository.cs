using Travellour.Core.Entities;
using Travellour.Core.Interfaces;
using Travellour.Data.DAL;

namespace Travellour.Data.Implementations;

public class PostRepository : Repository<Post>, IPostRepository
{
    private readonly AppDbContext? _context;
    public PostRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }
}
