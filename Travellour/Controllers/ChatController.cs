using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Travellour.Business.DTOs.MessageDTO;
using Travellour.Business.DTOs.StatusCode;
using Travellour.Business.Interfaces;

namespace Travellour.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : Controller
    {
        private readonly IUnitOfWorkService _unitOfWork;
        public ChatController(IUnitOfWorkService unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpPost("messageSend")]
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
        [HttpGet("{id}")]
        public async Task<ActionResult<List<MessageDto>>> GetMessages(string id)
        {
            try
            {
                return Ok(await _unitOfWork.MessageService.GetMessages(id));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status502BadGateway, new Response { Status = "Error", Message = ex.Message });
            }
        }
    }
}
