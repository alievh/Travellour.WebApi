using AutoMapper;
using ChatApp.Business.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using System.Globalization;
using System.Security.Claims;
using Travellour.Business.DTOs.MessageDTO;
using Travellour.Business.DTOs.UserDTO;
using Travellour.Business.Exceptions;
using Travellour.Business.Hubs;
using Travellour.Core;
using Travellour.Core.Entities;

namespace Travellour.Business.Implementations
{
    public class MessageService : IMessageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHubContext<ChatHub, IChatClient> _hubContext;


        public MessageService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContext, IHubContext<ChatHub, IChatClient> hubContext)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContext;
            _hubContext = hubContext;

        }
        public async Task<GetMessage> SendMessage(MessageDto message)
        {
            var sendUser = await _unitOfWork.UserRepository.GetAsync(u => u.Id == message.SendUserId);
            if (await _unitOfWork.UserRepository.GetAsync(u => u.Id == message.SendUserId) is null)
                throw new NotFoundException("User is not defined");
            GetMessage getMessage = new()
            {
                Content = message.Content,
                SenderDate = DateTime.UtcNow.AddHours(4).ToString("hh:mm tt", CultureInfo.InvariantCulture),
                SendUser = _mapper.Map<UserGetDto>(sendUser),
            };
#pragma warning disable CS8604 // Possible null reference argument.
            await _hubContext.Clients.User(message.SendUserId).ReceiveMessage(getMessage);
#pragma warning restore CS8604 // Possible null reference argument.
            Message newMessage = _mapper.Map<Message>(message);
            newMessage.UserId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _unitOfWork.MessageRepository.CreateAsync(newMessage);
            await _unitOfWork.SaveAsync();
            return getMessage;
        }
        public async Task<List<GetMessage>> GetMessages(string id)
        {
            AppUser user = await _unitOfWork.UserRepository.GetAsync(u => u.Id == id);
            if (user is null)
            {
                throw new NotFoundException("User is not defined");
            }
            var loginUserId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            List<Message> messagesDb = await _unitOfWork.MessageRepository.GetAllAsync(m => m.SenderDate, m => (m.SendUserId == loginUserId && m.UserId == user.Id) || (m.SendUserId == user.Id && m.UserId == loginUserId), "SendUser", "User.ProfileImage");
            return _mapper.Map<List<GetMessage>>(messagesDb);
        }
    }
}
