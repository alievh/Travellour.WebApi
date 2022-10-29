using Travellour.Business.DTOs.UserDTO;

namespace Travellour.Business.DTOs.MessageDTO
{
    public class GetMessage
    {
        public int Id { get; set; }
        public string? Content { get; set; }
        public string? SenderDate { get; set; }
        public UserGetDto? SendUser { get; set; }
        public UserGetDto? User { get; set; }
    }
}
