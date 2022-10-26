using Microsoft.AspNetCore.SignalR;
using Travellour.Business.Hubs.Data;

namespace Travellour.Business.Hubs
{
    public class OnlineHub : Hub
    {
        public async Task IsOnline(string id)
        {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            bool exist = OnlineUserSource.OnlineUsers.Exists(n => n == id);
            if (!exist)
            {
                OnlineUserSource.OnlineUsers?.Add(id);
            }

#pragma warning restore CS8602 // Dereference of a possibly null reference.
            await Clients.All.SendAsync("activeUser", OnlineUserSource.OnlineUsers);
        }

        public async Task IsOffline(string id)
        {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            bool exist = OnlineUserSource.OnlineUsers.Exists(n => n == id);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            if (exist)
            {
                OnlineUserSource.OnlineUsers?.Remove(id);
            }
            await Clients.All.SendAsync("activeUser", OnlineUserSource.OnlineUsers);
        }
    }
}
