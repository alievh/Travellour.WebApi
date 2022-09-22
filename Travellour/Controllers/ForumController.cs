using Microsoft.AspNetCore.Mvc;
using Travellour.Business.DTOs.Forum;
using Travellour.Business.DTOs.StatusCode;
using Travellour.Business.Interfaces;

namespace Travellour.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ForumController : Controller
{
    private readonly IUnitOfWorkService _unitOfWorkService;

    public ForumController(IUnitOfWorkService unitOfWorkService)
    {
        _unitOfWorkService = unitOfWorkService;
    }

    [HttpGet("ForumGet")]
    public async Task<ActionResult<ForumGetDto>> ForumGetAsync(int id)
    {
        try
        {
            return await _unitOfWorkService.ForumService.GetAsync(id);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status502BadGateway, new Response { Status = "Error", Message = ex.ToString() });
        }
    }


    [HttpGet("ForumGetAll")]
    public async Task<ActionResult<List<ForumGetDto>>> ForumGetAllAsync()
    {
        try
        {
            return await _unitOfWorkService.ForumService.GetAllAsync();
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status502BadGateway, new Response { Status = "Error", Message = ex.ToString() });
        }
    }
}
