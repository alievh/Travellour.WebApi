using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Travellour.Business.DTOs.Event;
using Travellour.Business.DTOs.StatusCode;
using Travellour.Business.Interfaces;

namespace Travellour.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
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
                return Ok(await _unitOfWorkService.EventService.GetAllAsync());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status502BadGateway, new Response { Status = "Error", Message = ex.ToString() });
            }
        }

        [HttpPost("EventCreate")]
        public async Task<ActionResult> EventCreateAsync([FromForm] EventCreateDto eventCreateDto)
        {
            try
            {
                await _unitOfWorkService.EventService.CreateAsync(eventCreateDto);
                return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Message = "Event created succesfully!" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status502BadGateway, new Response { Status = "Error", Message = ex.ToString() });
            }
        }
    }
}
