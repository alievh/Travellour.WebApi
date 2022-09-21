using Travellour.Core.Entities;
using Travellour.Core.Interfaces;
using Travellour.Data.DAL;

namespace Travellour.Data.Implementations;

public class EventRepository : Repository<Event>, IEventRepository
{
    private readonly AppDbContext _context;
    public EventRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }
}
