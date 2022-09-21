using Travellour.Core.Entities;
using Travellour.Core.Interfaces;
using Travellour.Data.DAL;

namespace Travellour.Data.Implementations;

public class CommentRepository : Repository<Comment>, ICommentRepository
{
    private readonly AppDbContext? _context;
    public CommentRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }
}
