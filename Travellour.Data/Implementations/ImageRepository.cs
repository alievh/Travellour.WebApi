using Travellour.Core.Entities;
using Travellour.Core.Interfaces;
using Travellour.Data.DAL;

namespace Travellour.Data.Implementations;

public class ImageRepository : Repository<Image>, IImageRepository
{
    private readonly AppDbContext? _context;
    public ImageRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }
}
