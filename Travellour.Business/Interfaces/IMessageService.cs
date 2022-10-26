using Travellour.Business.DTOs.MessageDTO;

namespace ChatApp.Business.Services.Interfaces
{
    public interface IMessageService
    {
        Task<GetMessage> SendMessage(MessageDto message);
        Task<List<GetMessage>> GetMessages(string username);
    }
}
