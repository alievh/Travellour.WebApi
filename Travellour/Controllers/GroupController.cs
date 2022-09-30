using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Travellour.Business.DTOs.Group;
using Travellour.Business.DTOs.StatusCode;
using Travellour.Business.Interfaces;

namespace Travellour.Controllers;

[Authorize(AuthenticationSchemes = "Bearer")]
[Route("api/[controller]")]
[ApiController]
public class GroupController : Controller
{
    private readonly IUnitOfWorkService _unitOfWorkService;

    public GroupController(IUnitOfWorkService unitOfWorkService)
    {
        _unitOfWorkService = unitOfWorkService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GroupGetDto>> GroupGetAsync(int id)
    {
        try
        {
            return Ok(await _unitOfWorkService.GroupService.GetAsync(id));
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status502BadGateway, new Response { Status = "Error", Message = ex.ToString() });
        }
    }

    [HttpGet("GroupGetAll")]
    public async Task<ActionResult<List<GroupGetDto>>> GroupGetAllAsync()
    {
        try
        {
            return Ok(await _unitOfWorkService.GroupService.GetAllAsyn());
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status502BadGateway, new Response { Status = "Error", Message = ex.ToString() });
        }
    }

    [HttpPost("GroupCreate")]
    public async Task<ActionResult> GroupCreateAsync([FromForm] GroupCreateDto groupCreateDto)
    {
        try
        {
            await _unitOfWorkService.GroupService.CreateAsync(groupCreateDto);
            return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Message = "Group created succesfully!" });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status502BadGateway, new Response { Status = "Error", Message = ex.ToString() });
        }
    }
}
