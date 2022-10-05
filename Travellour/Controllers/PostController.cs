using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Travellour.Business.DTOs.PostDTO;
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
            return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Message = "Post created succesfully!" });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status502BadGateway, new Response { Status = "Error", Message = ex.ToString() });
        }
    }

    [HttpGet("userPost/{id}")]
    public async Task<ActionResult<List<PostGetDto>>> PostByUserIdAsync(string? id)
    {
        try
        {
            return Ok(await _unitOfWorkService.PostService.GetPostByUserIdAsync(id));
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status502BadGateway, new Response { Status = "Error", Message = ex.ToString() });
        }
    }

    [HttpPut("delete/{id}")]
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

    [HttpPost("likeadd/{id}")]
    public async Task<ActionResult> LikeAddAsync(int id)
    {
        try
        {
            await _unitOfWorkService.LikeService.AddLikeAsync(id);
            return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Message = "Post deleted succesfully!" });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status502BadGateway, new Response { Status = "Error", Message = ex.ToString() });
        }
    }

    [HttpPut("likedelete/{id}")]
    public async Task<ActionResult> LikeDeleteAsync(int id)
    {
        try
        {
            await _unitOfWorkService.LikeService.DeleteLikeAsync(id);
            return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Message = "Post deleted succesfully!" });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status502BadGateway, new Response { Status = "Error", Message = ex.ToString() });
        }
    }
}
