using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Travellour.Business.DTOs.StatusCode;
using Travellour.Business.DTOs.User;
using Travellour.Business.Interfaces;

namespace Travellour.Controllers;

[Authorize(AuthenticationSchemes = "Bearer")]
[Route("api/[controller]")]
[ApiController]
public class UserController : Controller
{
    private readonly IUnitOfWorkService _unitOfWorkService;

    public UserController(IUnitOfWorkService unitOfWorkService)
    {
        _unitOfWorkService = unitOfWorkService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserGetDto>> GetUserAsync(string id)
    {
        try
        {
            return Ok(await _unitOfWorkService.UserService.GetAsync(id));
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status502BadGateway, new Response { Status = "Error", Message = ex.ToString() });
        }
    }

    [HttpPost("userUpdate")]
    public async Task<ActionResult> UserUpdateAsyncs(UserUpdateDto userUpdateDto)
    {
        try
        {
            #pragma warning disable CS8604 // Possible null reference argument.
            await _unitOfWorkService.UserService.UpdateAsync(userUpdateDto);
            #pragma warning restore CS8604 // Possible null reference argument.
            return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Message = "Account updated succesfully!" });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status502BadGateway, new Response { Status = "Error", Message = ex.ToString() });
        }
    }

    [HttpPost("changeProfilePhoto")]
    public async Task<ActionResult> ChangeProfilePhoto([FromForm] ProfilePhotoDto profilePhotoDto)
    {
        try
        {
        #pragma warning disable CS8604 // Possible null reference argument.
            await _unitOfWorkService.UserService.ChangeProfilePhotoAsync(profilePhotoDto);
        #pragma warning restore CS8604 // Possible null reference argument.
            return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Message = "Image updated succesfully!" });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status502BadGateway, new Response { Status = "Error", Message = ex.ToString() });
        }
    }

    [HttpPost("changeCoverPhoto")]
    public async Task<ActionResult> ChangeCoverPhoto([FromForm] CoverPhotoDto coverPhotoDto)
    {
        try
        {
        #pragma warning disable CS8604 // Possible null reference argument.
            await _unitOfWorkService.UserService.ChangeCoverPhotoAsync(coverPhotoDto);
        #pragma warning restore CS8604 // Possible null reference argument.
            return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Message = "Image updated succesfully!" });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status502BadGateway, new Response { Status = "Error", Message = ex.ToString() });
        }
    }

    [HttpGet("friendSuggestion")]
    public async Task<ActionResult> GetFriendSuggestionAsync()
    {
        try
        {
            return Ok(await _unitOfWorkService.UserService.GetFriendSuggestionAsync());
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status502BadGateway, new Response { Status = "Error", Message = ex.ToString() });
        }
    }
}
