using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Travellour.Business.DTOs.NotificationDTO;
using Travellour.Business.DTOs.StatusCode;
using Travellour.Business.Interfaces;

namespace Travellour.Controllers;

[Authorize(AuthenticationSchemes = "Bearer")]
[Route("api/[controller]")]
[ApiController]
public class NotificationController : Controller
{
    private readonly IUnitOfWorkService _unitOfWorkService;

    public NotificationController(IUnitOfWorkService unitOfWorkService)
    {
        _unitOfWorkService = unitOfWorkService;
    }

    [HttpGet("notificationgetall")]
    public async Task<ActionResult> NotificationGetAllAsync()
    {
        try
        {
            return Ok(await _unitOfWorkService.NotificationService.GetAllNotificationAsync());
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status502BadGateway, new Response { Status = "Error", Message = ex.ToString() });
        }
    }

    [HttpPost("notificationCreate")]
    public async Task<ActionResult> NotificationCreateAsync(NotificationCreateDto notificationCreateDto)
    {
        try
        {
            await _unitOfWorkService.NotificationService.CreateNotificationAsync(notificationCreateDto);
            return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Message = "Group created succesfully!" });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status502BadGateway, new Response { Status = "Error", Message = ex.ToString() });
        }
    }

    [HttpPost("notificationChangeStatus/{id}")]
    public async Task<ActionResult> NotificationChangeStatusAsync(int id)
    {
        try
        {
            await _unitOfWorkService.NotificationService.ChangeNotificationSatatusAsync(id);
            return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Message = "Group created succesfully!" });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status502BadGateway, new Response { Status = "Error", Message = ex.ToString() });
        }
    }
}
