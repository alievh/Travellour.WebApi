using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Travellour.Core.Entities;

namespace Travellour.Business.Hubs
{
    public class ChatHub : Hub<IChatClient>
    {
        private static readonly List<AppUser> _clients = new();
        private readonly Travellour.Data.DAL.AppDbContext _context;
        public ChatHub(
            Travellour.Data.DAL.AppDbContext context,
            IHttpContextAccessor httpContext)
        {
            _context = context;
        }

        public override async Task OnConnectedAsync()
        {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8604 // Possible null reference argument.
            var name = Context.User.Identity.Name;
            var user = await _context.Users.Where(u => u.UserName == name).FirstOrDefaultAsync();
            if (!_clients.Any(u => u.UserName == name))
            {
                _clients.Add(user);
            }
            await Clients.All.GetClients(_clients);
            await Clients.All.GetClient(user);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore CS8604 // Possible null reference argument.
        }
#pragma warning disable CS8765 // Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes).
        public override async Task OnDisconnectedAsync(Exception exception)
#pragma warning restore CS8765 // Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes).
        {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            var name = Context.User.Identity.Name;
            var user = _context.Users.Where(u => u.UserName == name).FirstOrDefault();
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            _clients.RemoveAll(u => u.UserName == name);
            await Clients.All.GetClients(_clients);
#pragma warning disable CS8604 // Possible null reference argument.
            await Clients.All.GetClient(user);
#pragma warning restore CS8604 // Possible null reference argument.
        }
    }
}
