using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Travellour.Business.DTOs.Forum;
using Travellour.Business.DTOs.StatusCode;
using Travellour.Business.Interfaces;

namespace Travellour.Controllers;

[Authorize(AuthenticationSchemes = "Bearer")]
[Route("api/[controller]")]
[ApiController]
public class ForumController : Controller
{
    private readonly IUnitOfWorkService _unitOfWorkService;

    public ForumController(IUnitOfWorkService unitOfWorkService)
    {
        _unitOfWorkService = unitOfWorkService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ForumGetDto>> ForumGetAsync(int id)
    {
        try
        {
            return Ok(await _unitOfWorkService.ForumService.GetAsync(id));
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

    [HttpPost("ForumCreate")]
    public async Task<ActionResult> ForumCreateAsync([FromBody] ForumCreateDto forumCreateDto)
    {
        try
        {
            await _unitOfWorkService.ForumService.CreateAsync(forumCreateDto);
            return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Message = "Forum created succesfully!" });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status502BadGateway, new Response { Status = "Error", Message = ex.ToString() });
        }
    }
}
