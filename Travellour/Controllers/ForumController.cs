using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Travellour.Business.DTOs.CommentDTO;
using Travellour.Business.DTOs.ForumDTO;
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

    [HttpPost("CommentAdd")]
    public async Task<ActionResult> CommentAddAsync(CommentCreateDto commentCreateDto)
    {
        try
        {
            await _unitOfWorkService.CommentService.CreateCommentAsync(commentCreateDto);
            return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Message = "Post deleted succesfully!" });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status502BadGateway, new Response { Status = "Error", Message = ex.ToString() });
        }
    }

    [HttpPut("CommentDelete/{id}")]
    public async Task<ActionResult> CommentDeleteAsync(int id)
    {
        try
        {
            await _unitOfWorkService.CommentService.DeleteCommentAsync(id);
            return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Message = "Comment deleted succesfully!" });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status502BadGateway, new Response { Status = "Error", Message = ex.ToString() });
        }
    }
}
