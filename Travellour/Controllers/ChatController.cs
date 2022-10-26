using Microsoft.AspNetCore.Mvc;
using Travellour.Business.DTOs.MessageDTO;
using Travellour.Business.DTOs.StatusCode;
using Travellour.Business.Interfaces;

namespace Travellour.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : Controller
    {
        private readonly IUnitOfWorkService _unitOfWork;
        public ChatController(IUnitOfWorkService unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpPost]
        public async Task<ActionResult<GetMessage>> SendMessage(MessageDto message)
        {
            try
            {
                return Ok(await _unitOfWork.MessageService.SendMessage(message));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status502BadGateway, new Response { Status = "Error", Message = ex.Message });
            }
        }
        [HttpGet("{username}")]
        public async Task<ActionResult<List<MessageDto>>> GetMessages(string username)
        {
            try
            {
                return Ok(await _unitOfWork.MessageService.GetMessages(username));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status502BadGateway, new Response { Status = "Error", Message = ex.Message });
            }
        }
    }
}
