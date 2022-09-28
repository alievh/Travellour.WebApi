using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Travellour.Business.DTOs.Post;
using Travellour.Business.DTOs.StatusCode;
using Travellour.Business.Interfaces;

namespace Travellour.Controllers;

[Authorize(AuthenticationSchemes = "Bearer")]
[Route("api/[controller]")]
[ApiController]
public class PostController : Controller
{
    private readonly IUnitOfWorkService _unitOfWorkService;

    public PostController(IUnitOfWorkService unitOfWorkService)
    {
        _unitOfWorkService = unitOfWorkService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PostGetDto>> PostGetAsync(int id)
    {
        try
        {
            return Ok(await _unitOfWorkService.PostService.GetAsync(id));
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status502BadGateway, new Response { Status = "Error", Message = ex.ToString() });
        }
    }

    [HttpGet("PostGetAll")]
    public async Task<ActionResult<PostGetDto>> PostGetAllAsync()
    {   
        try
        {
            return Ok(await _unitOfWorkService.PostService.GetAllAsync());
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status502BadGateway, new Response { Status = "Error", Message = ex.ToString() });
        }
    }

    [HttpPost("PostCreate")]
    public async Task<ActionResult> PostCreateAsync([FromForm] PostCreateDto postCreateDto)
    {
        try
        {
            await _unitOfWorkService.PostService.CreateAsync(postCreateDto);
            return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Message = "Image updated succesfully!" });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status502BadGateway, new Response { Status = "Error", Message = ex.ToString() });
        }
    }

    [HttpPost("PostDelete")]
    public async Task<ActionResult> PostDeleteAsync(int id)
    {
        try
        {
            await _unitOfWorkService.PostService.DeleteAsync(id);
            return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Message = "Post deleted succesfully!" });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status502BadGateway, new Response { Status = "Error", Message = ex.ToString() });
        }
    }
}
