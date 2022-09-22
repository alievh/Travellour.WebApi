using Microsoft.AspNetCore.Mvc;
using Travellour.Business.DTOs.Event;
using Travellour.Business.DTOs.StatusCode;
using Travellour.Business.Interfaces;

namespace Travellour.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : Controller
    {
        private readonly IUnitOfWorkService _unitOfWorkService;

        public EventController(IUnitOfWorkService unitOfWorkService)
        {
            _unitOfWorkService = unitOfWorkService;
        }

        [HttpGet("EventGetAll")]
        public async Task<ActionResult<List<EventGetDto>>> EventGetAllAsync()
        {
            try
            {
                return await _unitOfWorkService.EventService.GetAllAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status502BadGateway, new Response { Status = "Error", Message = ex.ToString() });
            }
        }
    }
}
