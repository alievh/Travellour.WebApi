using Travellour.Business.DTOs.MessageDTO;
using Travellour.Core.Entities;

namespace Travellour.Business.Hubs;

public interface IChatClient
{
    Task ReceiveMessage(GetMessage message);
    Task GetClients(List<AppUser> clients);
    Task GetConnectionId(string connectionId);
    Task GetClient(AppUser user);
}
