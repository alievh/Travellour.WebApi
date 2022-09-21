using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travellour.Core.Entities;
using Travellour.Core.Interfaces;
using Travellour.Data.DAL;

namespace Travellour.Data.Implementations;

public class NotificationRepository : Repository<Notification>, INotificationRepository
{
    private readonly AppDbContext _context;
    public NotificationRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }
}
