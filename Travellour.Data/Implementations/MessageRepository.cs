using Microsoft.EntityFrameworkCore;
using Travellour.Core.Entities;
using Travellour.Core.Interfaces;
using Travellour.Data.DAL;

namespace Travellour.Data.Implementations;

public class MessageRepository : Repository<Message>, IMessageRepository
{
    private readonly AppDbContext? _context;
    public MessageRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }
}
